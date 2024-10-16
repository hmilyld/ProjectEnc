using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProjectEnc.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 获取当前路径，结尾包含反斜杠
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 删除一个路径
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                // 如果路径是文件
                File.Delete(path);
            }
            else if (Directory.Exists(path))
            {
                // 如果路径是文件夹，并删除所有子内容
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destinationDir"></param>
        /// <param name="copySubDirs"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void CopyDirectory(string sourceDir, string destinationDir, bool copySubDirs)
        {
            // 获取源文件夹的信息
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            // 检查源文件夹是否存在
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"源文件夹不存在: {sourceDir}");
            }

            // 如果目标文件夹不存在，则创建
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // 复制文件夹中的所有文件
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(tempPath, false); // 复制文件，设置为不覆盖同名文件
            }

            // 如果需要递归复制子文件夹
            if (copySubDirs)
            {
                DirectoryInfo[] subDirs = dir.GetDirectories();
                foreach (DirectoryInfo subDir in subDirs)
                {
                    string tempPath = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, tempPath, copySubDirs); // 递归复制子文件夹
                }
            }
        }

        /// <summary>
        /// 序列化对象到本地二进制文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="obj"></param>
        public static void Serialize(string fileName, Object obj)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }

        /// <summary>
        /// 反序列化文件到对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            T obj = (T)bf.Deserialize(fs);
            fs.Close();
            return obj;
        }
    }
}