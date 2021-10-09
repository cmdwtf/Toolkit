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
			var form = control.TopLevelControl as Form;

			return control == form ?
				form.ClientRectangle :
				form.RectangleToClient(control.Parent.RectangleToScreen(control.Bounds));
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


	}
}
