using System.Security.Cryptography;
using System.Text;
using LTM.Common.Extensions;

namespace LTM.Common.Secutiry
{
    /// <summary>
    ///     字符串Hash操作类
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        ///     获取字符串的MD5哈希值
        /// </summary>
        public static string GetMd5(string value, Encoding encoding = null)
        {
            value.CheckNotNull(nameof(value));
            if (encoding == null)
            {
                encoding = Encoding.ASCII;
            }
            var bytes = encoding.GetBytes(value);
            return GetMd5(bytes);
        }

        /// <summary>
        ///     获取字节数组的MD5哈希值
        /// </summary>
        public static string GetMd5(byte[] bytes)
        {
            bytes.CheckNotNullOrEmpty(nameof(bytes));
            var sb = new StringBuilder();
            MD5 hash = new MD5CryptoServiceProvider();
            bytes = hash.ComputeHash(bytes);
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     获取字符串的SHA1哈希值
        /// </summary>
        public static string GetSha1(string value)
        {
            value.CheckNotNullOrEmpty(nameof(value));

            var sb = new StringBuilder();
            var hash = new SHA1Managed();
            var bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(value));
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     获取字符串的Sha256哈希值
        /// </summary>
        public static string GetSha256(string value)
        {
            value.CheckNotNullOrEmpty(nameof(value));

            var sb = new StringBuilder();
            var hash = new SHA256Managed();
            var bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(value));
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     获取字符串的Sha512哈希值
        /// </summary>
        public static string GetSha512(string value)
        {
            value.CheckNotNullOrEmpty(nameof(value));

            var sb = new StringBuilder();
            var hash = new SHA512Managed();
            var bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(value));
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
    }
}