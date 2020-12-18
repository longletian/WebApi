using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace WebApi.Tools.UploadFile
{
   public  class UplodFileRepository
    {

        public static MemoryStream ExportExcel<T>(List<T> entities, Dictionary<string, string> dicColumns, string exportFileName = null, string title = null)
        {
            //当数据为空时
            if (entities.Count <= 0)
            {
                return null;
            }
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(exportFileName);
            MemoryStream ms = null;
            //获取实体属性名
            PropertyInfo[] properties = entities[0].GetType().GetProperties();
            int cellsIndex = 0;
            //标题
            if (!string.IsNullOrEmpty(title))
            {
                #region 样式设计
                ICellStyle style = workbook.CreateCellStyle();
                //边框  
                style.BorderBottom = BorderStyle.Dotted;
                style.BorderLeft = BorderStyle.Hair;
                style.BorderRight = BorderStyle.Hair;
                style.BorderTop = BorderStyle.Dotted;
                //水平对齐  
                style.Alignment = HorizontalAlignment.Left;

                //垂直对齐  
                style.VerticalAlignment = VerticalAlignment.Center;

                //设置字体
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "微软雅黑";
                style.SetFont(font);
                #endregion

                IRow cellsTitle = sheet.CreateRow(0);
                cellsTitle.CreateCell(0).SetCellValue(title);
                cellsTitle.RowStyle = style;
                //合并单元格
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, dicColumns.Count - 1));
                cellsIndex = 2;
            }
            //列名
            IRow cellsColumn = sheet.CreateRow(cellsIndex);
            int index = 0;
            Dictionary<string, int> columns = new Dictionary<string, int>();
            foreach (var item in dicColumns)
            {
                cellsColumn.CreateCell(index).SetCellValue(item.Value);
                columns.Add(item.Value, index);
                index++;
            }
            cellsIndex += 1;
            //数据
            foreach (var item in entities)
            {
                IRow cellsData = sheet.CreateRow(cellsIndex);
                for (int i = 0; i < properties.Length; i++)
                {
                    if (!dicColumns.ContainsKey(properties[i].Name)) continue;
                    //这里可以也根据数据类型做不同的赋值，也可以根据不同的格式参考上面的ICellStyle设置不同的样式
                    object[] entityValues = new object[properties.Length];
                    entityValues[i] = properties[i].GetValue(item);
                    //获取对应列下标
                    index = columns[dicColumns[properties[i].Name]];
                    cellsData.CreateCell(index).SetCellValue(entityValues[i].ToString());
                }
                cellsIndex++;
            }
            using (ms = new MemoryStream())
            {
                workbook.Write(ms);
                //ms.Flush();
                //ms.Close();
            }
            return ms;
        }
        /// <summary>
        /// 导入文件读取成数据集
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataSet ReadExcelToDataSet(string filePath, string sheetName = null)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);
                //获取sheet信息
                ISheet sheet = null;
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        return null;
                    }
                    DataTable dt = ReadExcelFunc(workbook, sheet);
                    ds.Tables.Add(dt);
                }
                else
                {
                    //遍历获取所有数据
                    int sheetCount = workbook.NumberOfSheets;
                    for (int i = 0; i < sheetCount; i++)
                    {
                        sheet = workbook.GetSheetAt(i);
                        if (sheet != null)
                        {
                            DataTable dt = ReadExcelFunc(workbook, sheet);
                            ds.Tables.Add(dt);
                        }
                    }
                }
                return ds;
            }
        }
     
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="uploadPath"></param>
        /// <returns></returns>
        public static bool UploadFile(string filePath, string uploadPath)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 导入文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<T> InputExcel<T>(IFormFile file) where T : class, new()
        {
            List<T> list = new List<T> { };
            IWorkbook workbook = null;
            using MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            workbook = new XSSFWorkbook(ms);
            #region MyRegion
            //var filetype = Path.GetExtension(file.FileName).ToLower();
            //if (filetype == ".xlsx")
            //{
            //    workbook = new XSSFWorkbook(ms);
            //}
            //else if (filetype == ".xls")
            //{
            //    workbook = new HSSFWorkbook(ms);
            //}
            //else
            //{
            //    workbook = null;
            //}
            #endregion
            ISheet sheet = workbook.GetSheetAt(0);
            IRow cellNum = sheet.GetRow(0);
            //获取最后一行行数
            int num = cellNum.LastCellNum;
            //反射实体
            var propertys = typeof(T).GetProperties();
            var obj = new T();
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                for (int j = row.FirstCellNum; j < num; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        var value = row.GetCell(j).ToString();
                        string str = (propertys[j].PropertyType).FullName;
                        if (str == "System.String")
                        {
                            propertys[j].SetValue(obj, value, null);
                        }
                        else if (str == "System.DateTime")
                        {
                            DateTime pdt = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                            propertys[j].SetValue(obj, pdt, null);
                        }
                        else if (str == "System.Boolean")
                        {
                            bool pb = Convert.ToBoolean(value);
                            propertys[j].SetValue(obj, pb, null);
                        }
                        else if (str == "System.Int32")
                        {
                            int pi32 = Convert.ToInt32(value);
                            propertys[j].SetValue(obj, pi32, null);
                        }
                        else if (str == "System.Byte")
                        {
                            byte pb = Convert.ToByte(value);
                            propertys[j].SetValue(obj, pb, null);
                        }
                        else
                        {
                            propertys[j].SetValue(obj, null, null);
                        }
                    }
                }
                list.Add(obj);
            }

            return list;
        }
        private static DataTable ReadExcelFunc(IWorkbook workbook, ISheet sheet)
        {
            DataTable dt = new DataTable();
            //获取列信息
            IRow cells = sheet.GetRow(sheet.FirstRowNum);
            int cellsCount = cells.PhysicalNumberOfCells;
            int emptyCount = 0;
            int cellIndex = sheet.FirstRowNum;
            List<string> listColumns = new List<string>();
            bool isFindColumn = false;
            while (!isFindColumn)
            {
                emptyCount = 0;
                listColumns.Clear();
                for (int i = 0; i < cellsCount; i++)
                {
                    if (string.IsNullOrEmpty(cells.GetCell(i).StringCellValue))
                    {
                        emptyCount++;
                    }
                    listColumns.Add(cells.GetCell(i).StringCellValue);
                }
                //这里根据逻辑需要，空列超过多少判断
                if (emptyCount == 0)
                {
                    isFindColumn = true;
                }
                cellIndex++;
                cells = sheet.GetRow(cellIndex);
            }

            foreach (string columnName in listColumns)
            {
                if (dt.Columns.Contains(columnName))
                {
                    continue;
                }
                dt.Columns.Add(columnName, typeof(string));
            }
            //开始获取数据
            int rowsCount = sheet.PhysicalNumberOfRows;
            cellIndex += 1;
            DataRow dr = null;
            for (int i = cellIndex; i < rowsCount; i++)
            {
                cells = sheet.GetRow(i);
                dr = dt.NewRow();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //这里可以判断数据类型
                    switch (cells.GetCell(j).CellType)
                    {
                        case CellType.String:
                            dr[j] = cells.GetCell(j).StringCellValue;
                            break;
                        case CellType.Numeric:
                            dr[j] = cells.GetCell(j).NumericCellValue.ToString();
                            break;
                        case CellType.Unknown:
                            dr[j] = cells.GetCell(j).StringCellValue;
                            break;
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
