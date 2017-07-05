using System.Net.Http;
using System.Web;

namespace LTM.Common.Extensions
{
    public static class RequestContentExtensions
    {
        /// <summary>
        ///     将API httpRequestMessage对象转换为传统HttpContent对象
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        /// <returns></returns>
        public static HttpContextBase GetContextBase(this HttpRequestMessage httpRequestMessage)
        {
            return (HttpContextBase) httpRequestMessage.Properties["MS_HttpContext"];
        }
    }
}