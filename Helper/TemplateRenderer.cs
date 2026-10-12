using System;
using System.IO;
using System.Text;

namespace ProjectEnc.Helper
{
    /// <summary>
    /// 模板渲染器：读取模板文件、执行占位符替换、写出临时文件。
    /// </summary>
    public static class TemplateRenderer
    {
        /// <summary>
        /// 读取 <paramref name="templatePath"/>，交由 <paramref name="modify"/> 处理占位符后写入
        /// <paramref name="outputPath"/>，返回输出路径。
        /// </summary>
        public static string Render(string templatePath, string outputPath, Encoding encoding, Func<string, string> modify)
        {
            string content = File.ReadAllText(templatePath, encoding);
            content = modify(content);
            File.WriteAllText(outputPath, content, encoding);
            return outputPath;
        }
    }
}
