using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using static cmdwtf.Toolkit.WinForms.Native.Gdip;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// This is where the fun begins. A collection of tools, helpers, and extensions
	/// that will help leverage far more out of GDI+ graphics, particularly if you're
	/// drawing custom controls.
	/// </summary>
	public static class Drawing
	{
		#region Colored Images

		/// <summary>
		/// Draws an image, but tinted by the tint color.
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="image">The image to draw.</param>
		/// <param name="destRect">The rectangle the image should draw in.</param>
		/// <param name="tintColor">The color to tint the image.</param>
		/// <param name="srcX">The source x of the image to draw from. Defaults to 0 (left).</param>
		/// <param name="srcY">The source y of the image to draw from. Defaults to 0 (top).</param>
		/// <param name="srcWidth">The source width of the image to draw from. Defaults to -1 (will use Image.Width).</param>
		/// <param name="srcHeight">The source height of the image to draw from. Defaults to -1 (will use Image.Width).</param>
		public static void DrawImageTinted(this Graphics g, Image image, Rectangle destRect, System.Drawing.Color tintColor,
				int srcX = 0, int srcY = 0, int srcWidth = -1, int srcHeight = -1)
		{
			using var attribs = new ImageAttributes();
			attribs.SetTintColorMatrix(tintColor);
			g.DrawImageAttributed(image, destRect, srcX, srcY, srcWidth, srcHeight, attribs);
		}

		/// <summary>
		/// Draws an image, but in grayscale.
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="image">The image to draw.</param>
		/// <param name="destRect">The rectangle the image should draw in.</param>
		/// <param name="srcX">The source x of the image to draw from. Defaults to 0 (left).</param>
		/// <param name="srcY">The source y of the image to draw from. Defaults to 0 (top).</param>
		/// <param name="srcWidth">The source width of the image to draw from. Defaults to -1 (will use Image.Width).</param>
		/// <param name="srcHeight">The source height of the image to draw from. Defaults to -1 (will use Image.Width).</param>
		/// <remarks>See also: https://www.codeproject.com/Articles/3772/ColorMatrix-Basics-Simple-Image-Color-Adjustment </remarks>
		public static void DrawImageGrayscale(this Graphics g, Image image, Rectangle destRect,
				int srcX = 0, int srcY = 0, int srcWidth = -1, int srcHeight = -1)
		{
			using var attribs = new ImageAttributes();
			attribs.SetColorMatrix(ColorMatrix.GrayscaleColorMatrix);
			g.DrawImageAttributed(image, destRect, srcX, srcY, srcWidth, srcHeight, attribs);
		}

		/// <summary>
		/// Draws an image, but hue-shifted by the hue color given and intensity desired.
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="image">The image to draw.</param>
		/// <param name="destRect">The rectangle the image should draw in.</param>
		/// <param name="hueColor">The color to shift the hue to.</param>
		/// <param name="intensity">A multiplier describing how intense the hue shift should be, defauls to 1.0.</param>
		/// <param name="srcX">The source x of the image to draw from. Defaults to 0 (left).</param>
		/// <param name="srcY">The source y of the image to draw from. Defaults to 0 (top).</param>
		/// <param name="srcWidth">The source width of the image to draw from. Defaults to -1 (will use Image.Width).</param>
		/// <param name="srcHeight">The source height of the image to draw from. Defaults to -1 (will use Image.Width).</param>
		public static void DrawImageHued(this Graphics g, Image image, Rectangle destRect, System.Drawing.Color hueColor, float intensity = 1.0f,
				int srcX = 0, int srcY = 0, int srcWidth = -1, int srcHeight = -1)
		{
			using var attribs = new ImageAttributes();
			attribs.SetHueColorMatrix(hueColor, intensity);
			g.DrawImageAttributed(image, destRect, srcX, srcY, srcWidth, srcHeight, attribs);
		}

		/// <summary>
		/// Draws an image, but hue-shifted by the hue color given and intensity desired.
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="image">The image to draw.</param>
		/// <param name="destRect">The rectangle the image should draw in.</param>
		/// <param name="srcX">The source x of the image to draw from. Defaults to 0 (left).</param>
		/// <param name="srcY">The source y of the image to draw from. Defaults to 0 (top).</param>
		/// <param name="srcWidth">The source width of the image to draw from. Defaults to -1 (will use Image.Width).</param>
		/// <param name="srcHeight">The source height of the image to draw from. Defaults to -1 (will use Image.Width).</param>
		/// <param name="attribs">Attributes with which to draw the image with.</param>
		public static void DrawImageAttributed(this Graphics g, Image image, Rectangle destRect,
		int srcX = 0, int srcY = 0, int srcWidth = 0, int srcHeight = 0, ImageAttributes attribs = null)
		{
			if (image == null)
			{
				throw new ArgumentNullException(nameof(image));
			}

			if (srcWidth == -1)
			{
				srcWidth = image.Width;
			}

			if (srcHeight == -1)
			{
				srcHeight = image.Height;
			}

			g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel, attribs);
		}

		#endregion Colored Images

		/// <summary>
		/// Resizes an image to the given width and height.
		/// </summary>
		/// <param name="source">The image to resize.</param>
		/// <param name="width">The desired width of the image.</param>
		/// <param name="height">The desired height of the image.</param>
		/// <returns>The resized image.</returns>
		public static Bitmap Resize(this Image source, int width, int height)
		{
			var result = new Bitmap(width, height);
			var g = Graphics.FromImage(result);

			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(source, 0, 0, width, height);
			g.Flush(FlushIntention.Flush);

			return result;
		}

		/// <summary>
		/// Creates a rectangular graphics path, but with arced corners.
		/// </summary>
		/// <param name="rect">The rectangle to draw.</param>
		/// <param name="slope">The slope of the arc of the corners.</param>
		/// <returns>The rounded rectangle path.</returns>
		public static GraphicsPath RoundRect(Rectangle rect, int slope)
		{
			var graphicsPath = new GraphicsPath();

			checked
			{
				int num = slope * 2;
				graphicsPath.AddArc(new Rectangle(rect.X, rect.Y, num, num), -180f, 90f);
				graphicsPath.AddArc(new Rectangle(rect.Width - num + rect.X, rect.Y, num, num), -90f, 90f);
				graphicsPath.AddArc(new Rectangle(rect.Width - num + rect.X, rect.Height - num + rect.Y, num, num), 0f, 90f);
				graphicsPath.AddArc(new Rectangle(rect.X, rect.Height - num + rect.Y, num, num), 90f, 90f);
				graphicsPath.CloseAllFigures();
				return graphicsPath;
			}
		}

		/// <summary>
		/// Creates a SolidBrush, virtually identical to <see cref="SolidBrush"/>
		/// However, it uses the native call so that it can pass it to the native function
		/// <see cref="SetBackgroundBrush(IntPtr, Native.BrushHandle)"/>, which normal brushes
		/// can't. Because for some reason, they hide their handle, and won't let us have nice things.
		/// </summary>
		/// <param name="_">Ignored. This is just so this function is an extension method on IntPtr.</param>
		/// <param name="color">The color of the brush to create.</param>
		/// <returns>The native brush handle.</returns>
		public static Native.BrushHandle CreateSolidBrush(this IntPtr _, System.Drawing.Color color)
			=> Native.Gdip.CreateSolidBrush(ColorTranslator.ToWin32(color));

		/// <summary>
		/// Sets the window's background brush to the given native brush.
		/// </summary>
		/// <param name="hWnd">The handle to the window of the background to set.</param>
		/// <param name="hBrush">The handle to the native brush.</param>
		/// <returns>The handle of the previous background brush if successful, or Zero on error.</returns>
		public static IntPtr SetBackgroundBrush(this IntPtr hWnd, Native.BrushHandle hBrush)
			=> hWnd.SetClassLong(GCL_HBRBACKGROUND, hBrush.DangerousGetHandle());

		#region Arrow Drawing

		/// <summary>
		/// Draws a simple arrow from origin to destination in the desired color and width.
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="origin">Where the 'tail' end of the arrow should start.</param>
		/// <param name="destination">Where the 'tip' end of the arrow should end.</param>
		/// <param name="color">The color of the arrow.</param>
		/// <param name="width">The width of the arrow.</param>
		public static void DrawArrow2(this Graphics g, PointF origin, PointF destination, System.Drawing.Color color, float width = 1.0f)
		{
			using var pen = new Pen(color, width)
			{
				StartCap = LineCap.Flat,
				EndCap = LineCap.ArrowAnchor
			};
			g.DrawLine(pen, origin, destination);
		}

		/// <summary>
		/// A set of points to save the transformed arrow shape info in.
		/// </summary>
		private static readonly PointF[] _transformedArrowPositions2 =
			new PointF[] { new(), new(), new(), new() };

		/// <summary>
		/// Draws a fancier arrow from origin to destination in the desired color and width.
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="origin">Where the 'tail' end of the arrow should start.</param>
		/// <param name="destination">Where the 'tip' end of the arrow should end.</param>
		/// <param name="color">The color of the arrow.</param>
		/// <param name="width">The width of the arrow.</param>
		/// <param name="multiplier">A multiplier to use arrow-wide.</param>
		// Originally via William Winner: https://www.codeproject.com/Answers/125075/Draw-an-arrow-with-big-cap#answer2
		public static void DrawArrow2(this Graphics g, PointF origin, PointF destination, System.Drawing.Color color, float width = 1.0f, float multiplier = 1.0f)
		{
			using var pen = new Pen(color, width);

			//draw the line
			g.DrawLine(pen, origin, destination);

			//determine the coords for the arrow point

			//tip of the arrow
			PointF arrowPoint = destination;

			//determine arrow length
			double arrowLength = System.Math.Sqrt(System.Math.Pow(System.Math.Abs(origin.X - destination.X), 2) +
										   System.Math.Pow(System.Math.Abs(origin.Y - destination.Y), 2));

			//determine arrow angle
			double arrowAngle = System.Math.Atan2(System.Math.Abs(origin.Y - destination.Y), System.Math.Abs(origin.X - destination.X));

			//get the x,y of the back of the point

			//to change from an arrow to a diamond, change the 3
			//in the next if/else blocks to 6

			double pointX, pointY;
			if (origin.X > destination.X)
			{
				pointX = origin.X - (System.Math.Cos(arrowAngle) * (arrowLength - (3 * multiplier)));
			}
			else
			{
				pointX = (System.Math.Cos(arrowAngle) * (arrowLength - (3 * multiplier))) + origin.X;
			}

			if (origin.Y > destination.Y)
			{
				pointY = origin.Y - (System.Math.Sin(arrowAngle) * (arrowLength - (3 * multiplier)));
			}
			else
			{
				pointY = (System.Math.Sin(arrowAngle) * (arrowLength - (3 * multiplier))) + origin.Y;
			}

			var arrowPointBack = new PointF((float)pointX, (float)pointY);

			//get the secondary angle of the left tip
			double angleB = System.Math.Atan2((3 * multiplier), (arrowLength - (3 * multiplier)));

			double angleC = System.Math.PI * (90 - (arrowAngle * (180 / System.Math.PI)) - (angleB * (180 / System.Math.PI))) / 180;

			//get the secondary length
			double secondaryLength = 3 * multiplier / System.Math.Sin(angleB);

			if (origin.X > destination.X)
			{
				pointX = origin.X - (System.Math.Sin(angleC) * secondaryLength);
			}
			else
			{
				pointX = (System.Math.Sin(angleC) * secondaryLength) + origin.X;
			}

			if (origin.Y > destination.Y)
			{
				pointY = origin.Y - (System.Math.Cos(angleC) * secondaryLength);
			}
			else
			{
				pointY = (System.Math.Cos(angleC) * secondaryLength) + origin.Y;
			}

			//get the left point
			var arrowPointLeft = new PointF((float)pointX, (float)pointY);

			//move to the right point
			angleC = arrowAngle - angleB;

			if (origin.X > destination.X)
			{
				pointX = origin.X - (System.Math.Cos(angleC) * secondaryLength);
			}
			else
			{
				pointX = (System.Math.Cos(angleC) * secondaryLength) + origin.X;
			}

			if (origin.Y > destination.Y)
			{
				pointY = origin.Y - (System.Math.Sin(angleC) * secondaryLength);
			}
			else
			{
				pointY = (System.Math.Sin(angleC) * secondaryLength) + origin.Y;
			}

			var arrowPointRight = new PointF((float)pointX, (float)pointY);

			//create the point list
			_transformedArrowPositions2[0] = arrowPoint;
			_transformedArrowPositions2[1] = arrowPointLeft;
			_transformedArrowPositions2[2] = arrowPointBack;
			_transformedArrowPositions2[3] = arrowPointRight;

			//draw the outline
			g.DrawPolygon(pen, _transformedArrowPositions2);

			//fill the polygon
			using var brush = new SolidBrush(color);
			g.FillPolygon(brush, _transformedArrowPositions2);
		}

		/// <summary>
		/// Arrow shape information, for DrawArrow
		/// </summary>
		private static readonly PointF[] _arrowPositions = new PointF[]
		{
			new(4.0f, 0.0f),
			new(0.0f, 4.0f),
			new(0.0f, -4.0f),
		};

		/// <summary>
		/// A set of points to save the transformed arrow shape info in.
		/// </summary>
		private static readonly PointF[] _transformedArrowPositions =
			new PointF[] { new(), new(), new() };

		/// <summary>
		/// Draws an fancy arrow from a given point rotated by the given angle, scaled by the length.
		/// This, as opposed to <see cref="DrawArrow2(Graphics, PointF, PointF, System.Drawing.Color, float)"/>,
		/// is intended more as a use for drawing a tangent/normal/ray, rather than an arrow with a specific start/stop.
		/// </summary>
		/// <param name="g">The graphics device to draw with.</param>
		/// <param name="origin">Where the arrow should start.</param>
		/// <param name="angle">The angle it should point.</param>
		/// <param name="length">How long (in pixels) should the arrow be?</param>
		/// <param name="color">The color to draw the arrow with.</param>
		public static void DrawArrow(this Graphics g, PointF origin, float angle, float length, System.Drawing.Color color)
		{
			using var pen = new Pen(color);
			using var brush = new SolidBrush(color);

			float sin = (float)System.Math.Sin(angle);
			float cos = (float)System.Math.Cos(angle);

			var pt = new PointF(origin.X + (cos * length), origin.Y - (sin * length));
			g.DrawLine(pen, origin, pt);

			// transform points.
			for (int i = 0; i < _arrowPositions.Length; ++i)
			{
				_transformedArrowPositions[i].X =
					pt.X + ((cos * _arrowPositions[i].X) - (sin * _arrowPositions[i].Y));
				_transformedArrowPositions[i].Y =
					pt.Y - ((sin * _arrowPositions[i].X) + (cos * _arrowPositions[i].Y));
			}

			g.FillPolygon(brush, _transformedArrowPositions);
		}

		#endregion Arrow Drawing
	}
}
