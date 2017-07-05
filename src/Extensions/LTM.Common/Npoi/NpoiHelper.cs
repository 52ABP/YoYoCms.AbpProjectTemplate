using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LTM.Common.Npoi
{
    public  class NpoiHelper
    {
        #region 属性

        private readonly int _perSheetCount = 40000;//每个sheet要保存的条数
        public NpoiHelper()
        { }
        /// <summary>
        /// 最大接收5万条每页，大于5万时，使用系统默认的值(4万)
        /// </summary>
        /// <param name="perSheetCounts"></param>
        public NpoiHelper(int perSheetCounts)
        {
            if (_perSheetCount <= 50000)
                _perSheetCount = perSheetCounts;
        }

        #endregion

        #region IExcelProvider 成员

        public DataTable Import(Stream fs, string ext, out string msg, List<string> validates = null)
        {
            msg = string.Empty;
            var dt = new DataTable();
            try
            {
                IWorkbook workbook;
                if (ext == ".xls")
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else
                {
                    workbook = new XSSFWorkbook(fs);
                }
                const int num = 0;
                var sheet = workbook.GetSheetAt(num);
                dt.TableName = sheet.SheetName;
                var rowCount = sheet.LastRowNum;
                const int firstNum = 0;
                var headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                if (validates != null)
                {
                    var validateCount = validates.Count;
                    if (validateCount > cellCount)
                    {
                        msg = "上传EXCEL文件格式不正确";
                        return null;
                    }
                    for (var i = 0; i < validateCount; i++)
                    {
                        var columnName = headerRow.GetCell(i).StringCellValue;
                        if (validates[i] == columnName) continue;
                        msg = "上传EXCEL文件格式不正确";
                        return null;
                    }
                }
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    dt.Columns.Add(column);
                }
                for (var i = (firstNum + 1); i <= rowCount; i++)
                {
                    var row = sheet.GetRow(i);
                    var dataRow = dt.NewRow();
                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = GetCellValue(row.GetCell(j), ext);
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static IFont GetFont(IWorkbook workbook, HSSFColor color)
        {
            var font = workbook.CreateFont();
            font.Color = color.Indexed;
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            //font.FontName = "楷体";
            font.IsItalic = true;
            return font;
        }

        public void SetCellValues(ICell cell, string cellType, string cellValue)
        {
            switch (cellType)
            {
                case "System.String": //字符串类型
                    double result;
                    if (double.TryParse(cellValue, out result))
                        cell.SetCellValue(result);
                    else
                        cell.SetCellValue(cellValue);
                    break;
                case "System.DateTime": //日期类型
                    DateTime dateV;
                    DateTime.TryParse(cellValue, out dateV);
                    cell.SetCellValue(dateV);
                    break;
                case "System.Boolean": //布尔型
                    bool boolV;
                    bool.TryParse(cellValue, out boolV);
                    cell.SetCellValue(boolV);
                    break;
                case "System.Int16": //整型
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV;
                    int.TryParse(cellValue, out intV);
                    cell.SetCellValue(intV);
                    break;
                case "System.Decimal": //浮点型
                case "System.Double":
                    double doubV;
                    double.TryParse(cellValue, out doubV);
                    cell.SetCellValue(doubV);
                    break;
                case "System.DBNull": //空值处理
                    cell.SetCellValue("");
                    break;
                default:
                    cell.SetCellValue("");
                    break;
            }
        }

        public string Export(string excelFileName, DataTable dtIn)
        {
            var workbook = new HSSFWorkbook();
            ICell cell;
            var sheetCount = 1;//当前的sheet数量
            var currentSheetCount = 0;//循环时当前保存的条数，每页都会清零

            //表头样式
            var style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            var green = new HSSFColor.Green();
            style.SetFont(GetFont(workbook, green));

            //内容样式
            style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            var blue = new HSSFColor.Blue();
            style.SetFont(GetFont(workbook, blue));

            var sheet = workbook.CreateSheet("Sheet" + sheetCount);
            //填充表头
            var row = sheet.CreateRow(0);
            for (var i = 0; i < dtIn.Columns.Count; i++)
            {
                cell = row.CreateCell(i);
                cell.SetCellValue(dtIn.Columns[i].ColumnName);
                cell.CellStyle = style;
            }
            //填充内容
            for (var i = 0; i < dtIn.Rows.Count; i++)
            {
                if (currentSheetCount >= _perSheetCount)
                {
                    sheetCount++;
                    currentSheetCount = 0;
                    sheet = workbook.CreateSheet("Sheet" + sheetCount);
                }
                row = sheetCount == 1 ? sheet.CreateRow(currentSheetCount + 1) : sheet.CreateRow(currentSheetCount);
                currentSheetCount++;
                for (var j = 0; j < dtIn.Columns.Count; j++)
                {
                    cell = row.CreateCell(j);
                    cell.CellStyle = style;
                    SetCellValues(cell, dtIn.Columns[j].DataType.ToString(), dtIn.Rows[i][j].ToString());
                }
            }
            var fs = new FileStream(excelFileName, FileMode.CreateNew, FileAccess.Write);
            workbook.Write(fs);
            fs.Close();
            return excelFileName;
        }

        public DataTable Import(string filepath, string key, string sheetName, string endKey)
        {
            var table = new DataTable();
            try
            {
                using (var excelFileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    var file = Path.GetExtension(filepath);
                    if (file != null)
                    {
                        var type = file.Replace(".", "");
                        IWorkbook workbook;
                        if (type == "xls")
                        {
                            workbook = new HSSFWorkbook(excelFileStream);
                        }
                        else
                        {
                            workbook = new XSSFWorkbook(excelFileStream);
                        }

                        for (var num = 0; num < workbook.NumberOfSheets; num++)
                        {

                            var sheet = workbook.GetSheetAt(num);
                            if (sheet.SheetName != sheetName)
                            {
                                continue;
                            }
                            table.TableName = sheet.SheetName;
                            var rowCount = sheet.LastRowNum;
                            IRow headerRow = null;
                            var cellCount = 0;
                            var firstNum = 0;

                            for (var i = 0; i <= rowCount; i++)
                            {
                                if (sheet.GetRow(i).GetCell(0).StringCellValue != key) continue;
                                headerRow = sheet.GetRow(i);
                                cellCount = headerRow.LastCellNum;
                                firstNum = i;
                                break;
                            }

                            //列名

                            //handling header. 
                            if (headerRow != null)
                                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                                {
                                    var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                                    table.Columns.Add(column);
                                }

                            for (var i = (firstNum + 1); i <= rowCount; i++)
                            {
                                var row = sheet.GetRow(i);
                                var dataRow = table.NewRow();
                                var isEnd = false;
                                if (row != null)
                                {
                                    for (int j = row.FirstCellNum; j < cellCount; j++)
                                    {

                                        if (row.GetCell(j) != null)
                                            dataRow[j] = GetCellValue(row.GetCell(j), type);
                                        if (dataRow[j].ToString() != endKey) continue;
                                        isEnd = true;
                                        break;
                                    }
                                }
                                if (isEnd)
                                {
                                    break;
                                }
                                table.Rows.Add(dataRow);
                            }
                            return table;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return table;
        }

        private static string GetCellValue(ICell cell, string type)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                    var format = cell.CellStyle.DataFormat;
                    if (format == 14 || format == 31 || format == 57 || format == 58)
                    {
                        var date = cell.DateCellValue;
                        var re = date.ToString("yyy-MM-dd");
                        return re;
                    }
                    return cell.ToString();

                case CellType.String:
                    return cell.StringCellValue;

                case CellType.Formula:
                    try
                    {
                        if (type == "xls")
                        {
                            var e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                            e.EvaluateInCell(cell);
                            return cell.ToString();
                        }
                        else
                        {
                            var e = new XSSFFormulaEvaluator(cell.Sheet.Workbook);
                            e.EvaluateInCell(cell);
                            return cell.ToString();
                        }
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                    }
                case CellType.Unknown:
                    return cell.ToString();
                default:
                    return cell.ToString();

            }
        }

        #endregion

        #region 辅助导入

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datatable"></param>
        /// <returns></returns>
        public IEnumerable<T> ConvertTo<T>(DataTable datatable) where T : new()
        {
            var temp = new List<T>();
            try
            {
                var columnsNames = (from DataColumn dataColumn in datatable.Columns select dataColumn.ColumnName).ToList();
                temp = datatable.AsEnumerable().ToList().ConvertAll(row => GetObject<T>(row, columnsNames));
                return temp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据DataTable生成对象，对象的属性与列同名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnsName"></param>
        /// <returns></returns>
        public T GetObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            var obj = new T();
            try
            {
                var properties = typeof(T).GetProperties();
                foreach (var objProperty in properties)
                {
                    var attrs = objProperty.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                    if (!attrs.Any()) continue;
                    var displayName = ((DisplayNameAttribute)attrs.First()).DisplayName;

                    var columnname = columnsName.Find(s => s == displayName);
                    if (string.IsNullOrEmpty(columnname)) continue;
                    var value = row[columnname].ToString();
                    if (string.IsNullOrEmpty(value)) continue;
                    try
                    {
                        if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                        {
                            value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                            objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                        }
                        else
                        {
                            value = row[columnname].ToString().Replace("%", "");
                            objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception(displayName + "格式错误");
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion
    }
}
