using System.Xml;
using System.Xml.Linq;

namespace LTM.Common.Extensions
{
    /// <summary>
    ///     Xml 扩展操作类
    /// </summary>
    public static class LinqToXmlExtensions
    {
        #region 公共方法

        /// <summary>
        ///     将XmlNode转换为XElement
        /// </summary>
        /// <returns> XElment对象 </returns>
        public static XElement ToXElement(this XmlNode node)
        {
            var xdoc = new XDocument();
            using (var xmlWriter = xdoc.CreateWriter())
            {
                node.WriteTo(xmlWriter);
            }
            return xdoc.Root;
        }

        /// <summary>
        ///     将XElement转换为XmlNode
        /// </summary>
        /// <returns> 转换后的XmlNode </returns>
        public static XmlNode ToXmlNode(this XElement element)
        {
            using (var xmlReader = element.CreateReader())
            {
                var xml = new XmlDocument();
                xml.Load(xmlReader);
                return xml;
            }
        }

        #endregion 公共方法
    }
}