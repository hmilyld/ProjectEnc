using System;
using System.IO;
using System.Text;

namespace ProjectEnc.Helper
{
    /// <summary>
    /// 轻量日志工具：按天写入程序目录下的 Logs 文件夹，线程安全，零第三方依赖。
    /// </summary>
    public static class LogHelper
    {
        private static readonly object SyncRoot = new object();

        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        public static void Info(string message) => Write("INFO", message);

        public static void Warn(string message) => Write("WARN", message);

        public static void Error(string message) => Write("ERROR", message);

        public static void Error(Exception exception) => Write("ERROR", exception == null ? "发生未知异常。" : exception.ToString());

        private static void Write(string level, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            try
            {
                lock (SyncRoot)
                {
                    if (!Directory.Exists(LogDirectory))
                    {
                        Directory.CreateDirectory(LogDirectory);
                    }

                    string filePath = Path.Combine(LogDirectory, $"{DateTime.Now:yyyy-MM-dd}.log");
                    string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";
                    File.AppendAllText(filePath, line, Encoding.UTF8);
                }
            }
            catch
            {
                // 日志写入失败不应影响主流程
            }
        }
    }
}
