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

        private string AppPath => AppDomain.CurrentDomain.BaseDirectory;
        private string ToolPath => Path.Combine(AppPath, "Tools");
        private string ProjectPath => Path.Combine(AppPath, "Projects", Name);

        private string ExeFileName(bool ext = false) => ext ? Path.GetFileName(ExeFile) : Path.GetFileNameWithoutExtension(ExeFile);

        public void Init()
        {
            Directory.CreateDirectory(ProjectPath);
            FileHelper.Serialize(Path.Combine(ProjectPath, "工程.project"), this);
            CopyTemplateFiles();
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
                progress.Report("没有需要合并的文件，无需合并");
                return;
            }

            BackUpExeFile();

            string exePath = Path.Combine(ToolPath, "ILMerge", "ILMerge.exe");
            string arguments = $"/out:{ExeFile} {ExeFile} {string.Join(" ", MergeDlls)}";

            Exec(exePath, arguments, "程序合并成功");

            File.Delete(Path.Combine(RootPath, $"{ExeFileName()}.pdb"));
        }

        public void Protect()
        {
            BackUpExeFile();

            string exePath = Path.Combine(ToolPath, "ConfuserEx", "Confuser.CLI.exe");
            string configPath = GetProtectConfig();

            Exec(exePath, configPath, "程序加密成功");
            File.Delete(configPath);
        }

        public void Package()
        {
            string exePath = Path.Combine(ToolPath, "Inno", "iscc.exe");
            string configPath = GetPackageConfig();

            Exec(exePath, configPath, "安装包打包成功");
            File.Delete(configPath);
        }

        private void Exec(string exePath, string arguments, string successMessage)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = arguments,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();

            process.StandardInput.WriteLine();
            process.WaitForExit();

            progress.Report(output);
            progress.Report(successMessage);
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
                ignoreFiles.Add(Path.Combine(RootPath, $"{ExeFileName()}.bakup"));

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
            string backup = Path.Combine(RootPath, $"{ExeFileName()}.bakup");
            if (!File.Exists(backup))
            {
                File.Copy(ExeFile, backup, true);
            }
        }
    }
}