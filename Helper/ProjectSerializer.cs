using ProjectEnc.Bean;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;

namespace ProjectEnc.Helper
{
    /// <summary>
    /// 工程文件读写。新格式为 JSON；读取时自动兼容历史二进制文件，并在读取成功后迁移为 JSON。
    /// </summary>
    public static class ProjectSerializer
    {
        public static void Save(Project project, string filePath)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            ProjectData data = ProjectData.FromProject(project);
            var serializer = new DataContractJsonSerializer(typeof(ProjectData));

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                serializer.WriteObject(stream, data);
            }
        }

        public static Project Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            ProjectData data = TryLoadJson(filePath);
            if (data != null)
            {
                return data.ToProject();
            }

            // 兼容历史二进制工程文件
            Project legacy = TryLoadLegacy(filePath);
            if (legacy != null)
            {
                try
                {
                    // 读取成功后立即迁移为 JSON 格式
                    Save(legacy, filePath);
                }
                catch
                {
                    // 迁移失败不影响本次读取结果
                }
            }

            return legacy;
        }

        private static ProjectData TryLoadJson(string filePath)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(ProjectData));

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    return serializer.ReadObject(stream) as ProjectData;
                }
            }
            catch
            {
                return null;
            }
        }

        private static Project TryLoadLegacy(string filePath)
        {
            try
            {
                var formatter = new BinaryFormatter { Binder = new LegacyProjectBinder() };

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    return formatter.Deserialize(stream) as Project;
                }
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 反序列化类型白名单：仅允许工程模型及其字符串集合，
    /// 阻断 BinaryFormatter 的 gadget 链攻击（防远程代码执行）。
    /// </summary>
    internal sealed class LegacyProjectBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (typeName == typeof(Project).FullName)
            {
                return typeof(Project);
            }

            if (typeName != null && typeName.StartsWith("System.Collections.Generic.List`1[[System.String", StringComparison.Ordinal))
            {
                return typeof(List<string>);
            }

            if (typeName == typeof(string[]).FullName)
            {
                return typeof(string[]);
            }

            if (typeName == typeof(string).FullName)
            {
                return typeof(string);
            }

            throw new SerializationException($"出于安全考虑，不允许反序列化类型：{typeName}");
        }
    }
}
