using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HDF.NPOI.Extension;

/// <summary>
/// Excel导出类
/// </summary>
public static class ExcelHelper
{

    #region Private Method

    private static CellType GetCellType(Type type)
    {
        if (type == typeof(Nullable<>))
            type = type.GenericTypeArguments[0];

        var t = CellType.String;
        if (type == typeof(int)
            || type == typeof(long)
            || type == typeof(short)
            || type == typeof(uint)
            || type == typeof(ulong)
            || type == typeof(ushort)
            || type == typeof(byte)
            || type == typeof(float)
            || type == typeof(double)
            || type == typeof(decimal)
            || type == typeof(DateTime)//NPOI中时间类型为Numeric
            )
            t = CellType.Numeric;
        else if (type == typeof(bool))
            t = CellType.Boolean;

        return t;
    }

    private static IWorkbook CreateWorkBook(string path)
    {
        if (path.EndsWith(".xlsx")) // 2007
            return new XSSFWorkbook();
        else if (path.EndsWith(".xls")) // 2003
            return new HSSFWorkbook();
        else
            throw new ArgumentException("该Excel文件后缀名不支持", nameof(path));
    }

    private static void SetCellValue(ICell cell, CellType type, object? value, Type? dataType, string datetimeformat)
    {
        if (type == CellType.String)
            cell.SetCellValue(value?.ToString());
        else if (type == CellType.Numeric)
        {
            if (dataType == typeof(DateTime) || dataType == typeof(DateTime?))
            {
                cell.SetCellValue(Convert.ToDateTime(value));

                var format = cell.Row.Sheet.Workbook.CreateDataFormat();
                var style = cell.Row.Sheet.Workbook.CreateCellStyle();
                style.DataFormat = format.GetFormat(datetimeformat);
                cell.CellStyle = style;
            }
            else
                cell.SetCellValue(Convert.ToDouble(value));
        }
        else if (type == CellType.Boolean)
            cell.SetCellValue(Convert.ToBoolean(value));
        else
            cell.SetCellValue(value?.ToString());


    }

    private static void SetMergeCell(List<ExcelMergeCell> mergecell, ISheet sheet, string datetimeformat)
    {
        foreach (var item in mergecell)
        {
            var row = sheet.CreateRow(item.StartRow);

            var type = item.Value == null ? CellType.String : GetCellType(item.Value.GetType());

            var cell = row.CreateCell(item.StartColumn, type);

            SetCellValue(cell, type, item.Value, item.Value?.GetType(), datetimeformat);

            sheet.AddMergedRegion(item);
        }


    }


#pragma warning disable IDE0051 // 删除未使用的私有成员
    private static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> item, out TKey key, out TValue value)
