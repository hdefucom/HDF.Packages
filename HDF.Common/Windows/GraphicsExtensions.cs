using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace HDF.Common.Windows
{
    /// <summary>
    /// GDI+绘制拓展
    /// </summary>
    public static class GraphicsExtensions
    {
        #region RadiusRectangle


        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void FillRadiusRectangle(this Graphics g, Brush b, Rectangle rect, int radius) => g.FillRadiusRectangle(b, rect.X, rect.Y, rect.Width, rect.Height, radius);

        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="radius"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void FillRadiusRectangle(this Graphics g, Brush b, int x, int y, int width, int height, int radius)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (b == null)
                throw new ArgumentNullException(nameof(b));
            if (radius < 0)
                throw new ArgumentOutOfRangeException(nameof(radius));

            if (radius == 0)
            {
                g.FillRectangle(b, x, y, width, height);
                return;
            }

            using GraphicsPath path = new();
            var l = radius * 2;
            path.AddArc(x, y, l, l, 180, 90);
            path.AddArc(x + width - l, y, l, l, 270, 90);
            path.AddArc(x + width - l, y + height - l, l, l, 0, 90);
            path.AddArc(x, y + height - l, l, l, 90, 90);
            path.CloseFigure();
            g.FillPath(b, path);
        }

        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void FillRadiusRectangle(this Graphics g, Brush b, RectangleF rect, int radius) => g.FillRadiusRectangle(b, rect.X, rect.Y, rect.Width, rect.Height, radius);

        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="radius"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void FillRadiusRectangle(this Graphics g, Brush b, float x, float y, float width, float height, float radius)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (b == null)
                throw new ArgumentNullException(nameof(b));
            if (radius < 0)
                throw new ArgumentOutOfRangeException(nameof(radius));

            if (radius == 0)
            {
                g.FillRectangle(b, x, y, width, height);
                return;
            }

            using GraphicsPath path = new();
            var l = radius * 2;
            path.AddArc(x, y, l, l, 180, 90);
            path.AddArc(x + width - l, y, l, l, 270, 90);
            path.AddArc(x + width - l, y + height - l, l, l, 0, 90);
            path.AddArc(x, y + height - l, l, l, 90, 90);
            path.CloseFigure();
            g.FillPath(b, path);
        }






        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p"></param>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void DrawRadiusRectangle(this Graphics g, Pen p, Rectangle rect, int radius) => g.DrawRadiusRectangle(p, rect.X, rect.Y, rect.Width, rect.Height, radius);

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="radius"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void DrawRadiusRectangle(this Graphics g, Pen p, int x, int y, int width, int height, int radius)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (p == null)
                throw new ArgumentNullException(nameof(p));
            if (radius < 0)
                throw new ArgumentOutOfRangeException(nameof(radius));

            if (radius == 0)
            {
                g.DrawRectangle(p, x, y, width, height);
                return;
            }

            using GraphicsPath path = new();
            var l = radius * 2;
            path.AddArc(x, y, l, l, 180, 90);
            path.AddArc(x + width - l, y, l, l, 270, 90);
            path.AddArc(x + width - l, y + height - l, l, l, 0, 90);
            path.AddArc(x, y + height - l, l, l, 90, 90);
            path.CloseFigure();
            g.DrawPath(p, path);
        }


        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p"></param>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void DrawRadiusRectangle(this Graphics g, Pen p, RectangleF rect, int radius) => g.DrawRadiusRectangle(p, rect.X, rect.Y, rect.Width, rect.Height, radius);

        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="radius"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static void DrawRadiusRectangle(this Graphics g, Pen p, float x, float y, float width, float height, float radius)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (p == null)
                throw new ArgumentNullException(nameof(p));
            if (radius < 0)
                throw new ArgumentOutOfRangeException(nameof(radius));

            if (radius == 0)
            {
                g.DrawRectangle(p, x, y, width, height);
                return;
            }

            using GraphicsPath path = new();
            var l = radius * 2;
            path.AddArc(x, y, l, l, 180, 90);
            path.AddArc(x + width - l, y, l, l, 270, 90);
            path.AddArc(x + width - l, y + height - l, l, l, 0, 90);
            path.AddArc(x, y + height - l, l, l, 90, 90);
            path.CloseFigure();
            g.DrawPath(p, path);
        }



        #endregion












    }


}