using System;
using System.IO;
using System.Text;

namespace ProjectEnc.Helper
{
    /// <summary>
    /// 文件相关工具方法。
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 获取当前路径，结尾包含反斜杠
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>ConfuserEx 工程模板（.crproj）使用的编码。</summary>
        public static readonly Encoding ConfuserTemplateEncoding = Encoding.UTF8;

        /// <summary>Inno Setup 打包模板（.iss）使用的编码。</summary>
        public static readonly Encoding InnoTemplateEncoding = Encoding.GetEncoding("GB2312");

        /// <summary>
        /// 删除一个路径（文件或文件夹，文件夹会递归删除）
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}
