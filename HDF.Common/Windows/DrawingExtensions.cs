using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDF.Common.Windows
{

    /// <summary>
    /// Windows界面绘制相关拓展
    /// </summary>
    public static class DrawingExtensions
    {

        /// <summary>
        /// 将图片转换成Base64字符串
        /// </summary>
        /// <param name="img">需要转换的图片对象</param>
        /// <returns>转换后的Base64字符串</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToBase64(this Image img)
        {
            if (img == null)
                throw new ArgumentNullException(nameof(img));
            try
            {
                MemoryStream stream = new();
                img.Save(stream, img.RawFormat);
                return Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Base64字符串转换成图片对象
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Image ToImage(this string base64)
        {
            if (base64 == null)
                throw new ArgumentNullException(nameof(base64));
            try
            {
                var bytes = Convert.FromBase64String(base64);
                MemoryStream stream = new(bytes);
                return Image.FromStream(stream);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 检索指定类型的数据
        /// </summary>
        /// <typeparam name="T">数据格式</typeparam>
        /// <param name="data">数据对象</param>
        /// <returns>与指定格式关联的数据</returns>
        public static T GetData<T>(this IDataObject data) => (T)data.GetData(typeof(T));

        /// <summary>
        /// 检索数据对象中是够储存指定类型的数据
        /// </summary>
        /// <typeparam name="T">数据格式</typeparam>
        /// <param name="data">数据对象</param>
        /// <returns>如果是返回true，否则返回false</returns>
        public static bool GetDataPresent<T>(this IDataObject data) => data.GetDataPresent(typeof(T));






        /// <summary>
        /// 获取矩形序列的交集的<see cref="Rectangle"/>结构
        /// </summary>
        /// <param name="source">矩形序列</param>
        /// <returns>包含矩形序列的<see cref="Rectangle"/>结构</returns>
        public static Rectangle Union(this IEnumerable<Rectangle> source)
        {
            if (source == null)
                return Rectangle.Empty;

            var rect = Rectangle.Empty;
            foreach (var item in source)
            {
                rect = Rectangle.Union(rect, item);
            }
            return rect;
        }

        /// <summary>
        /// 获取矩形序列的交集的<see cref="RectangleF"/>结构
        /// </summary>
        /// <param name="source">矩形序列</param>
        /// <returns>包含矩形序列的<see cref="RectangleF"/>结构</returns>
        public static RectangleF Union(this IEnumerable<RectangleF> source)
        {
            if (source == null)
                return RectangleF.Empty;

            var rect = RectangleF.Empty;
            foreach (var item in source)
            {
                rect = RectangleF.Union(rect, item);
            }
            return rect;
        }


        #region Scale


        /// <summary>
        /// 缩放边距对象
        /// </summary>
        /// <param name="m"></param>
        /// <param name="rate"></param>
        public static void Scale(this Margins m, float rate) => m.Scale(rate, rate);

        /// <summary>
        /// 缩放边距对象
        /// </summary>
        /// <param name="m"></param>
        /// <param name="xrate"></param>
        /// <param name="yrate"></param>
        public static void Scale(this Margins m, float xrate, float yrate)
        {
            if (m == null)
                return;
            m.Left = (int)(m.Left * xrate);
            m.Right = (int)(m.Right * xrate);
            m.Top = (int)(m.Top * yrate);
            m.Bottom = (int)(m.Bottom * yrate);
        }


        /// <summary>
        /// 缩放矩形区域（包含坐标）
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static RectangleF Scale(this Rectangle rect, float rate) => Scale(rect, rate, rate);
        /// <summary>
        /// 缩放矩形区域（包含坐标）
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static RectangleF Scale(this RectangleF rect, float rate) => Scale(rect, rate, rate);
        /// <summary>
        /// 缩放大小
        /// </summary>
        /// <param name="s"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static SizeF Scale(this Size s, float rate) => Scale(s, rate, rate);
        /// <summary>
        /// 缩放大小
        /// </summary>
        /// <param name="s"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static SizeF Scale(this SizeF s, float rate) => Scale(s, rate, rate);
        /// <summary>
        /// 缩放坐标
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static PointF Scale(this Point p, float rate) => Scale(p, rate, rate);
        /// <summary>
        /// 缩放坐标
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static PointF Scale(this PointF p, float rate) => Scale(p, rate, rate);
        /// <summary>
        /// 缩放填充对象
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static Padding Scale(this Padding p, float rate) => Scale(p, rate, rate);


        /// <summary>
        /// 缩放矩形区域（包含坐标）
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="xrate"></param>
        /// <param name="yrate"></param>
        /// <returns></returns>
        public static RectangleF Scale(this Rectangle rect, float xrate, float yrate)
        {
            if (rect == Rectangle.Empty)
                return rect;
            return new RectangleF(rect.X * xrate, rect.Y * yrate, rect.Width * xrate, rect.Height * yrate);
        }
        /// <summary>
        /// 缩放矩形区域（包含坐标）
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="xrate"></param>
        /// <param name="yrate"></param>
        /// <returns></returns>
        public static RectangleF Scale(this RectangleF rect, float xrate, float yrate)
        {
            if (rect == RectangleF.Empty)
                return rect;
            return new RectangleF(rect.X * xrate, rect.Y * yrate, rect.Width * xrate, rect.Height * yrate);
        }
        /// <summary>
        /// 缩放大小
        /// </summary>
        /// <param name="s"></param>
        /// <param name="xrate"></param>
        /// <param name="yrate"></param>
        /// <returns></returns>
        public static SizeF Scale(this Size s, float xrate, float yrate)
        {
            if (s == Size.Empty)
                return s;
            return new SizeF(s.Width * xrate, s.Height * yrate);
        }
        /// <summary>
        /// 缩放大小
        /// </summary>
        /// <param name="s"></param>
        /// <param name="xrate"></param>
        /// <param name="yrate"></param>
        /// <returns></returns>
        public static SizeF Scale(this SizeF s, float xrate, float yrate)
        {
            if (s == SizeF.Empty)
                return s;
            return new SizeF(s.Width * xrate, s.Height * yrate);
        }
        /// <summary>
        /// 缩放坐标
        /// </summary>
        /// <param name="p"></param>
        /// <param name="xrate"></param>
        /// <param name="yrate"></param>
        /// <returns></returns>
        public static PointF Scale(this Point p, float xrate, float yrate)
        {
            if (p == Point.Empty)
                return p;
            return new PointF(p.X * xrate, p.Y * yrate);
        }
        /// <summary>
        /// 缩放坐标
        /// </summary>
        /// <param name="p"></param>
        /// <param name="xrate"></param>
        /// <param name="yrate"></param>
        /// <returns></returns>
        public static PointF Scale(this PointF p, float xrate, float yrate)
        {
            if (p == PointF.Empty)
                return p;
            return new PointF(p.X * xrate, p.Y * yrate);
        }
        /// <summary>
        /// 缩放填充对象
        /// </summary>
        /// <param name="p"></param>
        /// <param name="xrate"></param>
        /// <param name="yrate"></param>
        /// <returns></returns>
        public static Padding Scale(this Padding p, float xrate, float yrate)
        {
            if (p == Padding.Empty)
                return p;
            return new Padding((int)(p.Left * xrate), (int)(p.Top * yrate), (int)(p.Right * xrate), (int)(p.Bottom * yrate));
        }

        #endregion Scale




    }
}
