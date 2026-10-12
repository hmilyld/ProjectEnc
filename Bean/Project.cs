using ProjectEnc.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEnc.Bean
{
    [Serializable]
    public class Project
    {
        [NonSerialized]
        private IProgress<string> progress;

        public string Id { get; set; }
        public string Name { get; set; }
        public string RootPath { get; set; }
        public string ExeFile { get; set; }
        public string PackagePath { get; set; }
        public string Password { get; set; }

        public List<string> MergeDlls { get; set; } = new List<string>();
        public List<string> PackageIgnores { get; set; } = new List<string>();

        /// <summary>
        /// 工程在磁盘上的文件夹名称。历史工程文件可能没有该字段，此时回退为 <see cref="Name"/>。
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// 外部工具（ILMerge/ConfuserEx/Inno）执行的超时时间，超时后强制结束进程。
        /// </summary>
        private const int ProcessTimeoutMs = 10 * 60 * 1000;

        private string AppPath => AppDomain.CurrentDomain.BaseDirectory;
        private string ToolPath => Path.Combine(AppPath, "Tools");
        private string FolderNameOrName => string.IsNullOrEmpty(FolderName) ? Name : FolderName;
        private string ProjectPath => Path.Combine(AppPath, "Projects", FolderNameOrName);

        private string ExeFileName(bool ext = false) => ext ? Path.GetFileName(ExeFile) : Path.GetFileNameWithoutExtension(ExeFile);

        public void Init()
        {
            this.FolderName = this.Name;

            Directory.CreateDirectory(ProjectPath);
            FileHelper.Serialize(Path.Combine(ProjectPath, "工程.project"), this);
            CopyTemplateFiles();
        }

        /// <summary>
        /// 保存工程到磁盘。若工程名称发生变化，会同步重命名工程目录，保证名称与目录一致。
        /// </summary>
        public void Save()
        {
            string targetDir = Path.Combine(AppPath, "Projects", Name);
            string currentDir = ProjectPath;

            if (!string.Equals(Path.GetFullPath(currentDir), Path.GetFullPath(targetDir), StringComparison.OrdinalIgnoreCase))
            {
                if (Directory.Exists(targetDir))
                {
                    throw new InvalidOperationException($"已存在同名工程“{Name}”，请更换工程名称。");
                }

                if (Directory.Exists(currentDir))
                {
                    Directory.Move(currentDir, targetDir);
                }

                this.FolderName = Name;
            }

            if (!Directory.Exists(ProjectPath))
            {
                Directory.CreateDirectory(ProjectPath);
            }

            FileHelper.Serialize(Path.Combine(ProjectPath, "工程.project"), this);
        }

        private void CopyTemplateFiles()
        {
            string[] templateFiles = { "工程加密.crproj", "工程打包.iss" };
            foreach (var file in templateFiles)
            {
                File.Copy(Path.Combine(AppPath, "Template", file), Path.Combine(ProjectPath, file));
            }
        }

        public void SetProgress(IProgress<string> progress) => this.progress = progress;

        public void Merge()
        {
            if (!MergeDlls.Any())
            {
                Report("没有需要合并的文件，无需合并");
                return;
            }

            BackUpExeFile();

            string exePath = Path.Combine(ToolPath, "ILMerge", "ILMerge.exe");
            string arguments = $"/out:{Quote(ExeFile)} {Quote(ExeFile)} {string.Join(" ", MergeDlls.Select(Quote))}";

            Exec(exePath, arguments, "程序合并成功");

            string pdbPath = Path.Combine(RootPath, $"{ExeFileName()}.pdb");
            if (File.Exists(pdbPath))
            {
                File.Delete(pdbPath);
            }
        }

        public void Protect()
        {
            BackUpExeFile();

            string exePath = Path.Combine(ToolPath, "ConfuserEx", "Confuser.CLI.exe");
            string configPath = GetProtectConfig();

            Exec(exePath, Quote(configPath), "程序加密成功");
            File.Delete(configPath);
        }

        public void Package()
        {
            string exePath = Path.Combine(ToolPath, "Inno", "iscc.exe");
            string configPath = GetPackageConfig();

            Exec(exePath, Quote(configPath), "安装包打包成功");
            File.Delete(configPath);
        }

        /// <summary>
        /// 执行外部工具。会等待进程结束、校验退出码，失败或超时均抛出异常，避免误报成功。
        /// </summary>
        private void Exec(string exePath, string arguments, string successMessage)
        {
            if (!File.Exists(exePath))
            {
                throw new FileNotFoundException($"未找到执行工具：{exePath}", exePath);
            }

            var output = new StringBuilder();
            var error = new StringBuilder();

            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        output.AppendLine(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        error.AppendLine(e.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                if (!process.WaitForExit(ProcessTimeoutMs))
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                        // 忽略结束失败，下面统一抛出超时异常
                    }

                    throw new TimeoutException($"执行超时（超过 {ProcessTimeoutMs / 1000} 秒）：{Path.GetFileName(exePath)}");
                }

                // 再次调用（无参）以确保异步输出事件已全部刷新
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    string detail = error.Length > 0 ? error.ToString().Trim() : output.ToString().Trim();
                    throw new InvalidOperationException(
                        $"{Path.GetFileName(exePath)} 执行失败（退出码 {process.ExitCode}）。{Environment.NewLine}{detail}");
                }

                Report(output.ToString().TrimEnd());
                Report(successMessage);
            }
        }

        /// <summary>
        /// 将路径包裹为单个命令行参数，避免含空格路径被拆分，并转义内部引号。
        /// </summary>
        private static string Quote(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "\"\"";
            }

            return "\"" + value.Replace("\"", "\\\"") + "\"";
        }

        private void Report(string message)
        {
            if (progress != null && !string.IsNullOrEmpty(message))
            {
                progress.Report(message);
            }
        }

        private string GetProtectConfig()
        {
            return CreateTempConfig("工程加密.crproj", content =>
            {
                content = content.Replace("{path}", RootPath);
                content = content.Replace("{filename}", ExeFileName(true));
                return content;
            });
        }

        private string GetPackageConfig()
        {
            return CreateTempConfig("工程打包.iss", content =>
            {
                var ignoreFiles = MergeDlls.Concat(PackageIgnores).ToList();
                ignoreFiles.Add(Path.Combine(RootPath, $"{ExeFileName()}.backup"));

                var fvi = FileVersionInfo.GetVersionInfo(ExeFile);

                content = content.Replace("{filename}", ExeFileName());
                content = content.Replace("{id}", Id);
                content = content.Replace("{path}", RootPath);
                content = content.Replace("{output}", PackagePath);
                content = content.Replace("{ignore}", string.Join(",", ignoreFiles.Select(f => f.Replace($"{RootPath}\\", ""))));
                content = content.Replace("{version}", fvi.ProductVersion);
                content = content.Replace("{password}", Password);

                return content;
            }, Encoding.GetEncoding("GB2312"));
        }

        private string CreateTempConfig(string filename, Func<string, string> contentModifier, Encoding encoding = null)
        {
            string configPath = Path.Combine(ProjectPath, filename);
            string tempConfigPath = Path.Combine(ProjectPath, $"{Path.GetFileNameWithoutExtension(filename)}-临时{Path.GetExtension(filename)}");

            string content = File.ReadAllText(configPath, encoding ?? Encoding.UTF8);
            content = contentModifier(content);
            File.WriteAllText(tempConfigPath, content, encoding ?? Encoding.UTF8);

            return tempConfigPath;
        }

        private void BackUpExeFile()
        {
            if (!File.Exists(ExeFile))
            {
                throw new FileNotFoundException($"主程序文件不存在，无法备份：{ExeFile}", ExeFile);
            }

            string backup = Path.Combine(RootPath, $"{ExeFileName()}.backup");
            File.Copy(ExeFile, backup, true);
        }
    }
}