#pragma warning restore IDE0051 // 删除未使用的私有成员
    {
        key = item.Key;
        value = item.Value;
    }


    #endregion


    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data">导出的数据</param>
    /// <param name="path">导出的文件路径</param>
    /// <param name="columns">手动指定的列信息优先级比类属性标记特性高，一个列只能指定一个对象，指定多个对象只有首个生效</param>
    /// <param name="noSpecifyColIsExport">未指定的列是否导出，默认为true，未指定的意思是既没有标记特性，有没有传递ExcelColumnInfo对象</param>
    /// <param name="datetimeformat">DateTime导出的格式</param>
    /// <param name="mergecell">合并单元格</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void ExportExcel<T>(this IEnumerable<T> data, string path,
        List<ExcelColumnInfo>? columns = null,
        bool noSpecifyColIsExport = true,
        string datetimeformat = "yyyy-MM-dd HH:mm:ss",
        List<ExcelMergeCell>? mergecell = null)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (string.IsNullOrEmpty(path))
            throw new ArgumentNullException(nameof(path));

        IWorkbook workbook = CreateWorkBook(path);


        var sheet = workbook.CreateSheet();


        var dict = typeof(T).GetProperties()
            .ToDictionary(p => p, p => (
                Info: ExcelColumnInfo.Create(p.Name, p.GetCustomAttribute<ExcelColumnAttribute>(), noSpecifyColIsExport),
                Type: GetCellType(p.PropertyType)
            ));

        //初始化列信息
        {
            if (columns != null && columns.Count > 0)//手动指定的列信息优先级比类属性标记特性高
            {
                for (int i = 0; i < dict.Keys.Count; i++)
                {
                    var item = dict.Keys.ElementAt(i);
                    var info = columns.FirstOrDefault(c => c.Key == item.Name);
                    if (info != null)
                        dict[item] = (info, dict[item].Type);

                }
            }

            dict = dict
                //过滤不导出的列
                .Where(a => a.Value.Info.Export)
                .OrderBy(a => a.Value.Info.Order)
                .ToDictionary(a => a.Key, a => a.Value);
        }


        //创建表头
        {
            var header = sheet.CreateRow(0);
            var i = 0;
            foreach (var (prop, (info, type)) in dict)
            {

                var cell = header.CreateCell(i, CellType.String);
                cell.SetCellValue(info.Title);

                if (info.Width != -1)
                    sheet.SetColumnWidth(i, info.Width);
                else
                    sheet.AutoSizeColumn(i);//未指定宽度则自适应

                i++;
            }

        }

        //填充数据
        {

            var rowindex = 1;

            foreach (var item in data)
            {
                var row = sheet.CreateRow(rowindex);

                var colindex = 0;
                foreach (var (prop, (info, type)) in dict)
                {
                    var cell = row.CreateCell(colindex, type);

                    SetCellValue(cell, type, prop.GetValue(item), prop.PropertyType, datetimeformat);

                    colindex++;
                }

                rowindex++;
            }

            //调整列宽，datetime自适应宽度
            for (int i = 0; i < dict.Keys.Count; i++)
            {
                var p = dict.Keys.ElementAt(i);

                if ((p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
                    && dict[p].Info.Width != -1)//未指定宽度则自适应
                    sheet.AutoSizeColumn(i);
            }

        }

        //设置合并单元格
        if (mergecell != null && mergecell.Count > 0)
            SetMergeCell(mergecell, sheet, datetimeformat);



        //如果写入一个已经存在的excel文件，使用filestream会导致文件尾部有一串无意义的00数据，导致excel数据错误无法打开
        if (File.Exists(path))
            File.Delete(path);

        //保存文件
        using var file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);

        workbook.Write(file);




    }




    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="data">导出的数据</param>
    /// <param name="path">导出的文件路径</param>
    /// <param name="columns">手动指定的列信息优先级比类属性标记特性高，一个列只能指定一个对象，指定多个对象只有首个生效</param>
    /// <param name="noSpecifyColIsExport">未指定的列是否导出，默认为true，未指定的意思是既没有标记特性，有没有传递ExcelColumnInfo对象</param>
    /// <param name="datetimeformat">DateTime导出的格式</param>
    /// <param name="mergecell">合并单元格</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void ExportExcel(this DataTable data, string path,
        List<ExcelColumnInfo>? columns = null,
        bool noSpecifyColIsExport = true,
        string datetimeformat = "yyyy-MM-dd HH:mm:ss",
        List<ExcelMergeCell>? mergecell = null)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (string.IsNullOrEmpty(path))
            throw new ArgumentNullException(nameof(path));

        IWorkbook workbook = CreateWorkBook(path);

        var sheet = workbook.CreateSheet();

        var dict = data.Columns.Cast<DataColumn>()
            .ToDictionary(p => p, p => (
                Info: new ExcelColumnInfo
                {
                    Key = p.ColumnName,
                    Title = p.ColumnName,
                    Export = noSpecifyColIsExport,
                },
                Type: GetCellType(p.DataType)
            ));

        //初始化列信息
        {

            if (columns != null && columns.Count > 0)//手动指定的列信息优先级比类属性标记特性高
            {
                for (int i = 0; i < dict.Keys.Count; i++)
                {
                    var item = dict.Keys.ElementAt(i);
                    var info = columns.FirstOrDefault(c => c.Key == item.ColumnName);
                    if (info != null)
                        dict[item] = (info, dict[item].Type);

                }
            }

            dict = dict
                //过滤不导出的列
                .Where(a => a.Value.Info.Export)
                .OrderBy(a => a.Value.Info.Order)
                .ToDictionary(a => a.Key, a => a.Value);

        }

        //创建表头
        {
            var header = sheet.CreateRow(0);
            var i = 0;
            foreach (var (prop, (info, type)) in dict)
            {

                var cell = header.CreateCell(i, CellType.String);
                cell.SetCellValue(info.Title);

                if (info.Width != -1)
                    sheet.SetColumnWidth(i, info.Width);
                else
                    sheet.AutoSizeColumn(i);//未指定宽度则自适应

                i++;
            }

        }

        //填充数据
        {

            var rowindex = 1;

            foreach (DataRow item in data.Rows)
            {
                var row = sheet.CreateRow(rowindex);

                var colindex = 0;
                foreach (var (col, (info, type)) in dict)
                {
                    var cell = row.CreateCell(colindex, type);

                    SetCellValue(cell, type, item[col], col.DataType, datetimeformat);

                    colindex++;
                }

                rowindex++;
            }

            //调整列宽，datetime自适应宽度
            for (int i = 0; i < dict.Keys.Count; i++)
            {
                var p = dict.Keys.ElementAt(i);

                if ((p.DataType == typeof(DateTime) || p.DataType == typeof(DateTime?))
                    && dict[p].Info.Width != -1)//未指定宽度则自适应
                    sheet.AutoSizeColumn(i);
            }

        }


        //设置合并单元格
        if (mergecell != null && mergecell.Count > 0)
            SetMergeCell(mergecell, sheet, datetimeformat);


        //如果写入一个已经存在的excel文件，使用filestream会导致文件尾部有一串无意义的00数据，导致excel数据错误无法打开
        if (File.Exists(path))
            File.Delete(path);

        //保存文件
        using var file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);

        workbook.Write(file);





    }


}



