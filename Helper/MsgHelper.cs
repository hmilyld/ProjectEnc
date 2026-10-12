using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace ProjectEnc.Helper
{
    public class MsgHelper
    {
        public static void Show(string text)
        {
            XtraMessageBox.Show(text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Success()
        {
            XtraMessageBox.Show("操作成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warn(string text)
        {
            XtraMessageBox.Show(text, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(Exception ex)
        {
            LogHelper.Error(ex);

            string text = ex == null
                ? "发生未知错误，详情请查看程序目录下的 Logs 日志。"
                : $"操作失败：{ex.Message}{Environment.NewLine}详情请查看程序目录下的 Logs 日志。";

            XtraMessageBox.Show(text, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Error(string text)
        {
            text = string.Format("错误提示：{0}", text);
            XtraMessageBox.Show(text, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Confirm(string text)
        {
            return XtraMessageBox.Show(text, "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult ConfirmDelete()
        {
            return XtraMessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}