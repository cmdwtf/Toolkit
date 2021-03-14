using System;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Native methods, constants, and structs for GDI operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal static class Gdi32
	{
		public const string NativeLibrary = "gdi32.dll";

		public const int GCL_HBRBACKGROUND = -10;

		[Flags]
		public enum DrawingOptions
		{
			PRF_CHECKVISIBLE = 0x01,
			PRF_NONCLIENT = 0x02,
			PRF_CLIENT = 0x04,
			PRF_ERASEBKGND = 0x08,
			PRF_CHILDREN = 0x10,
			PRF_OWNED = 0x20
		}

		public enum TernaryRasterOperations : uint
		{
			SRCCOPY = 0x00CC0020,
			SRCPAINT = 0x00EE0086,
			SRCAND = 0x008800C6,
			SRCINVERT = 0x00660046,
			SRCERASE = 0x00440328,
			NOTSRCCOPY = 0x00330008,
			NOTSRCERASE = 0x001100A6,
			MERGECOPY = 0x00C000CA,
			MERGEPAINT = 0x00BB0226,
			PATCOPY = 0x00F00021,
			PATPAINT = 0x00FB0A09,
			PATINVERT = 0x005A0049,
			DSTINVERT = 0x00550009,
			BLACKNESS = 0x00000042,
			WHITENESS = 0x00FF0062,
			CAPTUREBLT = 0x40000000 //only if WinVer >= 5.0.0 (see wingdi.h)
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left, Top, Right, Bottom;

			public RECT(int left, int top, int right, int bottom)
			{
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}

			public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

			public int X
			{
				get => Left;
				set { Right -= Left - value; Left = value; }
			}

			public int Y
			{
				get => Top;
				set { Bottom -= Top - value; Top = value; }
			}

			public int Height
			{
				get => Bottom - Top;
				set => Bottom = value + Top;
			}

			public int Width
			{
				get => Right - Left;
				set => Right = value + Left;
			}

			public System.Drawing.Point Location
			{
				get => new(Left, Top);
				set { X = value.X; Y = value.Y; }
			}

			public System.Drawing.Size Size
			{
				get => new(Width, Height);
				set { Width = value.Width; Height = value.Height; }
			}

			public static implicit operator System.Drawing.Rectangle(RECT r)
			{
				return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
			}

			public static implicit operator RECT(System.Drawing.Rectangle r)
			{
				return new RECT(r);
			}

			public static bool operator ==(RECT r1, RECT r2)
			{
				return r1.Equals(r2);
			}

			public static bool operator !=(RECT r1, RECT r2)
			{
				return !r1.Equals(r2);
			}

			public bool Equals(RECT r) => r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;

			public override bool Equals(object obj)
			{
				return obj switch
				{
					RECT rect => Equals(rect),
					System.Drawing.Rectangle rect => Equals(new RECT(rect)),
					_ => false
				};
			}

			public override int GetHashCode() => ((System.Drawing.Rectangle)this).GetHashCode();

			public override string ToString() => string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
		}

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern BrushHandle CreateSolidBrush(int crColor);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FillRgn(IntPtr hdc, IntPtr hrgn, IntPtr hbr);
	}
}
