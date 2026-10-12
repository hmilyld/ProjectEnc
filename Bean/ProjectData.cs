using ProjectEnc.Helper;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProjectEnc.Bean
{
    /// <summary>
    /// 工程文件的持久化数据结构（JSON）。与领域模型 <see cref="Project"/> 分离，
    /// 其中密码以密文形式存储（DPAPI 加密）。
    /// </summary>
    [DataContract]
    public class ProjectData
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string FolderName { get; set; }

        [DataMember(Order = 4)]
        public string RootPath { get; set; }

        [DataMember(Order = 5)]
        public string ExeFile { get; set; }

        [DataMember(Order = 6)]
        public string PackagePath { get; set; }

        /// <summary>安装包密码（密文）。</summary>
        [DataMember(Order = 7)]
        public string Password { get; set; }

        [DataMember(Order = 8)]
        public List<string> MergeDlls { get; set; }

        [DataMember(Order = 9)]
        public List<string> PackageIgnores { get; set; }

        public static ProjectData FromProject(Project project)
        {
            return new ProjectData
            {
                Id = project.Id,
                Name = project.Name,
                FolderName = project.FolderName,
                RootPath = project.RootPath,
                ExeFile = project.ExeFile,
                PackagePath = project.PackagePath,
                Password = CryptoHelper.Encrypt(project.Password),
                MergeDlls = project.MergeDlls ?? new List<string>(),
                PackageIgnores = project.PackageIgnores ?? new List<string>()
            };
        }

        public Project ToProject()
        {
            return new Project
            {
                Id = Id,
                Name = Name,
                FolderName = FolderName,
                RootPath = RootPath,
                ExeFile = ExeFile,
                PackagePath = PackagePath,
                Password = CryptoHelper.Decrypt(Password),
                MergeDlls = MergeDlls ?? new List<string>(),
                PackageIgnores = PackageIgnores ?? new List<string>()
            };
        }
    }
}
