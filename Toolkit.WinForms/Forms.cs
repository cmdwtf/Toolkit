using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using static System.Math;
using static cmdwtf.Toolkit.WinForms.Native.Gdi32;
using static cmdwtf.Toolkit.WinForms.Native.User32;
using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Extensions relating to windows themselves. (As in, the application window,
	/// not the operating system Windows.)
	/// </summary>
	public static partial class Forms
	{
		#region Control

		/// <summary>
		/// A shortcut to convert a <see cref="float"/> from logical to
		/// device units (for DPI awareness), first rounding the value
		/// given before passing it to the given control.
		/// </summary>
		/// <param name="c">The <see cref="Control"/> to use to convert units.</param>
		/// <param name="value">The <see cref="float"/> to convert.</param>
		/// <returns>An <see cref="int"/> that represents the converted device units.</returns>
		public static int LogicalToDeviceUnits(this Control c, float value)
			=> c.LogicalToDeviceUnits((int)Round(value));

		/// <summary>
		/// A shortcut to convert a <see cref="SizeF"/> from logical to
		/// device units (for DPI awareness), first rounding the components
		/// of the size before passing it to the given control.
		/// </summary>
		/// <param name="c">The <see cref="Control"/> to use to convert units.</param>
		/// <param name="value">The <see cref="SizeF"/> to convert.</param>
		/// <returns>A new <see cref="Size"/> with units converted to device units.</returns>
		public static Size LogicalToDeviceUnits(this Control c, SizeF value)
		{
			Size rounded = new((int)Round(value.Width), (int)Round(value.Height));
			return c.LogicalToDeviceUnits(rounded);
		}

		/// <summary>
		/// Invalidates the specified region of the control (adds it to the control's update
		/// region, which is the area that will be repainted at the next paint operation),
		/// and causes a paint message to be sent to the control. This is functionally identical to
		/// <see cref="Control.Invalidate(Rectangle)"/>, but calls
		/// <see cref="Rectangle.Round(RectangleF)"/> on the <see cref="RectangleF"/> first.
		/// </summary>
		/// <param name="c">The <see cref="Control"/> to invalidate.</param>
		/// <param name="rc">A <see cref="RectangleF"/> that represents the region to invalidate.</param>
		public static void Invalidate(this Control c, RectangleF rc)
			=> c.Invalidate(Rectangle.Round(rc));

		/// <summary>
		/// Invalidates the specified region of the control (adds it to the control's update
		/// region, which is the area that will be repainted at the next paint operation),
		/// and causes a paint message to be sent to the control. Optionally, invalidates
		/// the child controls assigned to the control. This is functionally identical to
		/// <see cref="Control.Invalidate(Rectangle, bool)"/>, but calls
		/// <see cref="Rectangle.Round(RectangleF)"/> on the <see cref="RectangleF"/> first.
		/// </summary>
		/// <param name="c">The <see cref="Control"/> to invalidate.</param>
		/// <param name="rc">A <see cref="RectangleF"/> that represents the region to invalidate.</param>
		/// <param name="invalidateChildren">true to invalidate the control's child controls; otherwise, false.</param>
		public static void Invalidate(this Control c, RectangleF rc, bool invalidateChildren)
			=> c.Invalidate(Rectangle.Round(rc), invalidateChildren);

		#endregion Control

		/// <summary>
		/// The ValidateRect function validates the client area within a rectangle
		/// by removing the rectangle from the update region of the specified window.
		/// </summary>
		/// <param name="window">The window to validate an area on.</param>
		/// <param name="rect">The rectangle of area to validate.</param>
		/// <returns>True on succcess, false on failure.</returns>
		public static bool ValidateRect(this IWin32Window window, Rectangle rect)
		{
			var nativeRect = new RECT(rect);
			return Native.User32.ValidateRect(window.Handle, ref nativeRect);
		}

		/// <summary>
		/// Replaces the specified 32-bit (long) value at the specified offset into the extra class
		/// memory or the WNDCLASSEX structure for the class to which the specified window belongs.
		/// </summary>
		/// <param name="hWnd">The handle to the window of the background to set.</param>
		/// <param name="nIndex">The value to be replaced.</param>
		/// <param name="dwNewLong">The replacement value.</param>
		/// <returns>
		/// If the function succeeds, the return value is the previous value of the specified 32-bit integer.
		/// If the value was not previously set, the return value is zero.
		/// If the function fails, the return value is zero.To get extended error information,
		/// call GetLastError.
		/// </returns>
		/// <seealso>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setclasslonga</seealso>
		public static UIntPtr SetClassLong(this IntPtr hWnd, int nIndex, IntPtr dwNewLong)
		{
			unchecked
			{
				//check for x64
				if (IntPtr.Size > 4)
				{
					return SetClassLongPtr64(hWnd, nIndex, dwNewLong);
				}
				else
				{
					return (UIntPtr)SetClassLongPtr32(hWnd, nIndex, (uint)dwNewLong.ToInt32());
				}
			}
		}

		/// <summary>
		/// Shows a window without activating the window or giving it focus.
		/// </summary>
		/// <param name="window">The window to show.</param>
		/// <returns>True, if successful.</returns>
		public static bool ShowWithNoFoucs(this IWin32Window window)
			=> ShowWindow(new HandleRef(window, window.Handle), ShowWindowCommands.ShowNA);


		/// <summary>
		/// Converts a <see cref="Message.LParam"/> to an X and Y,
		/// for use with mouse Windows Messages.
		/// </summary>
		/// <param name="m">The message to inspect.</param>
		/// <returns>A tuple representing the X and Y positions pulled from <see cref="Message.LParam"/></returns>
		public static (int X, int Y) LParamToXY(this Message m)
		{
			return (Native.Param.SignedLOWORD(m.LParam),
					Native.Param.SignedHIWORD(m.LParam));
		}

		/// <summary>
		/// Converts a <see cref="Message.LParam"/> to a <see cref="Point"/>,
		/// for use with mouse Windows Messages.
		/// </summary>
		/// <param name="m">The message to inspect.</param>
		/// <returns>A <see cref="Point"/> positions pulled from <see cref="Message.LParam"/></returns>
		public static Point LParamToPoint(this Message m)
		{
			(int x, int y) = m.LParamToXY();
			return new Point(x, y);
		}

		/// <summary>
		/// Applies a <see cref="Padding"/> to a <see cref="RectangleF"/>.
		/// </summary>
		/// <param name="rect">The rect to modify.</param>
		/// <param name="padding">The amount to deflate the rect by.</param>
		/// <returns>The modified <see cref="RectangleF"/></returns>
		public static RectangleF ApplyPadding(this RectangleF rect, Padding padding)
			=> RectangleF.Inflate(rect, -padding.Horizontal / 2.0f, -padding.Vertical / 2.0f);

		/// <summary>
		/// Applies a <see cref="Padding"/> to a <see cref="RectangleF"/>.
		/// </summary>
		/// <param name="rect">The rect to modify.</param>
		/// <param name="padding">The amount to deflate the rect by.</param>
		/// <returns>The modified <see cref="Rectangle"/></returns>
		public static Rectangle ApplyPadding(this Rectangle rect, Padding padding)
			=> Rectangle.Inflate(rect, -padding.Horizontal / 2, -padding.Vertical / 2);
	}
}
