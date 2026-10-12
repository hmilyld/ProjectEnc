using System;
using System.Security.Cryptography;
using System.Text;

namespace ProjectEnc.Helper
{
    /// <summary>
    /// 使用 Windows DPAPI（当前用户范围）对敏感文本进行加解密。
    /// 密文只能被同一 Windows 用户在同一台机器上解密。
    /// </summary>
    public static class CryptoHelper
    {
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return plainText;
            }

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(cipherBytes);
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return cipherText;
            }

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] plainBytes = ProtectedData.Unprotect(cipherBytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch
            {
                // 兼容历史明文数据：解密失败时按原值返回
                return cipherText;
            }
        }
    }
}
