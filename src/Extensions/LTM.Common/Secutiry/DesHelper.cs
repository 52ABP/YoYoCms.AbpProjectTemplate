using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LTM.Common.Extensions;
using LTM.Common.Properties;

namespace LTM.Common.Secutiry
{
    /// <summary>
    ///     DES / TripleDES加密解密操作类
    /// </summary>
    public class DesHelper
    {
        private const int BufferAppendSize = 64;
        private const string SectionSign = "?SECTION?";
        private readonly bool _isTriple;

        /// <summary>
        ///     使用随机密码初始化一个<see cref="DesHelper" />类的新实例
        /// </summary>
        /// <param name="isTriple">是否使用TripleDES方式，否则为DES方式</param>
        public DesHelper(bool isTriple = false)
            : this(isTriple
                ? new TripleDESCryptoServiceProvider().Key
                : new DESCryptoServiceProvider().Key)
        {
            _isTriple = isTriple;
        }

        /// <summary>
        ///     获取 密钥
        /// </summary>
        public byte[] Key { get; }

        #region 实例方法

        /// <summary>
        ///     使用指定8位密码初始化一个<see cref="DesHelper" />类的新实例
        /// </summary>
        public DesHelper(byte[] key)
        {
            key.CheckNotNull(nameof(key));
            key.Required(k => k.Length == 8 || k.Length == 24,
                string.Format(Resources.Security_DES_KeyLenght, key.Length));
            Key = key;
            _isTriple = key.Length == 24;
        }

