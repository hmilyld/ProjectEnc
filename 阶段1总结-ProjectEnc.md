# 阶段 1 总结 ｜ P0：正确性与安全

> 执行日期：2026-10-12 ｜ 提交：`b69f921`
> 状态：**已完成编码 + 编译验证通过，等待你的功能测试确认**

---

## 一、完成情况一览

| 步骤 | 对应问题 | 状态 | 改动文件 |
|---|---|---|---|
| 1.1 重写 `Project.Exec` | C1 失败报成功 / C2 死锁挂起 | ✅ | `Bean/Project.cs` |
| 1.2 命令行参数转义 | C3 含空格路径失败 / 参数注入 | ✅ | `Bean/Project.cs` |
| 1.3 备份机制修正 | C11 备份不刷新 / 拼写笔误 | ✅ | `Bean/Project.cs` |
| 1.4 重命名路径一致性 | D3 改名后保存错位 | ✅ | `Bean/Project.cs`、`MainForm.cs` |
| 1.5 目录不进入文件列表 | D4 目录混入合并列表 | ✅ | `Control/FileControl.cs` |

**编译验证**：`MSBuild -t:Rebuild` → **0 错误、0 警告**；三处改动文件 BOM 完整（中文字面量无乱码）。
**改动规模**：3 个文件，+160 / −29 行。

---

## 二、各步骤改了什么

### 1.1 `Project.Exec` 重写（`Bean/Project.cs`）
- `using (var process = ...)` 包裹，确保进程句柄释放。
- 改用 `OutputDataReceived` / `ErrorDataReceived` 异步事件收集 stdout/stderr —— 消除「只读 stdout 导致 stderr 缓冲写满而死锁」。
- `WaitForExit(ProcessTimeoutMs)`（**10 分钟**）超时后 `Kill()` 并抛 `TimeoutException`；随后无参 `WaitForExit()` 确保异步输出刷新。
- **校验 `ExitCode`**：非 0 抛 `InvalidOperationException`（含退出码 + stderr 摘要）；**成功消息仅在退出码为 0 时输出**。
- 执行前校验工具文件存在，缺失抛 `FileNotFoundException`。
- 移除原 `StandardInput.WriteLine()` hack。
- 新增 `Report()` 空引用保护（`progress` 未注入时不再 NRE）。

### 1.2 命令行参数转义（`Bean/Project.cs`）
- 新增 `Quote(string)`：双引号包裹 + 转义内部引号。
- `Merge`：`/out:` 目标、主 exe、每个待合并 DLL 全部经 `Quote`。
- **额外修复**：`Protect` / `Package` 原本把配置文件路径直接当参数传入（未加引号），现同样 `Quote` —— 安装路径含空格时不再失败。

### 1.3 备份机制修正（`Bean/Project.cs`）
- `BackUpExeFile` 改为**每次操作前覆盖式备份**（`File.Copy(..., true)`），备份即「本次操作前的状态」。
- 备份前校验主程序存在，否则抛明确异常。
- 扩展名 `.bakup` → `.backup`，并同步 `GetPackageConfig` 中排除列表的引用（两处一致）。

### 1.4 重命名路径一致性（`Bean/Project.cs`、`MainForm.cs`）
- `Project` 新增可序列化字段 `FolderName`（工程在磁盘上的目录名）；`ProjectPath` 改为基于 `FolderName`（历史文件无该字段时回退 `Name`）。
- 新增 `Project.Save()`：若 `Name` 与当前目录不一致 → 目标同名目录存在则**报错中止**，否则 `Directory.Move` 并更新 `FolderName`，最后写入工程文件。
- `MainForm.SaveBtnItem` 改用 `project.Save()` 并包 `try/catch`，改名冲突时给出友好提示而非崩溃。
- 附带效果：改名后即使**未保存**就点合并/加密，`ProjectPath` 仍指向真实目录，不会读不到模板。

### 1.5 目录不进入文件列表（`Control/FileControl.cs`）
- `UpdateSelectFiles` 仅收集「真实文件」节点（`File.Exists` 判定），目录路径不再混入 `MergeDlls` / `PackageIgnores`。

---

## 三、需要你执行的功能测试清单

> 我已完成编译验证；以下为运行时行为验证，请在 VS 中编译后按项确认。

| # | 场景 | 预期结果 |
|---|---|---|
| T1 | 正常执行「合并文件」 | 提示「程序合并成功」，日志显示 ILMerge 输出 |
| T2 | 把 `Tools/ILMerge/ILMerge.exe` 临时改名后再合并 | 提示「未找到执行工具：…」，**不再误报成功** |
| T3 | 用错误参数触发工具非 0 退出 | 提示「…执行失败（退出码 X）」并显示原因 |
| T4 | 把待处理工程放在**含空格**的目录（如 `D:\My App\`）下执行合并/加密/打包 | 三步均正常执行，不再因空格失败 |
| T5 | 连续执行两次合并，查看主程序目录下的 `*.backup` | 内容为**最近一次操作前**的 exe（时间戳更新） |
| T6 | 打开已有工程 → 改工程名 → 保存 → 重新加载 | 目录已重命名、内容正确、无异常 |
| T7 | 改名时输入一个**已存在**的工程名 → 保存 | 提示「已存在同名工程…」，不破坏任何目录 |
| T8 | 在「待合并类库列表」勾选**整个文件夹** → 保存工程 | 保存后的列表只含文件、不含目录 |
| T9 | 用**旧的**工程文件（此前版本保存的）打开 | 正常加载（`FolderName` 缺省回退为 `Name`），无异常 |

---

## 四、说明与遗留

- **未改动**：`*.Designer.cs`、`*.resx`、`Tools/`。
- **兼容性**：`FolderName` 为新增可序列化字段，旧工程文件反序列化时为 `null`，已通过 `FolderNameOrName` 回退为 `Name`，**旧文件可正常打开**。
- **未提交的文件**：`代码审查报告-ProjectEnc.md`、`重构方案-ProjectEnc.md`、本总结文档 —— 均保持未跟踪状态，是否纳入版本库由你决定。
- **本阶段未做**（按计划留待阶段 2）：日志体系、密码加密、序列化迁移 JSON、输入校验、文件操作保护、编码统一。

---

## 五、下一步

确认本阶段测试通过后，进入 **阶段 2（P1：健壮性与安全）**：
2.1 全局异常兜底 + 日志体系 → 2.2 DPAPI 密码加密 → 2.3 序列化迁移 JSON（兼容旧文件）→ 2.4 输入校验与路径穿越防护 → 2.5 文件操作保护与资源释放 → 2.6 编码统一。
