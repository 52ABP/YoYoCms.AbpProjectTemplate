using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using LTM.Common.Extensions;

namespace LTM.Common.Data
{
    /// <summary>
    ///     序列化辅助操作类
    /// </summary>
    public static class SerializeHelper
    {
        #region 二进制序列化

        /// <summary>
        ///     将数据序列化为二进制数组
        /// </summary>
        public static byte[] ToBinary(object data)
        {
            data.CheckNotNull(nameof(data));
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, data);
                ms.Seek(0, 0);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     将二进制数组反序列化为强类型数据
        /// </summary>
        public static T FromBinary<T>(byte[] bytes)
        {
            bytes.CheckNotNullOrEmpty(nameof(bytes));
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                return (T) formatter.Deserialize(ms);
            }
        }

        /// <summary>
        ///     将数据序列化为二进制数组并写入文件中
        /// </summary>
        public static void ToBinaryFile(object data, string fileName)
        {
            data.CheckNotNull(nameof(data));
            fileName.CheckFileExists(nameof(fileName));
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, data);
            }
        }

        /// <summary>
        ///     将指定二进制数据文件还原为强类型数据
        /// </summary>
        public static T FromBinaryFile<T>(string fileName)
        {
            fileName.CheckFileExists(nameof(fileName));
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (T) formatter.Deserialize(fs);
            }
        }

        #endregion 二进制序列化

        #region XML序列化

        /// <summary>
        ///     将数据序列化为XML形式
        /// </summary>
        public static string ToXml(object data)
        {
            data.CheckNotNull(nameof(data));
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(ms, data);
                ms.Seek(0, 0);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        /// <summary>
        ///     将XML数据反序列化为强类型
        /// </summary>
        public static T FromXml<T>(string xml)
        {
            xml.CheckNotNull(nameof(xml));
            var bytes = Encoding.Default.GetBytes(xml);
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof (T));
                return (T) serializer.Deserialize(ms);
            }
        }

        /// <summary>
        ///     将数据序列化为XML并写入文件
        /// </summary>
        public static void ToXmlFile(object data, string fileName)
        {
            data.CheckNotNull(nameof(data));
            fileName.CheckNotNullOrEmpty(nameof(fileName));
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(fs, data);
            }
        }

        /// <summary>
        ///     将指定XML数据文件还原为强类型数据
        /// </summary>
        public static T FromXmlFile<T>(string fileName)
        {
            fileName.CheckFileExists(nameof(fileName));
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof (T));
                return (T) serializer.Deserialize(fs);
            }
        }

        #endregion XML序列化
    }
}