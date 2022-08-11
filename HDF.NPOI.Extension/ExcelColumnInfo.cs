using System;

namespace HDF.NPOI.Extension;

/// <summary>
/// Excel列信息
/// </summary>
public class ExcelColumnInfo
{
    /// <summary>
    /// 绑定的列名
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// 表头
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 列宽，默认-1，自适应宽度
    /// </summary>
    /// <remarks>
    /// the width in units of 1/256th of a character width
    /// </remarks>
    public int Width { get; set; } = -1;

    /// <summary>
    /// 排序号，默认9999
    /// </summary>
    public int Order { get; set; } = 9999;

    /// <summary>
    /// 是否导出
    /// </summary>
    public bool Export { get; set; } = true;


    /// <summary>
    /// 根据ExcelColumnAttribute创建ExcelColumnInfo对象
    /// </summary>
    /// <param name="key">列名</param>
    /// <param name="attr"></param>
    /// <param name="export"><paramref name="attr"/>为null时指定是否导出</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ExcelColumnInfo Create(string key, ExcelColumnAttribute attr, bool export = true)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("无效的Key", nameof(key));

        if (attr == null)
            return new ExcelColumnInfo
            {
                Key = key,
                Title = key,
                Export = export,
            };

        return new ExcelColumnInfo
        {
            Key = key,
            Title = attr.Title,
            Width = attr.Width,
            Order = attr.Order,
            Export = attr.Export,
        };
    }
}
