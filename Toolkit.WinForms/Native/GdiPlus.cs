using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Native methods, constants, and structs for GDI+ (GDI Plus) operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	/// <seealso>https://docs.microsoft.com/en-us/windows/win32/gdiplus/-gdiplus-flatapi-flat </seealso>
	internal static class GdiPlus
	{
		public const string NativeLibrary = "gdiplus.dll";

		private static readonly PropertyInfo PathGradientBrushNativeBrushProperty;

		static GdiPlus()
		{
			PathGradientBrushNativeBrushProperty = typeof(PathGradientBrush).GetProperty("NativeBrush");
		}

		/// <summary>
		/// Sets the gamma correction property on the given brush.
		/// </summary>
		/// <param name="brush">The brush to adjust.</param>
		/// <param name="useGammaCorrection">If true, to use gamma correction.</param>
		/// <returns>true, on success.</returns>
		public static bool SetPathGammaCorrection(this PathGradientBrush brush, bool useGammaCorrection)
		{
			if (PathGradientBrushNativeBrushProperty != null)
			{
				var brushRef = (HandleRef)PathGradientBrushNativeBrushProperty.GetValue(brush);
				return GdipSetPathGradientGammaCorrection(brushRef, useGammaCorrection);
			}

			return false;
		}

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GdipSetPathGradientGammaCorrection(HandleRef brush, bool useGammaCorrection);
	}
}
