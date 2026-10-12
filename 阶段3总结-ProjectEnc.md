# 阶段 3 总结 ｜ P2：可维护性重构

> 执行日期：2026-10-12 ｜ 提交：`6ae3c5d`
> 状态：**已完成编码 + 编译验证 + 运行验证通过，等待你的功能测试确认**
> 改动规模：13 个文件，+537 / −435 行（新增 4 个源文件）

---

## 一、完成情况一览

| 步骤 | 内容 | 状态 | 涉及文件 |
|---|---|---|---|
| 3.1 常量提取 | 集中目录名/文件名/工具路径/扩展名 | ✅ | **新增** `Helper/AppConstants.cs`；`Project.cs`、`MainForm.cs`、`DialogEditFileForm.cs` |
| 3.2 死代码清理 | 删除未使用成员 | ✅ | `Helper/FileHelper.cs`、`Helper/LoadingHelper.cs`、`Helper/MsgHelper.cs` |
| 3.3 MainForm 去重 | 抽取两个通用方法 | ✅ | `MainForm.cs` |
| 3.4 目录树性能 + 防重入 | HashSet + 递归保护 + 批量优化 | ✅ | `Control/FileControl.cs`、`Control/ProjectControl.cs` |
| 3.5 拆分 `Project` 服务化 | ProcessRunner / TemplateRenderer / ProjectService | ✅ | **新增** `Helper/ProcessRunner.cs`、`Helper/TemplateRenderer.cs`、`Service/ProjectService.cs`；`Bean/Project.cs` |
| 3.6 长任务异步化 | `async/await` + `Task.Run` | ✅ | `Service/ProjectService.cs`、`MainForm.cs` |

**编译验证**：`MSBuild -t:Rebuild` → **0 错误、0 警告**。
**运行验证**：临时测试程序 **26 项断言全部通过**（含阶段 2 回归 + 新增 `ProcessRunner.Quote`）。

---

## 二、各步骤改了什么

### 3.1 常量提取
- **新增 `Helper/AppConstants.cs`**：`Projects`/`Tools`/`Template` 目录名、`工程.project`、两个模板文件名、`.backup`/`.pdb` 扩展名、三个工具的相对路径。
- 替换了 `Project`、`MainForm`、`DialogEditFileForm` 中散落的字符串字面量。

### 3.2 死代码清理
- 删除 `FileHelper.CopyDirectory`（仅自递归、无外部调用）。
- 删除 `FileHelper.Serialize/Deserialize` —— 已被 `ProjectSerializer` 完全取代，且是**最后一处未被限制的 BinaryFormatter 调用**，删除后二进制反序列化只存在于 `ProjectSerializer` 的白名单路径中。
- 删除 `LoadingHelper.Show(Type)`/`Show(Form)`/`Set`、`MsgHelper.Confirm`（均无调用）。
- `FileHelper`、`LoadingHelper` 改为 `static class`（无实例语义）。

### 3.3 MainForm 去重
- 抽取 `OpenDirectory(Func<string>)`：3 个「打开目录」处理器各缩为一行；顺带增加「目录不存在」提示，并对含空格路径用 `Quote` 包裹。
- 抽取 `RunWithLoadingAsync(Func<Task>)`：3 个「操作」处理器统一为「判空 → await」，异常处理与遮罩关闭集中在同一处。

### 3.4 目录树性能 + 勾选防重入
- `List.Contains`（O(n)）改为 `HashSet<string>`（O(1)）—— 消除加载大目录时的 **O(n²)**。
- 新增 `propagating` 标志：阻止 `AfterCheckNode` 中通过代码修改 `CheckState` 引发的**递归触发**（原注释误以为 `BeginUpdate` 能阻止递归）。
- 新增 `batchUpdating` 标志：批量勾选（全选/取消）期间**只在结束时重建一次列表**，替代原先每勾一个节点就全量重建。
- `type` 字符串（`"merge"`/`"ignore"`）改为 **`FileListKind` 枚举**，编译期类型安全。
- `SelectFiles` 现在兼容 `dll` 与 `.dll` 两种输入。

### 3.5 拆分 `Project`，引入服务层
重构后的职责划分：

