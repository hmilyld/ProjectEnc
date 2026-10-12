using ProjectEnc.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectEnc.Bean
{
    /// <summary>
    /// 工程领域模型：仅承载数据、路径推导、持久化与校验。
    /// 合并/加密/打包等业务编排见 <see cref="ProjectEnc.Service.ProjectService"/>。
    /// </summary>
    [Serializable]
    public class Project
    {
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

        internal static string AppPath => AppDomain.CurrentDomain.BaseDirectory;

        internal static string ProjectsRoot => Path.Combine(AppPath, AppConstants.ProjectsFolder);

        internal string ToolPath => Path.Combine(AppPath, AppConstants.ToolsFolder);

        internal string FolderNameOrName => string.IsNullOrEmpty(FolderName) ? Name : FolderName;

        internal string ProjectPath => Path.Combine(ProjectsRoot, FolderNameOrName);

        internal string ExeFileName(bool ext = false) => ext ? Path.GetFileName(ExeFile) : Path.GetFileNameWithoutExtension(ExeFile);

        public void Init()
        {
            EnsureNameValid();

            this.FolderName = this.Name;

            if (Directory.Exists(ProjectPath) && Directory.EnumerateFileSystemEntries(ProjectPath).Any())
            {
                throw new InvalidOperationException($"已存在同名工程“{Name}”，请更换工程名称。");
            }

            EnsureInsideProjectsRoot(ProjectPath);

            Directory.CreateDirectory(ProjectPath);
            ProjectSerializer.Save(this, Path.Combine(ProjectPath, AppConstants.ProjectFileName));
            CopyTemplateFiles();
        }

        /// <summary>
        /// 保存工程到磁盘。若工程名称发生变化，会同步重命名工程目录，保证名称与目录一致。
        /// </summary>
        public void Save()
        {
            EnsureNameValid();

            string targetDir = Path.Combine(ProjectsRoot, Name);
            EnsureInsideProjectsRoot(targetDir);
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

            ProjectSerializer.Save(this, Path.Combine(ProjectPath, AppConstants.ProjectFileName));
        }

        /// <summary>
        /// 校验工程名称是否合法，非法时通过 <paramref name="error"/> 返回原因。
        /// </summary>
        public static bool IsValidName(string name, out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(name))
            {
                error = "工程名称不能为空。";
                return false;
            }

            if (name != name.Trim())
            {
                error = "工程名称不能以空格开头或结尾。";
                return false;
            }

            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                error = "工程名称包含非法字符。";
                return false;
            }

            if (name.Contains(".."))
            {
                error = "工程名称不能包含“..”。";
                return false;
            }

            if (name.Length > 100)
            {
                error = "工程名称过长（最多 100 个字符）。";
                return false;
            }

            return true;
        }

        private void EnsureNameValid()
        {
            string error;
            if (!IsValidName(Name, out error))
            {
                throw new InvalidOperationException(error);
            }
        }

        /// <summary>
        /// 校验路径必须位于程序的 Projects 目录内，防止路径穿越。
        /// </summary>
        private static void EnsureInsideProjectsRoot(string path)
        {
            string root = Path.GetFullPath(ProjectsRoot);
            string full = Path.GetFullPath(path);

            if (!full.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(full, root, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("工程路径超出允许范围。");
            }
        }

        private void CopyTemplateFiles()
        {
            string[] templateFiles = { AppConstants.ConfuserTemplateFile, AppConstants.InnoTemplateFile };
            foreach (var file in templateFiles)
            {
                File.Copy(
                    Path.Combine(AppPath, AppConstants.TemplateFolder, file),
                    Path.Combine(ProjectPath, file));
            }
        }
    }
}