        /// <summary>
        ///     加密字节数组
        /// </summary>
        /// <param name="source">要加密的字节数组</param>
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] source)
        {
            source.CheckNotNull(nameof(source));
            var provider = _isTriple
                ? (SymmetricAlgorithm) new TripleDESCryptoServiceProvider {Key = Key, Mode = CipherMode.ECB}
                : new DESCryptoServiceProvider {Key = Key, Mode = CipherMode.ECB};

            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(source, 0, source.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     解密字节数组
        /// </summary>
        /// <param name="source">要解密的字节数组</param>
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] source)
        {
            source.CheckNotNull(nameof(source));

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var provider = _isTriple
                ? (SymmetricAlgorithm) new TripleDESCryptoServiceProvider {Key = Key, Mode = CipherMode.ECB}
                : new DESCryptoServiceProvider {Key = Key, Mode = CipherMode.ECB};
            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(source, 0, source.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     加密字符串，输出BASE64编码字符串
        /// </summary>
        /// <param name="source">要加密的明文字符串</param>
        /// <returns>加密的BASE64编码的字符串</returns>
        public string Encrypt(string source)
        {
            source.CheckNotNull(nameof(source));
            var bytes = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(Encrypt(bytes));
        }

        /// <summary>
        ///     解密字符串，输入为BASE64编码字符串
        /// </summary>
        /// <param name="source">要解密的BASE64编码的字符串</param>
        /// <returns>明文字符串</returns>
        public string Decrypt(string source)
        {
            source.CheckNotNullOrEmpty(nameof(source));
            var bytes = Convert.FromBase64String(source);
            return Encoding.UTF8.GetString(Decrypt(bytes));
        }

        /// <summary>
        ///     整体加密文件
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        public void EncryptFile(string sourceFile, string targetFile)
        {
            sourceFile.CheckFileExists(nameof(sourceFile));
            targetFile.CheckNotNullOrEmpty(nameof(targetFile));

            using (FileStream ifs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read),
                ofs = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                var length = ifs.Length;
                var sourceBytes = new byte[length];
                ifs.Read(sourceBytes, 0, sourceBytes.Length);
                var targetBytes = Encrypt(sourceBytes);
                ofs.Write(targetBytes, 0, targetBytes.Length);
            }
        }

        /// <summary>
        ///     分段加密文件
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        /// <param name="sectionLength">分段大小（字节）</param>
        public void EncryptFile(string sourceFile, string targetFile, int sectionLength)
        {
            sourceFile.CheckFileExists(nameof(sourceFile));
            targetFile.CheckNotNullOrEmpty(nameof(targetFile));
            sectionLength.CheckGreaterThan(nameof(sectionLength), 0);

            using (FileStream ifs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read),
                ofs = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                //追加附加数据到加密文件开关
                var decryptFileSize = ifs.Length;
                var appendBytes = new byte[BufferAppendSize];

                //0位为加密分段大小，1位为未加密文件的长度
                //附加信息格式为：{分段长度}|{明文文件长度}|{结束0标记}
                var appendStr = "{0}|{1}|{2}".FormatWith(sectionLength, decryptFileSize, 0);
                appendStr = Encrypt(appendStr);
                var sectionSignSize = (SectionSign + "|").Length;
                //密文附加信息长度
                var appendStrSize = appendStr.Length + sectionSignSize;
                //附加信息格式为：{附加信息长度}|{分段长度}|{明文文件长度}|{结束0标记}
                appendStr = "{0}|{1}".FormatWith(appendStrSize, appendStr);

                //在文件最开关添加分段标记Section_Sign，说明文件是分段加密文件
                //附加串信息格式为：{分段加密标记}|{附加信息长度}|{分段长度}|{明文文件长度}|{结束0标记}
                appendStr = "{0}|{1}".FormatWith(SectionSign, appendStr);
                appendStr = appendStr.Replace("|" + appendStrSize.ToString(CultureInfo.InvariantCulture) + "|",
                    "|" + appendStr.Length.ToString(CultureInfo.InvariantCulture) + "|");
                var tmpBytes = Encoding.UTF8.GetBytes(appendStr);
                using (var ms = new MemoryStream(appendBytes))
                {
                    ms.Write(tmpBytes, 0, tmpBytes.Length);
                    appendBytes = ms.ToArray();
                }

                ofs.Seek(0, SeekOrigin.Begin);
                ofs.Write(appendBytes, 0, appendBytes.Length);

                var fileSize = ifs.Length;
                var sectionCount = fileSize/sectionLength;
                var lastLength = (int) (fileSize%sectionLength);

                int length;
                var sourceBytes = new byte[sectionLength];
                if (sectionCount > 0)
                {
                    length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                }
                else
                {
                    sourceBytes = new byte[lastLength];
                    length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                }
                while (length > 0)
                {
                    var targetBytes = Encrypt(sourceBytes);
                    ofs.Write(targetBytes, 0, targetBytes.Length);
                    sectionCount--;
                    if (sectionCount > 0)
                    {
                        length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                    }
                    else
                    {
                        sourceBytes = new byte[lastLength];
                        length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                    }
                }
            }
        }

        /// <summary>
        ///     对文件内容进行DES解密，能自动识别并处理是否为分段加密
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        public void DecryptFile(string sourceFile, string targetFile)
        {
            sourceFile.CheckFileExists(nameof(sourceFile));
            targetFile.CheckNotNullOrEmpty(nameof(targetFile));

            using (FileStream ifs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read),
                ofs = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                //判断是否分段加密
                var isSection = CheckSectionSign(ifs);
                if (!isSection)
                {
                    ifs.Seek(0, SeekOrigin.Begin);
                    var length = ifs.Length;
                    var sourceBytes = new byte[length];
                    ifs.Read(sourceBytes, 0, sourceBytes.Length);
                    var targetBytes = Decrypt(sourceBytes);
                    ofs.Write(targetBytes, 0, targetBytes.Length);
                }
                else
                {
                    //从加密文件中读取附加信息，获取加密时分段大小
                    ifs.Seek(0, SeekOrigin.Begin);
                    var appendBytes = new byte[BufferAppendSize];
                    ifs.Read(appendBytes, 0, appendBytes.Length);
                    var appendStr = Encoding.UTF8.GetString(appendBytes);
                    var tmpAppend = appendStr.Substring(SectionSign.Length + 1);
                    var appendStrSize =
                        Convert.ToInt32(tmpAppend.Substring(0, tmpAppend.IndexOf("|", StringComparison.Ordinal)));
                    appendStr = appendStr.Substring(0, appendStrSize);
                    tmpAppend = appendStr.Substring(appendStr.LastIndexOf("|", StringComparison.Ordinal) + 1,
                        appendStr.Length - appendStr.LastIndexOf("|", StringComparison.Ordinal) - 1);
                    appendStr = Decrypt(tmpAppend);
                    var sectionLength = Convert.ToInt32(appendStr.Split('|')[0]);
                    ifs.Seek(BufferAppendSize, SeekOrigin.Begin); //把文件读取指针移到附加信息后面
                    sectionLength = Encrypt(new byte[sectionLength]).Length;
                    var fileSize = ifs.Length;
                    fileSize -= BufferAppendSize;
                    var sectionCount = fileSize/sectionLength; //段数
                    var laseLength = (int) (fileSize%sectionLength); //最后一段长度
                    int length;
                    var sourceBytes = new byte[sectionLength]; //加密数据缓冲区
                    if (sectionCount > 0)
                    {
                        length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                    }
                    else
                    {
                        sourceBytes = new byte[laseLength];
                        length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                    }
                    while (length > 0)
                    {
                        var targetBytes = Decrypt(sourceBytes);
                        ofs.Write(targetBytes, 0, targetBytes.Length);
                        sectionCount--;
                        if (sectionCount > 0)
                        {
                            length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                        }
                        else if (sectionCount == 0)
                        {
                            sourceBytes = new byte[laseLength];
                            length = ifs.Read(sourceBytes, 0, sourceBytes.Length);
                        }
                        else
                        {
                            length = 0;
                        }
                    }
                }
            }
        }

        private static bool CheckSectionSign(Stream ifs)
        {
            var sectionSignSize = SectionSign.Length;
            ifs.Seek(0, SeekOrigin.Begin);
            var sectionSignBytes = new byte[sectionSignSize];
            ifs.Read(sectionSignBytes, 0, sectionSignBytes.Length);
            var sectionSignString = Encoding.UTF8.GetString(sectionSignBytes);
            return sectionSignString == SectionSign;
        }

        #endregion 实例方法

        #region 静态方法

        /// <summary>
        ///     加密字节数组
        /// </summary>
        /// <param name="source">要加密的字节数组</param>
        /// <param name="key">密钥字节数组，长度为8或者24</param>
        /// <returns>加密后的字节数组</returns>
        public static byte[] Encrypt(byte[] source, byte[] key)
        {
            var des = new DesHelper(key);
            return des.Encrypt(source);
        }

        /// <summary>
        ///     解密字节数组
        /// </summary>
        /// <param name="source">要解密的字节数组</param>
        /// <param name="key">密钥字节数组，长度为8或者24</param>
        /// <returns>解密后的字节数组</returns>
        public static byte[] Decrypt(byte[] source, byte[] key)
        {
            var des = new DesHelper(key);
            return des.Decrypt(source);
        }

        /// <summary>
        ///     加密字符串，输出BASE64编码字符串
        /// </summary>
        /// <param name="source">要加密的明文字符串</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        /// <returns>加密的BASE64编码的字符串</returns>
        public static string Encrypt(string source, string key)
        {
            source.CheckNotNull(nameof(source));
            key.CheckNotNullOrEmpty(nameof(key));
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var des = new DesHelper(keyBytes);
            return des.Encrypt(source);
        }

        /// <summary>
        ///     解密字符串，输入BASE64编码字符串
        /// </summary>
        /// <param name="source">要解密的BASE64编码字符串</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        /// <returns>解密的明文字符串</returns>
        public static string Decrypt(string source, string key)
        {
            source.CheckNotNullOrEmpty(nameof(source));
            key.CheckNotNullOrEmpty(nameof(key));
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var des = new DesHelper(keyBytes);
            return des.Decrypt(source);
        }

        /// <summary>
        ///     判断字符串是否可以正确被解密
        /// </summary>
        /// <param name="source">要解密的BASE64编码字符串</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        /// <returns></returns>
        public static bool IsDecrypt(string source, string key)
        {
            try
            {
                Decrypt(source, key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     整体加密文件
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        public static void EncryptFile(string sourceFile, string targetFile, string key)
        {
            sourceFile.CheckFileExists(nameof(sourceFile));
            targetFile.CheckNotNullOrEmpty(nameof(targetFile));
            key.CheckNotNullOrEmpty(nameof(key));

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var des = new DesHelper(keyBytes);
            des.EncryptFile(sourceFile, targetFile);
        }

        /// <summary>
        ///     分段加密文件
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        /// <param name="sectionLength">分段大小（字节）</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        public static void EncryptFile(string sourceFile, string targetFile, int sectionLength, string key)
        {
            sourceFile.CheckFileExists(nameof(sourceFile));
            targetFile.CheckNotNullOrEmpty(nameof(targetFile));
            key.CheckNotNullOrEmpty(nameof(key));
            sectionLength.CheckGreaterThan(nameof(sectionLength), 0);

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var des = new DesHelper(keyBytes);
            des.EncryptFile(sourceFile, targetFile, sectionLength);
        }

        /// <summary>
        ///     对文件内容进行DES解密，能自动识别并处理是否为分段加密
        /// </summary>
        /// <param name="sourceFile">待加密的文件名</param>
        /// <param name="targetFile">保存加密文件名</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        public static void DecryptFile(string sourceFile, string targetFile, string key)
        {
            sourceFile.CheckFileExists(nameof(sourceFile));
            targetFile.CheckNotNullOrEmpty(nameof(targetFile));
            key.CheckNotNullOrEmpty(nameof(key));

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var des = new DesHelper(keyBytes);
            des.DecryptFile(sourceFile, targetFile);
        }

        #endregion 静态方法
    }
}