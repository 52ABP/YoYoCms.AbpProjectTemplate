using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LTM.Common.Web
{
    public class WebHelper
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="urlFormat">页码链接格式，页码的占位符为{pageIndex}，比如/Message/Page/{pageIndex}</param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="totalCount">总数据条数</param>
        /// <param name="pageSize">每一页显示多少数据</param>
        /// <returns></returns>
        public static HtmlString Pager(string urlFormat, int pageIndex,
            int totalCount, int pageSize = 5)
        {
            StringBuilder sbHTML = new StringBuilder();
            int pageCount = (int)Math.Ceiling(totalCount * 1.0 / pageSize);//一共显示多少页
            int startPageIndex = Math.Max(1, pageIndex - 5);//不会一下子把所有页码都显示出来，在当前页码前再最多显示5个
            int endPageIndex = Math.Min(pageCount, startPageIndex + 10 - 1);//一共最多显示10个页码

            if (pageIndex != 1)
            {
                sbHTML.Append(" <li> <a href='")
                       .Append(urlFormat.Replace("{pageIndex}", (pageIndex - 1).ToString()))
                       .Append("'>")
                       .Append("上一页")
                       .Append("</a></li>");
            }
            else
            {
                sbHTML.Append(" <li> <a href='")
                      .Append(urlFormat.Replace("{pageIndex}", "1"))
                      .Append("'>")
                      .Append("上一页")
                      .Append("</a></li>");
            }

            for (int i = startPageIndex; i <= endPageIndex; i++)
            {
                if (pageIndex == i)
                {
                    sbHTML.Append(" <li class='active'> <a href='")
                        .Append(urlFormat.Replace("{pageIndex}", i.ToString()))
                        .Append("'>")
                        .Append(i)
                        .Append("<span class='sr-only'>(current)</span>").Append("</a></li>");
                }
                else
                {
                    sbHTML.Append(" <li> <a href='")
                        .Append(urlFormat.Replace("{pageIndex}", i.ToString()))
                        .Append("'>")
                        .Append(i)
                        .Append("</a></li>");
                }
            }
            if (pageIndex != endPageIndex)
            {
                sbHTML.Append(" <li> <a href='")
                      .Append(urlFormat.Replace("{pageIndex}", (pageIndex + 1).ToString()))
                      .Append("'>")
                      .Append("下一页")
                      .Append("</a></li>");
            }
            else
            {
                sbHTML.Append(" <li> <a href='")
                      .Append(urlFormat.Replace("{pageIndex}", endPageIndex.ToString()))
                      .Append("'>")
                      .Append("下一页")
                      .Append("</a></li>");
            }

            return new HtmlString(sbHTML.ToString());
        }
    }
}
