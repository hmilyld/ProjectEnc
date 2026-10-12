using ProjectEnc.Bean;
using ProjectEnc.Helper;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectEnc.Service
{
    /// <summary>
    /// 工程操作服务：合并、加密、打包。承载业务编排，与 UI 解耦，可独立测试。
    /// </summary>
    public class ProjectService
    {
        public Task MergeAsync(Project project, IProgress<string> progress)
        {
            return Task.Run(() => Merge(project, progress));
        }

        public Task ProtectAsync(Project project, IProgress<string> progress)
        {
            return Task.Run(() => Protect(project, progress));
        }

        public Task PackageAsync(Project project, IProgress<string> progress)
        {
            return Task.Run(() => Package(project, progress));
        }

        private static void Merge(Project project, IProgress<string> progress)
        {
            if (!project.MergeDlls.Any())
            {
                Report(progress, "没有需要合并的文件，无需合并");
                return;
            }

            BackUpExeFile(project);

            string exePath = Path.Combine(project.ToolPath, AppConstants.ILMergeRelativePath);
            string arguments =
                $"/out:{ProcessRunner.Quote(project.ExeFile)} {ProcessRunner.Quote(project.ExeFile)} {string.Join(" ", project.MergeDlls.Select(ProcessRunner.Quote))}";

            string output = ProcessRunner.Run(exePath, arguments);
            Report(progress, output);
            Report(progress, "程序合并成功");

            string pdbPath = Path.Combine(project.RootPath, project.ExeFileName() + AppConstants.PdbExtension);
            if (File.Exists(pdbPath))
            {
                File.Delete(pdbPath);
            }
        }

        private static void Protect(Project project, IProgress<string> progress)
        {
            BackUpExeFile(project);

            string exePath = Path.Combine(project.ToolPath, AppConstants.ConfuserExeRelativePath);
            string configPath = GetProtectConfig(project);

            string output = ProcessRunner.Run(exePath, ProcessRunner.Quote(configPath));
            Report(progress, output);
            Report(progress, "程序加密成功");

            File.Delete(configPath);
        }

        private static void Package(Project project, IProgress<string> progress)
        {
            string exePath = Path.Combine(project.ToolPath, AppConstants.InnoRelativePath);
            string configPath = GetPackageConfig(project);

            string output = ProcessRunner.Run(exePath, ProcessRunner.Quote(configPath));
            Report(progress, output);
            Report(progress, "安装包打包成功");

            File.Delete(configPath);
        }

        private static string GetProtectConfig(Project project)
        {
            string templatePath = Path.Combine(project.ProjectPath, AppConstants.ConfuserTemplateFile);
            string tempPath = BuildTempPath(project, AppConstants.ConfuserTemplateFile);

            return TemplateRenderer.Render(templatePath, tempPath, FileHelper.ConfuserTemplateEncoding, content =>
            {
                content = content.Replace("{path}", Safe(project.RootPath));
                content = content.Replace("{filename}", Safe(project.ExeFileName(true)));
                return content;
            });
        }

        private static string GetPackageConfig(Project project)
        {
            string templatePath = Path.Combine(project.ProjectPath, AppConstants.InnoTemplateFile);
            string tempPath = BuildTempPath(project, AppConstants.InnoTemplateFile);

            var ignoreFiles = project.MergeDlls.Concat(project.PackageIgnores).ToList();
            ignoreFiles.Add(Path.Combine(project.RootPath, project.ExeFileName() + AppConstants.BackupExtension));

            var versionInfo = FileVersionInfo.GetVersionInfo(project.ExeFile);

            return TemplateRenderer.Render(templatePath, tempPath, FileHelper.InnoTemplateEncoding, content =>
            {
                content = content.Replace("{filename}", Safe(project.ExeFileName()));
                content = content.Replace("{id}", Safe(project.Id));
                content = content.Replace("{path}", Safe(project.RootPath));
                content = content.Replace("{output}", Safe(project.PackagePath));
                content = content.Replace("{ignore}", string.Join(",", ignoreFiles.Select(f => f.Replace($"{project.RootPath}\\", ""))));
                content = content.Replace("{version}", Safe(versionInfo.ProductVersion));
                content = content.Replace("{password}", Safe(project.Password));
                return content;
            });
        }

        private static string BuildTempPath(Project project, string fileName)
        {
            return Path.Combine(
                project.ProjectPath,
                $"{Path.GetFileNameWithoutExtension(fileName)}-临时{Path.GetExtension(fileName)}");
        }

        private static void BackUpExeFile(Project project)
        {
            if (!File.Exists(project.ExeFile))
            {
                throw new FileNotFoundException($"主程序文件不存在，无法备份：{project.ExeFile}", project.ExeFile);
            }

            string backup = Path.Combine(project.RootPath, project.ExeFileName() + AppConstants.BackupExtension);
            File.Copy(project.ExeFile, backup, true);
        }

        private static void Report(IProgress<string> progress, string message)
        {
            if (progress != null && !string.IsNullOrEmpty(message))
            {
                progress.Report(message);
            }
        }

        /// <summary>占位符替换值为 null 时回退为空字符串，避免 Replace 抛 ArgumentNullException。</summary>
        private static string Safe(string value)
        {
            return value ?? string.Empty;
        }
    }
}
