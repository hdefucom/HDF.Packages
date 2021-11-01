using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace HDF.Common.Windows;

/// <summary>
/// 解构拓展
/// </summary>
public static class DeconstructExtensions
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public static void Deconstruct(this Rectangle rect, out int x, out int y, out int width, out int height)
    {
        x = rect.X;
        y = rect.Y;
        width = rect.Width;
        height = rect.Height;
    }
    public static void Deconstruct(this Rectangle rect, out int x, out int y)
    {
        x = rect.X;
        y = rect.Y;
    }
    public static void Deconstruct(this Rectangle rect, out Point point, out Size size)
    {
        point = rect.Location;
        size = rect.Size;
    }

    public static void Deconstruct(this RectangleF rect, out float x, out float y, out float width, out float height)
    {
        x = rect.X;
        y = rect.Y;
        width = rect.Width;
        height = rect.Height;
    }
    public static void Deconstruct(this RectangleF rect, out float x, out float y)
    {
        x = rect.X;
        y = rect.Y;
    }
    public static void Deconstruct(this RectangleF rect, out PointF point, out SizeF size)
    {
        point = rect.Location;
        size = rect.Size;
    }




    public static void Deconstruct(this Size s, out int width, out int height)
    {
        width = s.Width;
        height = s.Height;
    }
    public static void Deconstruct(this SizeF s, out float width, out float height)
    {
        width = s.Width;
        height = s.Height;
    }


    public static void Deconstruct(this Point p, out int x, out int y)
    {
        x = p.X;
        y = p.Y;
    }
    public static void Deconstruct(this PointF p, out float x, out float y)
    {
        x = p.X;
        y = p.Y;
    }



    public static void Deconstruct(this Padding p, out int left, out int top, out int right, out int bottom)
    {
        left = p.Left;
        top = p.Top;
        right = p.Right;
        bottom = p.Bottom;
    }

    public static void Deconstruct(this Padding p, out int horizontal, out int vertical)
    {
        horizontal = p.Horizontal;
        vertical = p.Vertical;
    }


    public static void Deconstruct(this Margins m, out int left, out int top, out int right, out int bottom)
    {
        left = m.Left;
        top = m.Top;
        right = m.Right;
        bottom = m.Bottom;
    }




}

