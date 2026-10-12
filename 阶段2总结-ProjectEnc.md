# 阶段 2 总结 ｜ P1：健壮性与安全加固

> 执行日期：2026-10-12 ｜ 提交：`9901bf5`
> 状态：**已完成编码 + 编译验证 + 运行验证通过，等待你的功能测试确认**
> 改动规模：13 个文件，+528 / −51 行（新增 4 个源文件）

---

## 一、完成情况一览

| 步骤 | 对应问题 | 状态 | 涉及文件 |
|---|---|---|---|
| 2.1 全局异常兜底 + 日志体系 | C7、C8 | ✅ | `Program.cs`、`Helper/MsgHelper.cs`、**新增** `Helper/LogHelper.cs` |
| 2.2 DPAPI 密码加密 | C4 | ✅ | **新增** `Helper/CryptoHelper.cs`、`Bean/ProjectData.cs` |
| 2.3 序列化迁移 JSON（兼容旧文件） | B4 | ✅ | **新增** `Bean/ProjectData.cs`、`Helper/ProjectSerializer.cs`；`Bean/Project.cs`、`MainForm.cs`、`Helper/FileHelper.cs`、`ProjectEnc.csproj` |
| 2.4 输入校验与路径穿越防护 | C5、D7 | ✅ | `Bean/Project.cs`、`Control/PBaseControl.cs` |
| 2.5 文件操作保护与资源释放 | C9/C10/C12/C13/C14 | ✅ | `Helper/FileHelper.cs`、`Bean/Project.cs`、`Form/DialogEditFileForm.cs`、`Form/DialogNewForm.cs`、`MainForm.cs` |
| 2.6 编码统一 | C6 | ✅ | `Helper/FileHelper.cs`、`Bean/Project.cs`、`Form/DialogEditFileForm.cs` |

**编译验证**：`MSBuild -t:Rebuild` → **0 错误、0 警告**。
**运行验证**：临时控制台测试程序 **22 项断言全部通过**（详见第三节）。

---

## 二、各步骤改了什么

### 2.1 全局异常兜底 + 日志体系
- **新增 `Helper/LogHelper.cs`**：按天写入程序目录 `Logs/yyyy-MM-dd.log`，`lock` 保证线程安全，写入失败静默（不影响主流程），零第三方依赖。
- `Program.cs` 在原有 `Application.ThreadException` 基础上，**增补** `AppDomain.CurrentDomain.UnhandledException`（非 UI 线程崩溃）与 `TaskScheduler.UnobservedTaskException`（未观察的 Task 异常，并 `SetObserved()`）。
- `MsgHelper.Error(Exception)` 改为：**先写日志**，弹窗只显示「操作失败：{Message}，详情请查看 Logs 日志」，**不再向用户暴露堆栈**。

### 2.2 DPAPI 密码加密
- **新增 `Helper/CryptoHelper.cs`**：基于 `ProtectedData`（`DataProtectionScope.CurrentUser`）加/解密；解密失败时回退原值（兼容历史明文）。
- 密码在**持久化边界**加密：`ProjectData.FromProject` 加密、`ToProject` 解密；业务代码（UI、`GetPackageConfig` 注入 `.iss`）仍使用明文，**无侵入**。

### 2.3 序列化迁移 JSON（并兼容旧文件）
- **新增 `Bean/ProjectData.cs`**：`[DataContract]` 持久化 DTO，与领域模型 `Project` 分离，密码字段存密文。
- **新增 `Helper/ProjectSerializer.cs`**：
  - `Save` → `DataContractJsonSerializer` 写 JSON（框架内置，无需 NuGet）。
  - `Load` → 先按 JSON 解析；失败则用 **`BinaryFormatter` + 白名单 `SerializationBinder`**（仅允许 `Project` / `List<string>` / `string` / `string[]`）读取旧文件，成功后**自动重写为 JSON**。
  - 白名单 Binder 用于**阻断 BinaryFormatter 的 gadget 链攻击（RCE）**。
- `Bean/Project.cs`（`Init`/`Save`）与 `MainForm.TreeList_DoubleClick` 调用点切换到 `ProjectSerializer`；`FileHelper.Serialize/Deserialize` 标记 `[Obsolete]`。
- `ProjectEnc.csproj` 增加 `System.Runtime.Serialization`、`System.Security` 引用及 4 个新源文件。

### 2.4 输入校验与路径穿越防护
- `Project.IsValidName`：非空、无首尾空格、不含 `Path.GetInvalidFileNameChars()`、不含 `..`、长度 ≤ 100。
- `Project.EnsureInsideProjectsRoot`：用 `Path.GetFullPath` 校验工程路径必须落在 `Projects` 根目录内，**防止 `..` 逃逸**。
- `Init()`/`Save()` 均先校验；`PBaseControl.Save()` 在创建前校验并给出友好提示。

