using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ProjectEnc.Helper
{
    /// <summary>
    /// 外部进程执行器：等待结束、异步读取输出、超时保护并校验退出码。
    /// </summary>
    public static class ProcessRunner
    {
        /// <summary>默认超时时间（10 分钟）。</summary>
        public const int DefaultTimeoutMs = 10 * 60 * 1000;

        /// <summary>
        /// 执行外部程序并返回其标准输出。失败或超时抛出异常，避免调用方误判成功。
        /// </summary>
        public static string Run(string exePath, string arguments, int timeoutMs = DefaultTimeoutMs)
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

                if (!process.WaitForExit(timeoutMs))
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                        // 忽略结束失败，下面统一抛出超时异常
                    }

                    throw new TimeoutException($"执行超时（超过 {timeoutMs / 1000} 秒）：{Path.GetFileName(exePath)}");
                }

                // 再次调用（无参）以确保异步输出事件已全部刷新
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    string detail = error.Length > 0 ? error.ToString().Trim() : output.ToString().Trim();
                    throw new InvalidOperationException(
                        $"{Path.GetFileName(exePath)} 执行失败（退出码 {process.ExitCode}）。{Environment.NewLine}{detail}");
                }

                return output.ToString().TrimEnd();
            }
        }

        /// <summary>
        /// 将路径包裹为单个命令行参数，避免含空格路径被拆分，并转义内部引号。
        /// </summary>
        public static string Quote(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "\"\"";
            }

            return "\"" + value.Replace("\"", "\\\"") + "\"";
        }
    }
}
