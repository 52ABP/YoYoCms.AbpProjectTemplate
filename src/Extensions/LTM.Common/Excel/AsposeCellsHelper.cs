#region 版权信息

// ------------------------------------------------------------------------------
// Copyright © 积微物联有限公司 版权所有。 
// 项目名：LTM.Common 
// 文件名：AsposeCellsHelper.cs
// 创建标识：梅军章  2017-04-11 16:59
// 创建描述：
// 
// 修改标识：
// 修改描述：
//  ------------------------------------------------------------------------------

#endregion

using System;
using System.Data;
using System.IO;
using Aspose.Cells;

namespace LTM.Common.Excel
{
    /// <summary>
    ///   AsposeCellsHelper的Excel帮助类
    /// </summary>
    public static class AsposeCellsHelper
    {
        /// <summary>
        ///  导入Excel数据
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns>DataTable</returns>
        public static DataTable Import(Stream stream)
        {
            var book = new Workbook(stream);
            var sheet = book.Worksheets[0];
            var cells = sheet.Cells;

            var importDatatable = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);

            return importDatatable;

        }

        /// <summary>
        /// 导入Excel数据
        /// </summary>
        /// <param name="file"><![CDATA[文件路径，必须为全路径格式:D:\\积微物联\\Jwll.CloudProcurementNew\xxx.xlxs]]></param>
        /// <returns>DataTable</returns>
        public static DataTable Import(string file)
        {
            var book = new Workbook(file);
            var sheet = book.Worksheets[0];
            var cells = sheet.Cells;

            var importDatatable = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);

            return importDatatable;
        }

        /// <summary>
        ///     数据导出 说明：解决工作表中有超过65536条数据的情况
        /// </summary>
        /// <param name="dataTable">需要导出的DataTable</param>
        /// <param name="sheetName">工作单元名字</param>
        public static MemoryStream Export(DataTable dataTable, string sheetName)
        {
            /*默认开启一个工作簿，并要求excel的格式为最低支持97和03的版本*/
            var wb = new Workbook(FileFormatType.Excel97To2003);
            wb.Worksheets.RemoveAt(0); //移除默认的一个工作表
            /*解决如果一个工作表中有超过65536条数据的情况*/
            var totalCount = dataTable.Rows.Count;
            const int sheetSize = 65536; //每个工作表显示多少条
            if (totalCount > sheetSize)
            {
                var s = totalCount / sheetSize; //取商
                var y = totalCount % sheetSize; //取余
                int sheetCount; //需要创建工作表的数量
                /*获取工作表的数量*/
                if (y > 0)
                {
                    sheetCount = s + 1;
                }
                else
                {
                    sheetCount = s;
                }
                /*遍历创建数据*/
                for (var i = 0; i < sheetCount; i++)
                {
                    wb.Worksheets.Add(sheetName + " 第" + (i + 1) + "页"); //动态创建工作表
                    /*动态创建DataTable*/
                    var dataTableNew = dataTable.Clone();
                    /*如果是最后一页的话*/
                    if (i == sheetCount - 1)
                    {
                        for (var j = i * sheetSize; j < totalCount; j++)
                        {
                            dataTableNew.ImportRow(dataTable.Rows[j]);
                        }
                    }
                    else
                    {
                        /*做整数页的数据*/
                        for (var j = i * sheetSize; j <= (i + 1) * sheetSize - 1; j++)
                        {
                            dataTableNew.ImportRow(dataTable.Rows[j]);
                        }
                    }
                    /*将数据添加到集合中去*/
                    wb.Worksheets[i].Cells.ImportDataTable(dataTableNew, true, "A1");
                }
            }
            else
            {
                wb.Worksheets.Add(sheetName); //动态创建工作表
                var sheet = wb.Worksheets[0];
                sheet.Cells.ImportDataTable(dataTable, true, "A1");
            }

            return wb.SaveToStream();
        }
    }
}