### 2.5 文件操作保护与资源释放
- `FileHelper.Serialize/Deserialize` 改用 `using`（修掉句柄泄漏）。
- `Project.Init()`：目标目录已存在且非空 → 抛「已存在同名工程」，避免覆盖。
- `DialogEditFileForm`：读写模板前判断文件存在 + `try/catch`。
- `DialogNewForm`：`Init()` 包 `try/catch`（捕获重名/校验失败）。
- `MainForm`：删除工程前**先判断是否勾选**（无勾选给出提示，不再误报成功）+ `try/catch`；合并/加密/打包及双击打开的加载遮罩全部改为 **`try/finally`**。

### 2.6 编码统一
- `FileHelper` 新增 `ConfuserTemplateEncoding`(UTF-8) 与 `InnoTemplateEncoding`(GB2312) 常量，替换 `Project` 与 `DialogEditFileForm` 中散落的 `Encoding.GetEncoding("GB2312")`。
- **说明**：经核实 `工程打包.iss` 本就是 GB2312 编码、代码按 GB2312 读取是**正确的**；本步仅做集中化，未改动模板文件内容。

---

## 三、运行验证结果（22 项全部通过）

用临时控制台程序直接引用改动后的真实源文件（`Project`/`ProjectData`/`FileHelper`/`CryptoHelper`/`LogHelper`/`ProjectSerializer`）运行：

| 组 | 断言 | 结果 |
|---|---|---|
| JSON 往返 | 文件为 JSON；密码**未以明文出现**；Name/FolderName/RootPath/ExeFile/PackagePath/**Password 解密还原**/MergeDlls/PackageIgnores 全部往返一致（含中文、空格、反斜杠路径） | ✅ 11/11 |
| 旧文件迁移 | 旧二进制文件可读取、字段正确、**已被重写为 JSON** | ✅ 3/3 |
| 安全 | 非法类型（`List<int>`）被 Binder **拦截返回 null**；不存在文件返回 null | ✅ 2/2 |
| 名称校验 | 合法名通过；拒绝空白、`..\..\Windows`、`a/b`、`a:b`、尾随空格 | ✅ 6/6 |

实际产出的工程 JSON 形如（密码为 DPAPI 密文）：
```json
{"Id":"…","Name":"演示工程-Demo","FolderName":"演示工程-Demo",
 "RootPath":"D:\\My App\\测试 目录","ExeFile":"…\\Main.exe","PackagePath":"D:\\out 输出",
 "Password":"AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA…","MergeDlls":["…a.dll","…b.dll"],
 "PackageIgnores":["…x.txt"]}
```

> 该验证程序建在系统临时目录、未纳入版本库，验证后已清理。

---

## 四、需要你执行的功能测试清单

| # | 场景 | 预期结果 |
|---|---|---|
| P1 | 新建工程 → 查看 `Projects\<名>\工程.project` | 内容为**可读 JSON**，其中 `Password` 为密文（乱码/Base64 串） |
| P2 | 用**旧版本**保存的工程文件打开 | 正常加载，且该文件**被自动改写为 JSON** |
| P3 | 打开含密码的工程 → 生成安装包 | 安装包仍要求输入原密码（加解密链路正确） |
| P4 | 新建工程时输入非法名称（`a:b`、`..\x`、含首尾空格） | 提示名称非法，不创建 |
| P5 | 新建与已有工程**同名**的工程 | 提示「已存在同名工程…」，不覆盖 |
| P6 | 人为触发一次异常（如把工具目录改名后执行合并） | 弹窗**友好提示**（无堆栈）；程序目录 `Logs\<日期>.log` 有记录 |
| P7 | 删除工程时不勾选任何项 | 提示「请先勾选要删除的工程」，不误报成功 |
| P8 | 编辑模板文件（"编辑模板文件"按钮） | 正常读写；中文不乱码 |
| P9 | 执行合并/加密/打包过程中出错 | 加载遮罩**能正常关闭**，不残留 |

---

## 五、说明与遗留

- **未改动**：`*.Designer.cs`、`*.resx`、`Tools/`、`Template/*` 内容。
- **兼容性**：旧二进制工程文件可正常打开并自动迁移；`FolderName` 缺省回退 `Name`；密码解密失败回退明文。
- **新增文件行尾**为 LF（仓库原为 CRLF），Git 会在下次触碰时自动规范化，**不影响编译**。
- **未提交的文档**：`代码审查报告`、`重构方案`、`阶段1总结`、本总结 —— 保持未跟踪。
- **本阶段未做**（属阶段 3/P2，本次范围外）：拆分 `Project` 服务化、UI 解耦、去重、异步化、目录树性能、勾选防重入等。

---

## 六、下一步

P0 + P1 已全部完成。后续可选（本次范围外）：
- **阶段 3（P2 可维护性）**：拆分 `Project`、引入服务层、消除重复代码、常量提取、死代码清理、长任务异步化、目录树性能优化、勾选防重入。
- **阶段 4（P3 长期）**：框架升级评估、单元测试、CI、操作审计与一键还原。

需要的话，可在你确认本阶段后继续启动阶段 3。
