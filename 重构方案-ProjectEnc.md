# ProjectEnc 重构方案（分阶段执行计划）

> 依据《代码审查报告-ProjectEnc.md》制定，本文件为**执行计划**，确认后逐阶段落地。
> 编制日期：2026-10-12

---

## 〇、决策基线（已确认）

| 决策项 | 选择 |
|---|---|
| 重构范围 | **P0 + P1**（先修高危正确性/安全，再做健壮性与安全加固；不做架构级重构） |
| 目标框架 | **保持 .NET Framework 4.5.2**（零迁移风险） |
| 序列化 | **迁移 JSON，并自动兼容旧的二进制工程文件** |
| 依赖策略 | **零 NuGet 依赖**，仅使用框架内置能力 |

## 环境与验证约束（重要）

- 本机**未安装 Visual Studio / MSBuild / .NET SDK**，且项目按 GAC 方式引用 DevExpress 22.2（csproj 无 HintPath），因此**我无法在本地编译**。
- 因此每阶段的验证分为两级：
  1. **静态验证（我来做）**：逐文件 diff 审查、调用点一致性核对、编译期隐患人工核查、字符串/常量一致性检查。
  2. **功能验证（需你在 VS 中做）**：我在每阶段末尾提供**可执行的测试清单**，由你编译并逐项确认。
- **回滚策略**：每阶段一个独立 commit（并在阶段完成时打 tag），任一步骤可单独回退。

---

## 阶段 1 ｜ P0：正确性与安全（会损坏用户程序/存在安全风险）

> 目标：堵住"操作失败却报成功""参数导致工具失败""进程卡死""备份不可靠""改名后路径错位""目录混入列表"六个高危点。
> 影响文件：`Bean/Project.cs`、`MainForm.cs`、`Control/PBaseControl.cs`、`Control/FileControl.cs`

### 步骤 1.1 — 重写 `Project.Exec`：退出码校验 + 防死锁 + 超时 + 资源释放
- **对应问题**：C1（失败报成功）、C2（死锁/挂起/泄漏）
- **位置**：`Bean/Project.cs:89-113`，调用点 `Merge:64`、`Protect:76`、`Package:85`
- **修改内容**：
  1. 用 `using (var process = new Process {...})` 包裹，确保句柄释放。
  2. 改用 `OutputDataReceived` / `ErrorDataReceived` 异步事件收集 stdout/stderr（替代只读 stdout），消除缓冲区写满导致的**死锁**。
  3. `WaitForExit(timeoutMs)`（默认 10 分钟）；超时则 `process.Kill()` 并抛出异常。
  4. **校验 `process.ExitCode`**：非 0 时抛出异常，消息包含退出码与 stderr 摘要；**成功消息仅在退出码为 0 时输出**。
  5. 调用前校验工具可执行文件存在（`File.Exists(exePath)`），缺失时给出明确提示。
  6. 移除 `StandardInput.WriteLine()` 这一经验性 hack（无实际用途）。
- **预期效果**：工具失败时不再误报"合并/加密/打包成功"；不再死锁或永久无响应；进程句柄不泄漏。
- **验证方式**：
  - 静态：确认 `Merge/Protect/Package` 三处异常都能被 `MainForm` 的 `try/catch` 捕获并提示；确认无遗留的同步 `ReadToEnd`。
  - 功能（你）：① 正常合并 → 提示成功；② 故意将 `Tools/ILMerge/ILMerge.exe` 改名 → 应提示"工具不存在"；③ 用错误参数触发非 0 退出 → 应提示失败并显示原因。

### 步骤 1.2 — 命令行参数转义
- **对应问题**：C3（含空格路径失败 / 参数注入）
- **位置**：`Bean/Project.cs:62`（`Merge`）
- **修改内容**：新增私有辅助 `Quote(string path)`（去除尾部斜杠 → 用双引号包裹 → 转义内部引号）；`/out:` 目标、主 exe、每个待合并 DLL 全部经 `Quote` 处理后再拼接。
- **预期效果**：`C:\Program Files\...` 等含空格路径可正确执行；杜绝参数注入。
- **验证方式**：
  - 静态：审查拼接结果，确认每个路径独立成参。
  - 功能（你）：用位于含空格目录下的测试工程执行合并，确认成功。

### 步骤 1.3 — 备份机制修正
- **对应问题**：C11（备份不刷新、拼写笔误）
- **位置**：`Bean/Project.cs:158-165`（`BackUpExeFile`）、`Bean/Project.cs:130`（`GetPackageConfig` 中的 `.bakup` 引用）
- **修改内容**：
  1. 扩展名 `.bakup` → `.backup`，**同步修改 `GetPackageConfig` 中的引用**（两处必须一致）。
  2. `BackUpExeFile` 改为**每次操作前覆盖式备份**（备份内容 = 本次操作前的状态），而非"不存在才创建"。
  3. 备份前校验 `ExeFile` 存在；目标已存在时明确覆盖策略。
