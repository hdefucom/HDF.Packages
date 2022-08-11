using NPOI.SS.Util;

namespace HDF.NPOI.Extension;


/// <summary>
/// Excel合并单元格信息
/// </summary>
public class ExcelMergeCell
{
    /// <summary>
    /// 开始行
    /// </summary>
    public int StartRow { get; }
    /// <summary>
    /// 结束行
    /// </summary>
    public int EndRow { get; }
    /// <summary>
    /// 开始列
    /// </summary>
    public int StartColumn { get; }
    /// <summary>
    /// 结束列
    /// </summary>
    public int EndColumn { get; }

    /// <summary>
    /// 单元格值
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="value"></param>
    /// <param name="startrow"></param>
    /// <param name="endrow"></param>
    /// <param name="startcol"></param>
    /// <param name="endcol"></param>
    public ExcelMergeCell(object value, int startrow, int endrow, int startcol, int endcol)
    {
        Value = value;
        StartRow = startrow;
        EndRow = endrow;
        StartColumn = startcol;
        EndColumn = endcol;
    }

    /// <summary>
    /// 隐式转换成<see cref="CellRangeAddress"/>
    /// </summary>
    /// <param name="a"></param>
    public static implicit operator CellRangeAddress?(ExcelMergeCell? a)
    {
        if (a == null)
            return null;

        return new CellRangeAddress(a.StartRow, a.EndRow, a.StartColumn, a.EndColumn);
    }
}