| 类型 | 职责 |
|---|---|
| `Bean/Project` | 纯数据 + 路径推导 + `Init`/`Save` 持久化 + 名称/路径校验 |
| `Helper/ProcessRunner` | 外部进程执行（超时、退出码校验、`Quote` 转义） |
| `Helper/TemplateRenderer` | 模板读取 → 占位符替换 → 写临时文件 |
| `Service/ProjectService` | 合并/加密/打包的业务编排 |
| `Helper/ProjectSerializer` | JSON 持久化 + 旧二进制迁移 |

- `Project` 从 300+ 行精简为「数据 + 持久化 + 校验」，不再持有 `IProgress`、不再直接调进程。
- 附带修复：模板占位符替换值为 `null` 时回退空串，避免 `Replace` 抛 `ArgumentNullException`（旧工程 `Id` 为空时原本会崩）。

### 3.6 长任务异步化
- `ProjectService.MergeAsync/ProtectAsync/PackageAsync` 返回 `Task`，内部 `Task.Run` 执行进程调用。
- `MainForm` 三个处理器改为 `async void` + `await this.RunWithLoadingAsync(...)`。
- **界面在合并/加密/打包期间不再冻结**；`Progress<T>` 仍在 UI 线程构造，进度回报依旧安全投递到 `LogEdit`。

### 附带修复
- `LoadProjects` 改用 `Path.GetFileName`，避免工程名含 `.` 时被 `GetFileNameWithoutExtension` **截断**（原 D-级隐患）。

---

## 三、运行验证结果（26 项全部通过）

临时控制台程序直接引用改动后的真实源文件运行：

| 组 | 结果 |
|---|---|
| JSON 往返（中文/空格路径、密码解密还原、集合） | ✅ 11/11 |
| 旧二进制文件读取并迁移为 JSON | ✅ 3/3 |
| 白名单 Binder 拦截非法类型、缺失文件返回 null | ✅ 2/2 |
| 名称校验（合法/空白/`..`/斜杠/冒号/首尾空格） | ✅ 6/6 |
| **新增** `ProcessRunner.Quote`（含空格、空值、内部引号转义）+ 工具不存在抛异常 | ✅ 4/4 |

> 验证程序建在系统临时目录、未入库，跑完已清理。

---

## 四、需要你执行的功能测试清单

| # | 场景 | 预期结果 |
|---|---|---|
| R1 | 打开工程，观察「待合并类库列表」「打包排除列表」 | 勾选状态与保存前一致（HashSet 改造回归） |
| R2 | 勾选/取消某个文件夹 | 子项联动、父项显示半选（Indeterminate）正常，无卡顿 |
| R3 | 在「批量过滤后缀名」输入 `dll`（或 `.dll`）点全选/取消 | 对应文件被切换，列表内容正确、只含文件 |
| R4 | 执行「合并文件」 | 界面**不冻结**、可拖动窗口，完成后提示成功 |
| R5 | 执行「加密主程序」「生成安装包」 | 同上，遮罩正常显示与关闭 |
| R6 | 三个「打开目录」按钮 | 正确打开工程目录/项目目录/输出目录；目录不存在时提示 |
| R7 | 工程名含点（如 `My.App`） | 工程列表显示完整名称，双击可正常打开 |
| R8 | 旧版本工程文件打开 | 正常加载并转 JSON（阶段 2 回归） |
| R9 | 编辑模板文件 | 中文不乱码（编码常量回归） |

---

## 五、说明与遗留

- **未改动**：`*.Designer.cs`、`*.resx`、`Tools/`、`Template/*`。
- **`Serialize/Deserialize` 的移除**：阶段 2 曾按计划标记 `[Obsolete]` 保留；本阶段在确认无任何调用后删除，以清除最后一处不受限的 `BinaryFormatter` 用法。如你希望保留该 API，我可以恢复。
- **未做**（原计划中已说明本次不含）：目录树**懒加载**（涉及 UI 交互变更、回归风险较高，未纳入）。
- **新增文件行尾**为 LF（仓库原为 CRLF），Git 会自动规范化，不影响编译。
- **未提交的文档**：`代码审查报告`、`重构方案`、`阶段1/2/3总结` —— 保持未跟踪。

---

## 六、下一步

P0 + P1 + P2 均已完成。剩余可选（阶段 4 / P3 长期）：
- 目标框架升级评估（4.5.2 → 4.8 或 .NET 8）
- 引入单元测试工程（把 `ProcessRunner`/`TemplateRenderer`/`ProjectSerializer`/`Project` 的验证固化为测试）
- CI 构建
- 操作审计与「一键还原」入口
- 目录树懒加载、`HelpBtnItem` 占位文案补全等收尾项
