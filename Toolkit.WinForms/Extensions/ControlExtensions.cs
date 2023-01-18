using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using static System.Math;
using static cmdwtf.Toolkit.WinForms.Native.Gdi32;
using static cmdwtf.Toolkit.WinForms.Native.User32;
using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms.Extensions
{
	/// <summary>
	/// Extensions that relate to multiple or non-specific WinForms Controls.
	/// </summary>
	public static class ControlExtensions
	{
		#region Control Rendering

		/// <summary>
		/// Renders a control as a bitmap. This is an alternative to
		/// <see cref="Control.DrawToBitmap(Bitmap, Rectangle)"/>, that provides
		/// the control in a higher quality.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="pixelFormat"></param>
		/// <returns></returns>
		public static Bitmap RenderControl(this Control control, PixelFormat pixelFormat = PixelFormat.Format32bppArgb)
		{
			var bitmap = new Bitmap(control.Width, control.Height, pixelFormat);
			bitmap.MakeTransparent(bitmap.GetPixel(0, 0));

			// draw to bitmap renders in reverse order, but
			// there's not much we can do about that.
			// if you handle your own painting (or handle it in 'OnPrint()',
			// things should draw how you want.)
			control.DrawToBitmapHighQuality(bitmap, new Rectangle(Point.Empty, control.Size));

			return bitmap;
		}

		/// <summary>
		/// A custom version of Control.DrawToBitmap based on mscorlib 4.0.0.0.
		/// Functions very similarly, but uses CompositingQuality.HighQuality,
		/// InterpolationMode.HighQualityBicubic, SmoothingMode.HighQuality,
		/// CompositingMode.SourceCopy,
		/// It also does not pass the PRF_ERASEBKGND flag, assuming the destBitmap
		/// to be ready to draw to.
		/// As well, can't call CreateControl() if handle is not created, so the
		/// user should do that first. (Check control.IsHandleCreated)
		/// </summary>
		/// <param name="control">The control to draw.</param>
		/// <param name="destBitmap">The bitmap to render the control to.</param>
		/// <param name="targetBounds">The bounds of the bitmap to draw in.</param>
		public static void DrawToBitmapHighQuality(this Control control, Bitmap destBitmap, Rectangle targetBounds)
		{
			if (destBitmap == null)
			{
				throw new ArgumentNullException(nameof(destBitmap));
			}
			if (targetBounds.Width <= 0 || targetBounds.Height <= 0 || targetBounds.X < 0 || targetBounds.Y < 0)
			{
				throw new ArgumentException($"{nameof(targetBounds)} must have valid bounds.", nameof(targetBounds));
			}
			if (!control.IsHandleCreated)
			{
				throw new ArgumentException("control must have it's handle created first. Try calling control.CreateControl() if IsHandleCreated is false.");
			}

			CompositingMode compositingMode = CompositingMode.SourceCopy;
			CompositingQuality compostingQuality = CompositingQuality.HighQuality;
			InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic;
			SmoothingMode smoothingMode = SmoothingMode.HighQuality;

			int nWidth = Min(control.Width, targetBounds.Width);
			int nHeight = Min(control.Height, targetBounds.Height);
			using var tempBitmap = new Bitmap(nWidth, nHeight, destBitmap.PixelFormat);
			using var tempGdi = Graphics.FromImage(tempBitmap);

			tempGdi.CompositingQuality = compostingQuality;
			tempGdi.InterpolationMode = interpolationMode;
			tempGdi.SmoothingMode = smoothingMode;
			tempGdi.CompositingMode = compositingMode;

			IntPtr hdc = tempGdi.GetHdc();

			DrawingOptions drawingOptions = DrawingOptions.PRF_NONCLIENT |
					DrawingOptions.PRF_CLIENT |
					//Native.DrawingOptions.PRF_ERASEBKGND |
					DrawingOptions.PRF_CHILDREN;

			_ = SendMessage(new HandleRef(control, control.Handle), WM.PRINT, hdc, (IntPtr)drawingOptions);

			using var targetGdi = Graphics.FromImage(destBitmap);
			targetGdi.CompositingMode = compositingMode;
			targetGdi.CompositingQuality = compostingQuality;
			targetGdi.InterpolationMode = interpolationMode;
			targetGdi.SmoothingMode = smoothingMode;

			IntPtr hdc2 = targetGdi.GetHdc();

			BitBlt(new HandleRef(targetGdi, hdc2), targetBounds.X, targetBounds.Y, nWidth, nHeight,
						  new HandleRef(tempGdi, hdc), 0, 0, (int)TernaryRasterOperations.SRCCOPY);

			targetGdi.ReleaseHdcInternal(hdc2);
			tempGdi.ReleaseHdcInternal(hdc);
		}

		/// <summary>
		/// Suspends painting on the given control, via the WM_SETREDRAW message.
		/// </summary>
		/// <param name="control">The control to stop painting.</param>
		/// <returns>True, on success.</returns>
		public static bool SuspendPainting(this Control control)
			=> SendMessage(control.Handle, WM.SETREDRAW, false, IntPtr.Zero) == 0;

		/// <summary>
		/// Resumes painting on the given control, if it was stopped via the WM_SETREDRAW message.
		/// </summary>
		/// <param name="control">The control to stop painting.</param>
		/// <returns>True, on success.</returns>
		public static bool ResumePainting(this Control control)
			=> SendMessage(control.Handle, WM.SETREDRAW, true, IntPtr.Zero) == 0;

		#endregion Control Rendering

		/// <summary>
		/// Gets a rectangle representing the control's coordinates, relative
		/// to the form they're contained in.
		/// </summary>
		/// <param name="control">The control to get the coordinates of.</param>
		/// <returns>The coordinate rectangle.</returns>
		public static Rectangle Coordinates(this Control control)
		{
			Form form = control.TopLevelControl as Form ?? throw new NullReferenceException("Control's TopLevelControl isn't a form.");

			return control == form || control.Parent == null
				? form.ClientRectangle
				: form.RectangleToClient(control.Parent.RectangleToScreen(control.Bounds));
		}

		/// <summary>
		/// Gets a point that describes the relation from one control
		/// to another, in screen space.
		/// </summary>
		/// <param name="control">The control to measure from.</param>
		/// <param name="toControl">The control to measure to.</param>
		/// <returns>The point.</returns>
		public static Point RelationTo(this Control control, Control toControl)
		{
			Point fromPoint = control.PointToScreen(Point.Empty);
			Point toPoint = toControl.PointToScreen(Point.Empty);
			return fromPoint.Subtract(toPoint);
		}

		/// <summary>
		/// Gets a rectangle that starts in relation from the difference between
		/// the two controls, and a size based on the first control.
		/// </summary>
		/// <param name="control">The control to measure from, and use the size of.</param>
		/// <param name="toControl">The control to measure to.</param>
		/// <returns>The rectangle.</returns>
		public static Rectangle RelativeClientRectangle(this Control control, Control toControl)
			=> new(control.RelationTo(toControl), control.Size);

		#region Style Flags

		/// <summary>
		/// Gets the native style flag of the specified value using the
		/// GWL_STYLE index of GetWindowLong.
		/// </summary>
		/// <param name="control">The control to get the style of.</param>
		/// <param name="flag">The flag value to get.</param>
		/// <returns><c>true</c> if the flag is set, otherwise <c>false</c>.</returns>
		public static bool GetNativeStyleFlagSet(this Control control, int flag)
		{
			if (control.IsHandleCreated == false)
			{
				return false;
			}

			int style = control.GetWindowLong(Native.User32.GWL.STYLE);
			return (style & flag) != 0;
		}

		/// <summary>
		/// Sets the native style flag of the specified value using the
		/// GWL_STYLE index of SetWindowLong.
		/// </summary>
		/// <param name="control">The control to set the style on.</param>
		/// <param name="flag">The flag value to set.</param>
		/// <param name="value"><c>true</c> to set the flag, <c>false</c> to clear it.</param>
		/// <returns>The previous GWL_STYLE value.</returns>
		public static int SetNativeStyleFlag(this Control control, int flag, bool value)
		{
			if (control.IsHandleCreated == false)
			{
				return -1;
			}

			int currentStyle = control.GetWindowLong(Native.User32.GWL.STYLE);

			int newStyle = value switch
			{
				true => currentStyle |= flag,
				false => currentStyle &= ~flag,
			};


			// don't bother calling set if the style isn't changing.
			return newStyle == currentStyle
				? currentStyle
				: control.SetWindowLong(Native.User32.GWL.STYLE, newStyle);
		}

		#endregion Style Flags

		#region Class and Window longs

		/// <summary>
		/// Gets the specified 32-bit (long) value at the specified offset into the extra class
		/// memory or the WNDCLASSEX structure for the class to which the specified window belongs.
		/// <seealso href="https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getclasslonga"/>
		/// </summary>
		/// <param name="control">The window to get the class long for.</param>
		/// <param name="index">The index of the class long to get.</param>
		/// <returns>
		/// If the function succeeds, the return value is the value of the specified 32-bit integer.
		/// If the value was not previously set, the return value is zero.
		/// If the function fails, the return value is zero. To get extended error information,
		/// call GetLastError.
		/// </returns>
		public static int GetClassLong(this Control control, int index)
			=> Native.User32.GetClassLong(control.Handle, index);

		/// <inheritdoc cref="GetClassLong(Control, int)"/>
		public static UIntPtr GetClassLongPtr(this Control control, int index)
			=> Native.User32.GetClassLongPtr(control.Handle, index);

		/// <summary>
		/// Replaces the specified 32-bit (long) value at the specified offset into the extra class
		/// memory or the WNDCLASSEX structure for the class to which the specified window belongs.
		/// <seealso href="https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setclasslonga"/>
		/// </summary>
		/// <param name="control">The window to set the class long for.</param>
		/// <param name="index">The index of the class long to set.</param>
		/// <param name="value">The value to set for the class long.</param>
		/// <returns>
		/// If the function succeeds, the return value is the previous value of the specified 32-bit integer.
		/// If the value was not previously set, the return value is zero.
		/// If the function fails, the return value is zero. To get extended error information,
		/// call GetLastError.
		/// </returns>
		public static uint SetClassLong(this Control control, int index, int value)
			=> Native.User32.SetClassLong(control.Handle, index, value);

		/// <inheritdoc cref="SetClassLong(Control, int, int)"/>
		public static UIntPtr SetClassLongPtr(this Control control, int index, IntPtr value)
			=> Native.User32.SetClassLongPtr(control.Handle, index, value);


		/// <summary>
		/// Retrieves information about the specified window. The function
		/// also retrieves the value at a specified offset into the extra window memory.
		/// <seealso href="https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowlonga"/>
		/// </summary>
		/// <param name="control">The window to get the window long for.</param>
		/// <param name="index">The index of the window long to get.</param>
		/// <returns>
		/// If the function succeeds, the return value is the value of the specified 32-bit integer.
		/// If the value was not previously set, the return value is zero.
		/// If the function fails, the return value is zero. To get extended error information,
		/// call GetLastError.
		/// </returns>
		public static int GetWindowLong(this Control control, int index)
			=> Native.User32.GetWindowLong(control.Handle, index);

		/// <inheritdoc cref="GetWindowLong(Control, int)"/>
		public static IntPtr GetWindowLongPtr(this Control control, int index)
			=> Native.User32.GetWindowLongPtr(control.Handle, index);

		/// <summary>
		/// Changes an attribute of the specified window. The function also
		/// sets a value at the specified offset in the extra window memory.
		/// <seealso href="https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowlonga"/>
		/// </summary>
		/// <param name="control">The window to set the window long for.</param>
		/// <param name="index">The index of the window long to set.</param>
		/// <param name="value">The value to set for the window long.</param>
		/// <returns>
		/// If the function succeeds, the return value is the previous value of the specified 32-bit integer.
		/// If the value was not previously set, the return value is zero.
		/// If the function fails, the return value is zero. To get extended error information,
		/// call GetLastError.
		/// </returns>
		public static int SetWindowLong(this Control control, int index, int value)
			=> Native.User32.SetWindowLong(control.Handle, index, value);

		/// <inheritdoc cref="SetWindowLong(Control, int, int)"/>
		public static IntPtr SetWindowLongPtr(this Control control, int index, IntPtr value)
			=> Native.User32.SetWindowLongPtr(control.Handle, index, value);

		#endregion Class and Window longs
	}
}
