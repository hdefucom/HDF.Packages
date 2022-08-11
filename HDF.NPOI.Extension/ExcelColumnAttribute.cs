using System;

namespace HDF.NPOI.Extension;

/// <summary>
/// Excel列信息特性
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ExcelColumnAttribute : Attribute
{
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

}
