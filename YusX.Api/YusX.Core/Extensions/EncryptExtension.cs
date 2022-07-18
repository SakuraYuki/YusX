using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 加密解密扩展
    /// </summary>
    public static class EncryptExtensions
    {
        /// <summary>
        /// 加密/解密向量
        /// </summary>
        private static readonly byte[] IVBytes = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="content">待加密的内容</param>
        /// <param name="encryptKey">加密密钥，要求为16位</param>
        /// <returns>加密成功返回加密后的密文，失败返回源内容</returns>
        public static string EncryptDES(this string content, string encryptKey)
        {
            try
            {
                var keyBytes = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
                var contentBytes = Encoding.UTF8.GetBytes(content);
                using var creator = Aes.Create();
                using var encryptedSteam = new MemoryStream();
                using var outputSteam = new CryptoStream(encryptedSteam, creator.CreateEncryptor(keyBytes, IVBytes), CryptoStreamMode.Write);
                outputSteam.Write(contentBytes, 0, contentBytes.Length);
                outputSteam.FlushFinalBlock();
                return Convert.ToBase64String(encryptedSteam.ToArray()).Replace('+', '_').Replace('/', '~');
            }
            catch (Exception ex)
            {
                throw new Exception("内容加密异常：" + ex.Message);
            }
        }

        /// <summary> 
        /// DES解密字符串 
        /// </summary> 
        /// <param name="cipherText">待解密的密文</param> 
        /// <param name="decryptKey">解密密钥，要求为16位，和加密密钥相同</param> 
        /// <returns>解密成功返回解密后的内容，失败返源密文</returns>
        public static string DecryptDES(this string cipherText, string decryptKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 16));
            var cipherTextBytes = Convert.FromBase64String(cipherText.Replace('_', '+').Replace('~', '/'));
            using var creator = Aes.Create();
            using var contentSteam = new MemoryStream();
            using var outputStream = new CryptoStream(contentSteam, creator.CreateDecryptor(keyBytes, IVBytes), CryptoStreamMode.Write);
            var cipherContainer = new byte[cipherTextBytes.Length];
            outputStream.Write(cipherTextBytes, 0, cipherTextBytes.Length);
            outputStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(contentSteam.ToArray());
        }

        /// <summary>
        /// 尝试解密DES
        /// </summary>
        /// <param name="cipherText">待解密的密文</param>
        /// <param name="decryptKey">解密密钥，要求为16位，和加密密钥相同</param> 
        /// <param name="result">解密成功后的密文，若解密失败则为空字符串</param>
        /// <returns>解密成功返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        public static bool TryDecryptDES(this string cipherText, string decryptKey, out string result)
        {
            result = string.Empty;
            try
            {
                result = DecryptDES(cipherText, decryptKey);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
