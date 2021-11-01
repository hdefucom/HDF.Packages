using System.Drawing;

namespace HDF.Common.Windows;

/// <summary>
/// 字体拓展
/// </summary>
public static class FontExtensions
{
    /// <summary>
    /// 返回该Font的FontFamily的单元格上升，采用设计单位。
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static int GetAscent(this Font f) => f.FontFamily.GetCellAscent(f.Style);
    /// <summary>
    /// 返回该Font的FontFamily的单元格下降，采用设计单位。
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static int GetDescent(this Font f) => f.FontFamily.GetCellDescent(f.Style);
    /// <summary>
    /// 返回该Font的FontFamily的em方形的高度，采用字体设计单位。
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static int GetEmHeight(this Font f) => f.FontFamily.GetEmHeight(f.Style);
    /// <summary>
    /// 返回该Font的FontFamily的行距，采用设计单位。行距是两个连续文本行的基线之间的垂直距离。
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static int GetLineSpacing(this Font f) => f.FontFamily.GetLineSpacing(f.Style);



}