- **预期效果**：回滚得到的是"本次操作前"的版本，而非首次的旧版本。
- **验证方式**：
  - 静态：全局搜索确认已无 `.bakup` 残留、两处扩展名一致。
  - 功能（你）：连续执行两次合并，检查 `.backup` 内容为最近一次操作前的 exe。

### 步骤 1.4 — 工程重命名后的路径一致性
- **对应问题**：D3（改名后保存路径错位 / 抛异常）
- **位置**：`Control/PBaseControl.cs:87-93`（实时写 `project.Name`）、`MainForm.cs:98`（用 `Name` 拼保存路径）
- **修改内容**（推荐方案）：
  1. 保存时不再"实时写回"，改为：以 `Project.Id` 在 `Projects` 目录下定位**当前磁盘目录**（避免依赖可能已变更的 `Name`）。
  2. 若 `Name` 与磁盘目录名不一致 → 先 `Directory.Move(旧目录, 新目录)`（目标已存在则报错并中止），再写入工程文件。
  3. `PBaseControl` 对"已存在工程"不直接改写 `project.Name`，改由保存流程统一提交（保留"可改名"能力）。
- **预期效果**：改名后保存不再写错目录或抛未捕获异常。
- **验证方式**：
  - 静态：核对 `Save`/`Init`/`Load` 三条路径的目录解析逻辑一致。
  - 功能（你）：打开已有工程 → 改名 → 保存 → 重新加载，确认目录已重命名、内容正确、无异常；再验证改名撞车（已存在同名目录）时给出友好提示。

### 步骤 1.5 — 目录节点不进入合并/排除列表
- **对应问题**：D4（勾选目录被当作待合并文件）
- **位置**：`Control/FileControl.cs:55-58`、`Control/FileControl.cs:152-160`（`UpdateSelectFiles`）
- **修改内容**：区分"目录节点"与"文件节点"（节点已用 `Tag` 存路径，可据此判断 `Directory.Exists`/`File.Exists`）；`UpdateSelectFiles` **仅收集文件节点**；目录勾选仅用于向下联动子项。
- **预期效果**：`MergeDlls` / `PackageIgnores` 中只含文件路径，不再把目录路径传给 ILMerge。
- **验证方式**：
  - 静态：确认收集逻辑过滤目录。
  - 功能（你）：勾选整个文件夹 → 保存工程 → 检查列表中只有文件、无目录。

### 阶段 1 收尾
- 提交为一个 commit，信息如：`[修复] P0：进程退出码/参数转义/备份/重命名/目录混入`。
- 输出《阶段 1 总结》，附上述功能测试清单，**待你确认后进入阶段 2**。

---

## 阶段 2 ｜ P1：健壮性与安全加固

> 目标：全局异常兜底、日志体系、密码加密、序列化迁移、输入校验、文件操作保护、编码统一。
> 影响文件：`Program.cs`、`Bean/Project.cs`、`Helper/*`、`MainForm.cs`、`Control/PBaseControl.cs`、`Form/DialogEditFileForm.cs`、`ProjectEnc.csproj`

### 步骤 2.1 — 全局异常兜底 + 日志体系
- **对应问题**：C7（异常入口不全）、C8（暴露堆栈、无日志）
- **修改内容**：
  1. `Program.cs` 增补 `AppDomain.CurrentDomain.UnhandledException` 与 `TaskScheduler.UnobservedTaskException` 处理。
  2. 新增 `Helper/LogHelper.cs`（零依赖）：写 `Logs/yyyy-MM-dd.log`，含时间/级别/消息/堆栈，线程安全。
  3. `MsgHelper.Error(Exception)` 改为：弹窗仅显示友好提示 + 建议查看日志；完整堆栈写入日志。
- **预期效果**：任何线程的未处理异常都有记录、不静默崩溃；用户不再看到堆栈。
- **验证方式**：静态 + 功能（你）：人为触发一次异常，确认弹窗友好且 `Logs/` 下生成日志文件。

### 步骤 2.2 — 密码加密存储
- **对应问题**：C4（明文密码）
- **修改内容**：新增 `Helper/CryptoHelper.cs`，使用 `System.Security.Cryptography.ProtectedData`（`CurrentUser` 范围）加/解密；`Project.Password` 在序列化前加密、读取后解密（在读写边界统一处理，业务代码仍用明文）。
- **预期效果**：工程文件中密码为密文，程序内仍可正确注入到 `.iss`。
- **验证方式**：静态 + 功能（你）：查看工程文件为密文；打包时密码生效。

