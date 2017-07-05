using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using LTM.Common.Collections;
using OfficeOpenXml;

namespace LTM.Common.Epplus
{
    /// <summary>
    ///     EpPlus帮助类
    /// </summary>
    public class EpPlusHelper
    {
        #region 读取Excel表转成datatable

        /// <summary>
        ///     读取Excel表转成datatable
        /// </summary>
        /// <param name="oSheet">oSheet表格</param>
        /// <returns></returns>
        public static DataTable WorksheetToDataTable(ExcelWorksheet oSheet)
        {
            var totalRows = oSheet.Dimension.End.Row;
            var totalCols = oSheet.Dimension.End.Column;
            var dt = new DataTable(oSheet.Name);
            DataRow dr = null;
            for (var i = 1; i <= totalRows; i++)
            {
                if (i > 1) dr = dt.Rows.Add();
                for (var j = 1; j <= totalCols; j++)
                {
                    if (i == 1)
                        dt.Columns.Add(oSheet.Cells[i, j].Value.ToString());
                    else
                        dr[j - 1] = oSheet.Cells[i, j].Value.ToString();
                }
            }
            return dt;
        }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <typeparam name="T">每行数据的类型</typeparam>
        /// <param name="FileName">Excel文件名</param>
        /// <returns>泛型列表</returns>
        public static IEnumerable<T> LoadFromExcel<T>(string FileName) where T : new()
        {
            var tempFilePath = HttpContext.Current.Server.MapPath(FileName);
            var existingFile = new FileInfo(tempFilePath);
            IList<T> resultList = new List<T>();
            var dictHeader = new Dictionary<string, int>();

            using (var package = new ExcelPackage(existingFile))
            {
                var worksheet = package.Workbook.Worksheets[1];
                var dt = WorksheetToDataTable(worksheet);
                resultList = ConvertHelper.ConvertTo<T>(dt);
            }
            return resultList;
        }

        #endregion
    }
}