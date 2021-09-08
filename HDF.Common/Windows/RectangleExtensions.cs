using System;
using System.Drawing;

namespace HDF.Common.Windows
{
    /// <summary>
    /// 相对矩形区域的位置枚举
    /// </summary>
    public enum RectangleArgePosition
    {
        /// <summary>
        /// 在矩形中
        /// </summary>
        Inner,
        /// <summary>
        /// 在矩形左上范围
        /// </summary>
        TopLeft,
        /// <summary>
        /// 在矩形正上范围
        /// </summary>
        Top,
        /// <summary>
        /// 在矩形右上范围
        /// </summary>
        TopRight,
        /// <summary>
        /// 在矩形正右范围
        /// </summary>
        Right,
        /// <summary>
        /// 在矩形右下范围
        /// </summary>
        BottomRight,
        /// <summary>
        /// 在矩形正下范围
        /// </summary>
        Bottom,
        /// <summary>
        /// 在矩形左下范围
        /// </summary>
        BottomLeft,
        /// <summary>
        /// 在矩形正左范围
        /// </summary>
        Left,
    }

    /// <summary>
    /// 矩形拓展
    /// </summary>
    public static class RectangleExtensions
    {


        /// <summary>
        /// 获得指定坐标在指定矩形相对的区域编号
        /// </summary>
        /// <remarks>
        ///         1  |   2   |  3         <br/>
        ///       -----+=======+-------     <br/>
        ///            ||     ||            <br/>
        ///         8  ||  0  ||  4         <br/>
        ///            ||     ||            <br/>
        ///       -----+=======+-------     <br/>
        ///         7  |   6   |  5         <br/>
        /// </remarks>
        /// <param name="rect"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectangleArgePosition GetRectangleArea(this RectangleF rect, float x, float y)
        {
            if (rect.Contains(x, y))
                return RectangleArgePosition.Inner;

            if (y < rect.Top)
            {
                if (x < rect.Left)
                    return RectangleArgePosition.TopLeft;
                if (x < rect.Right)
                    return RectangleArgePosition.Top;
                return RectangleArgePosition.TopRight;
            }
            if (y < rect.Bottom)
            {
                if (x < rect.Left)
                    return RectangleArgePosition.Left;
                if (x < rect.Right)
                    return RectangleArgePosition.Inner;
                return RectangleArgePosition.Right;
            }
            if (x < rect.Left)
                return RectangleArgePosition.BottomLeft;
            if (x < rect.Right)
                return RectangleArgePosition.Bottom;
            return RectangleArgePosition.BottomRight;
        }


        /// <summary>
        /// 获得指定坐标相对于指定矩形的距离
        /// </summary>
        /// <param name="rect">矩形边框</param>
        /// <param name="x">指定点的X坐标</param>
        /// <param name="y">指定点的Y坐标</param>
        /// <returns>距离，若小于0则点被包含在矩形区域中</returns>
        public static double GetDistance(this RectangleF rect, float x, float y) => rect.GetRectangleArea(x, y) switch
        {
            RectangleArgePosition.Inner => -1,
            RectangleArgePosition.TopLeft => Math.Sqrt((rect.X - x) * (rect.X - x) + (rect.Y - y) * (rect.Y - y)),
            RectangleArgePosition.Top => rect.Y - y,
            RectangleArgePosition.TopRight => Math.Sqrt((x - rect.Right) * (x - rect.Right) + (rect.Y - y) * (rect.Y - y)),
            RectangleArgePosition.Right => x - rect.Right,
            RectangleArgePosition.BottomRight => Math.Sqrt((x - rect.Right) * (x - rect.Right) + (y - rect.Bottom) * (y - rect.Bottom)),
            RectangleArgePosition.Bottom => y - rect.Bottom,
            RectangleArgePosition.BottomLeft => Math.Sqrt((x - rect.X) * (x - rect.X) + (y - rect.Bottom) * (y - rect.Bottom)),
            RectangleArgePosition.Left => rect.X - x,
            _ => throw new NotImplementedException($"{nameof(GetRectangleArea)}未实现"),
        };



        /// <summary>
        /// 将坐标移动到指定区域内
        /// </summary>
        /// <param name="rect">指定区域</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point MoveToArea(this Rectangle rect, int x, int y)
        {
            if (x < rect.X)
                x = rect.X;
            else if (x > rect.Right)
                x = rect.Right;
            if (y < rect.Y)
                y = rect.Y;
            else if (y > rect.Bottom)
                y = rect.Bottom;
            return new Point(x, y);
        }
        /// <summary>
        /// 将坐标移动到指定区域内
        /// </summary>
        /// <param name="rect">指定区域</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static PointF MoveToArea(this RectangleF rect, float x, float y)
        {
            if (x < rect.X)
                x = rect.X;
            else if (x > rect.Right)
                x = rect.Right;
            if (y < rect.Y)
                y = rect.Y;
            else if (y > rect.Bottom)
                y = rect.Bottom;
            return new PointF(x, y);
        }

        /// <summary>
        /// 将坐标移动到指定区域内
        /// </summary>
        /// <param name="rect">指定区域</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point MoveToArea(this RectangleF rect, int x, int y)
        {
            if (x < rect.X)
                x = (int)Math.Ceiling(rect.X);
            else if (x >= rect.Right)
                x = (int)rect.Right - 1;
            if (y < rect.Y)
                y = (int)Math.Ceiling(rect.Y);
            else if (y >= rect.Bottom)
                y = (int)rect.Bottom - 1;
            return new Point(x, y);
        }












    }
}