### 步骤 2.3 — 序列化迁移 JSON（并兼容旧文件）
- **对应问题**：B4（BinaryFormatter RCE + 弃用）
- **修改内容**：
  1. 新增 `Helper/ProjectSerializer.cs`：
     - `Save(Project)` → **JSON**（框架内置 `DataContractJsonSerializer`，仅需在 csproj 增加 `System.Runtime.Serialization` 引用，**无 NuGet**）。
     - `Load(path)` → 先按 JSON 解析；失败则按**受限 `BinaryFormatter`** 读取旧文件（自定义 `SerializationBinder` 白名单，**仅允许 `ProjectEnc.Bean.Project`**，阻断 gadget 链），成功后**自动重写为 JSON**。
  2. `FileHelper.Serialize/Deserialize` 标记 `[Obsolete]`，仅供 legacy 读取内部使用；`MainForm.cs:98,168`、`Bean/Project.cs:36` 改用 `ProjectSerializer`。
  3. 工程文件扩展名保持 `.project`（内容改为 JSON），确保用户无感。
- **预期效果**：新工程文件为可读 JSON；旧二进制文件首次打开自动迁移，用户无感；消除反序列化 RCE。
- **备选**：若你希望**完全手写 JSON 读写**（不引入 `System.Runtime.Serialization` 引用），我可改为手写实现——请在确认时告知。
- **验证方式**：静态 + 功能（你）：用旧 `.project` 文件打开 → 成功且文件变为 JSON；新工程文件用文本编辑器可读。

### 步骤 2.4 — 输入校验与路径穿越防护
- **对应问题**：C5（路径穿越）、D7（缺输入校验）
- **修改内容**：
  1. `Control/PBaseControl.cs`：工程名校验——非空、不含 `Path.GetInvalidFileNameChars()`、不含 `..` 与路径分隔符。
  2. `Bean/Project.cs`：`ProjectPath` 计算后用 `Path.GetFullPath` 校验仍位于 `Projects` 基目录内，否则拒绝。
- **预期效果**：非法工程名无法创建/保存；无法逃逸出工程根目录。
- **验证方式**：静态 + 功能（你）：输入 `..\..\Windows`、`a/b`、`a:b` 等应被拒绝。

### 步骤 2.5 — 文件操作保护与资源释放
- **对应问题**：C9、C10、C12、C13
- **修改内容**：
  1. `Helper/FileHelper.cs`：`Serialize/Deserialize` 改 `using`。
  2. `Bean/Project.cs`：`File.Delete` 前 `File.Exists` 判断；`File.Copy` 明确覆盖策略；`Init` 处理目录/文件已存在。
  3. `Form/DialogEditFileForm.cs`：读写模板前校验存在 + `try/catch`。
  4. `MainForm.cs:103-117`：删除前判断是否存在选中项，无则提示；包 `try/catch`。
  5. `MainForm.cs` 的加载遮罩改为 `try/finally`（C14）。
- **预期效果**：缺失文件/无选中/损坏数据等场景不再崩溃或误报成功。
- **验证方式**：静态 + 功能（你）：删除时未勾选任何项 → 有提示；模板文件缺失 → 友好报错。

### 步骤 2.6 — 编码一致性加固
- **对应问题**：C6（**已核实：当前 `.iss` 为 GB2312，代码按 GB2312 读取是正确的；风险在于硬编码散落、编辑器误存即损坏**）
- **修改内容**：
  1. 抽取编码常量（如 `TemplateEncoding`），替换散落的 `Encoding.GetEncoding("GB2312")`。
  2. 明确并注释模板编码约定；保证 VS/编辑器保存后仍按约定读写。
- **预期效果**：编码处理集中、可维护；避免编辑器改存导致中文乱码。
- **验证方式**：静态；功能（你）：编辑并保存模板后，生成的安装包文件名中文正常。

### 阶段 2 收尾
- 提交为一个 commit；输出《阶段 2 总结》与测试清单，**待你确认**。

---

## 后续阶段（本次不执行，仅登记）

- **阶段 3（P2 可维护性）**：拆分 `Project`（DTO/服务/模板渲染分离）、引入服务层解耦 UI、消除重复代码、常量提取、死代码清理、长任务异步化、目录树性能（HashSet + 懒加载）、勾选防重入。
- **阶段 4（P3 长期）**：目标框架升级评估、单元测试、CI、操作审计与一键还原。

> 如后续需要，可在 P0+P1 稳定后另行启动。

---

## 交付物与节奏

- 每阶段：修改代码 → 静态验证 → 输出阶段总结 + 测试清单 → **等你确认** → 下一阶段。
- 全程**不改动** `*.Designer.cs`（步骤 2.5 涉及的 `MainForm.cs` 逻辑除外）、`*.resx`、`Tools/`。
- 现有《代码审查报告-ProjectEnc.md》作为问题索引保留。

## 待你确认

1. 以上**阶段 1 的 5 个步骤**是否照此执行？
2. 步骤 1.4 采用"**Id 定位目录 + 保存时重命名**"方案（保留改名能力），是否同意？若你更希望"已保存工程禁止改名"，请告知。
3. 步骤 2.3 的 JSON 采用**框架内置 `DataContractJsonSerializer`**（新增一个 `System.Runtime.Serialization` 引用，仍无 NuGet）是否可接受？或改为完全手写 JSON？
