using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace HDF.Common.Windows
{
    /// <summary>
    /// GDI+绘制单位转换拓展
    /// </summary>
    public static class GraphicsUnitExtensions
    {
        /// <summary>
        /// 水平DPI
        /// </summary>
        public static float DPIX { get; set; }

        /// <summary>
        /// 垂直DPI
        /// </summary>
        [Obsolete("暂不使用，后续完善再修改")]
        public static float DPIY { get; set; }

        static GraphicsUnitExtensions()
        {
#pragma warning disable 0618
            DPIX = 96f;
            DPIY = 96f;

            using Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            DPIX = graphics.DpiX;
            DPIY = graphics.DpiY;
#pragma warning restore 0618

        }



        /// <summary>
        /// 获取一英寸占据的单位数
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>单位数</returns>
        public static float GetDpi(GraphicsUnit unit) => unit switch
        {
            GraphicsUnit.Display => DPIX,
            GraphicsUnit.Pixel => DPIX,
            GraphicsUnit.Point => 72,
            GraphicsUnit.Inch => 1,
            GraphicsUnit.Document => 300,
            GraphicsUnit.Millimeter => 25.4f,
            _ => DPIX,
        };


        /// <summary>
        /// 获取一单位占据的英寸数
        /// </summary>
        /// <param name="unit">单位类型</param>
        /// <remarks>优化单位转换精度，建议使用<see cref="GetDpi(GraphicsUnit)"/></remarks>
        /// <returns>英寸数</returns>
        [Obsolete("优化单位转换精度，建议使用GetDpi(GraphicsUnit)")]
        public static float GetUnit(GraphicsUnit unit) => unit switch
        {
            GraphicsUnit.Display => 1 / DPIX,
            GraphicsUnit.Pixel => 1 / DPIX,
            GraphicsUnit.Point => 1 / 72f,
            GraphicsUnit.Inch => 1,
            GraphicsUnit.Document => 1 / 300f,
            GraphicsUnit.Millimeter => 1 / 25.4f,
            _ => throw new NotSupportedException($"Not support '{unit}'"),
        };


        /// <summary>
        /// 将一个长度从旧单位换算成新单位使用的比率
        /// </summary>
        /// <param name="newUnit">新单位</param>
        /// <param name="oldUnit">旧单位</param>
        /// <returns>比率数</returns>
        public static double GetRate(this GraphicsUnit newUnit, GraphicsUnit oldUnit) => GetDpi(newUnit) / GetDpi(oldUnit);



        #region No Check Equals Transform

        private static int Transform(this int val, GraphicsUnit oldUnit, GraphicsUnit newUnit) => (int)(val * GetDpi(newUnit) / GetDpi(oldUnit));
        private static float Transform(this float val, GraphicsUnit oldUnit, GraphicsUnit newUnit) => (float)(val * GetDpi(newUnit) / GetDpi(oldUnit));

        #endregion

        #region GraphicsUnit Convert Method


        /// <summary>
        /// 将目标值从旧单位转换成新单位
        /// </summary>
        /// <param name="val">目标值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <param name="newUnit">新单位</param>
        /// <returns>新单位的值</returns>
        public static int Convert(this int val, GraphicsUnit oldUnit, GraphicsUnit newUnit) => oldUnit == newUnit ? val : val.Transform(oldUnit, newUnit);

        /// <summary>
        /// 将目标值从旧单位转换成新单位
        /// </summary>
        /// <param name="val">目标值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <param name="newUnit">新单位</param>
        /// <returns>新单位的值</returns>
        public static float Convert(this float val, GraphicsUnit oldUnit, GraphicsUnit newUnit) => oldUnit == newUnit ? val : val.Transform(oldUnit, newUnit);

        /// <summary>
        /// 将目标值从旧单位转换成新单位
        /// </summary>
        /// <param name="p">目标值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <param name="newUnit">新单位</param>
        /// <returns>新单位的值</returns>
        public static Point Convert(this Point p, GraphicsUnit oldUnit, GraphicsUnit newUnit) => oldUnit == newUnit ? p : new Point(p.X.Transform(oldUnit, newUnit), p.Y.Transform(oldUnit, newUnit));

        /// <summary>
        /// 将目标值从旧单位转换成新单位
        /// </summary>
        /// <param name="p">目标值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <param name="newUnit">新单位</param>
        /// <returns>新单位的值</returns>
        public static PointF Convert(this PointF p, GraphicsUnit oldUnit, GraphicsUnit newUnit) => oldUnit == newUnit ? p : new PointF(p.X.Transform(oldUnit, newUnit), p.Y.Transform(oldUnit, newUnit));

        /// <summary>
        /// 将目标值从旧单位转换成新单位
        /// </summary>
        /// <param name="size">目标值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <param name="newUnit">新单位</param>
        /// <returns>新单位的值</returns>
        public static Size Convert(this Size size, GraphicsUnit oldUnit, GraphicsUnit newUnit) => oldUnit == newUnit ? size : new Size(size.Width.Transform(oldUnit, newUnit), size.Height.Transform(oldUnit, newUnit));

        /// <summary>
        /// 将目标值从旧单位转换成新单位
        /// </summary>
        /// <param name="size">目标值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <param name="newUnit">新单位</param>
        /// <returns>新单位的值</returns>
        public static SizeF Convert(this SizeF size, GraphicsUnit oldUnit, GraphicsUnit newUnit) => oldUnit == newUnit ? size : new SizeF(size.Width.Transform(oldUnit, newUnit), size.Height.Transform(oldUnit, newUnit));

        /// <summary>
        /// 将目标值从旧单位转换成新单位
        /// </summary>
        /// <param name="rect">目标值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <param name="newUnit">新单位</param>
        /// <returns>新单位的值</returns>
        public static Rectangle Convert(this Rectangle rect, GraphicsUnit oldUnit, GraphicsUnit newUnit) => oldUnit == newUnit ? rect : new Rectangle(rect.X.Transform(oldUnit, newUnit), rect.Y.Transform(oldUnit, newUnit), rect.Width.Transform(oldUnit, newUnit), rect.Height.Transform(oldUnit, newUnit));


        /// <summary>
        /// 将目标值从旧单位转换成新单位
        /// </summary>
        /// <param name="rect">目标值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <param name="newUnit">新单位</param>
        /// <returns>新单位的值</returns>
        public static RectangleF Convert(this RectangleF rect, GraphicsUnit oldUnit, GraphicsUnit newUnit) => oldUnit == newUnit ? rect : new RectangleF(rect.X.Transform(oldUnit, newUnit), rect.Y.Transform(oldUnit, newUnit), rect.Width.Transform(oldUnit, newUnit), rect.Height.Transform(oldUnit, newUnit));

        /// <summary>
        /// 将目标值从旧单位转换成新单位
        /// </summary>
        /// <param name="padding">目标值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <param name="newUnit">新单位</param>
        /// <returns>新单位的值</returns>
        public static Padding Convert(this Padding padding, GraphicsUnit oldUnit, GraphicsUnit newUnit) => oldUnit == newUnit ? padding : new Padding(padding.Left.Transform(oldUnit, newUnit), padding.Top.Transform(oldUnit, newUnit), padding.Right.Transform(oldUnit, newUnit), padding.Bottom.Transform(oldUnit, newUnit));


        #endregion

        #region Target Convert Method


        /// <summary>
        /// 将百分之一英寸为单位的值转换成目标单位值
        /// </summary>
        /// <param name="margins">值</param>
        /// <param name="newUnit">目标单位</param>
        /// <returns>目标单位值</returns>
        public static Margins ConvertMargins(this Margins margins, GraphicsUnit newUnit) => new((int)(margins.Left * GetDpi(newUnit) / 100), (int)(margins.Right * GetDpi(newUnit) / 100), (int)(margins.Top * GetDpi(newUnit) / 100), (int)(margins.Bottom * GetDpi(newUnit) / 100));

        /// <summary>
        /// 将百分之一英寸为单位的值转换成目标单位值
        /// </summary>
        /// <param name="val">值</param>
        /// <param name="newUnit">目标单位</param>
        /// <returns>目标单位值</returns>
        public static float ConvertMargins(this int val, GraphicsUnit newUnit) => (float)(val * GetDpi(newUnit) / 100);

        /// <summary>
        /// 将旧单位值转换成百分之一英寸为单位的值
        /// </summary>
        /// <param name="val">值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <returns>百分之一英寸为单位的值</returns>
        public static int ConvertToMargins(this float val, GraphicsUnit oldUnit) => (int)(val * 100 / GetDpi(oldUnit));

        /// <summary>
        /// 将缇(Twips)为单位的值转换成目标单位值
        /// </summary>
        /// <param name="val">值</param>
        /// <param name="newUnit">目标单位</param>
        /// <returns>目标单位值</returns>
        public static int ConvertTwips(this int val, GraphicsUnit newUnit) => (int)(val * GetDpi(newUnit) / 1440);

        /// <summary>
        /// 将旧单位值转换成缇(Twips)为单位的值
        /// </summary>
        /// <param name="val">值</param>
        /// <param name="oldUnit">旧单位</param>
        /// <returns>缇(Twips)为单位的值</returns>
        public static int ConvertToTwips(this int val, GraphicsUnit oldUnit) => (int)(val * 1440 / GetDpi(oldUnit));


        #endregion


    }


}