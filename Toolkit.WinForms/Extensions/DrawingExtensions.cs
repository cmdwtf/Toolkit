using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

using static cmdwtf.Toolkit.WinForms.Native.Gdi32;
using static cmdwtf.Toolkit.WinForms.Native.GdiPlus;

using VerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment;

namespace cmdwtf.Toolkit.WinForms.Extensions
{
	/// <summary>
	/// This is where the fun begins. A collection of tools, helpers, and extensions
	/// that will help leverage far more out of GDI+ graphics, particularly if you're
	/// drawing custom controls.
	/// </summary>
	public static partial class DrawingExtensions
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
		public static void DrawImageTinted(this Graphics g, Image image, Rectangle destRect, Color tintColor,
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
			attribs.SetColorMatrix(ColorMatrixExtensions.GrayscaleColorMatrix);
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
		public static void DrawImageHued(this Graphics g, Image image, Rectangle destRect, Color hueColor, float intensity = 1.0f,
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

		/// <inheritdoc cref="FillRectangleRadialGradient(Graphics, Color, RectangleF, Color?, PointF?)"/>
		public static void FillRectangleRadialGradient(this Graphics g, Color fillColor, Rectangle rect, Color? highlightColor = null, Point? centerPoint = null)
			=> g.FillRectangleRadialGradient(fillColor, (RectangleF)rect, highlightColor, centerPoint);


		/// <summary>
		/// Fills a rectangle with a radial gradient.
		/// </summary>
		/// <param name="g">The <see cref="Graphics"/> to draw into.</param>
		/// <param name="fillColor">The color to start the gradient with.</param>
		/// <param name="rect">The rectangle to fill.</param>
		/// <param name="highlightColor">An optional color to end the gradient with. If null, will use <see cref="ControlPaint.Light(Color)"/> to calculate one.</param>
		/// <param name="centerPoint">The center point of the gradient.</param>
		/// <remarks>Thanks, <see href="https://www.codeproject.com/Articles/20018/Gradients-made-easy"/></remarks>
		public static void FillRectangleRadialGradient(this Graphics g, Color fillColor, RectangleF rect, Color? highlightColor = null, PointF? centerPoint = null)
		{
			using GraphicsPath path = new();
			path.AddEllipse(rect);

			// Optional: create a blend for the gradient
			//Blend blend = new Blend();

			if (highlightColor is null)
			{
				highlightColor = ControlPaint.Light(fillColor);
			}

			using var pathBrush = new PathGradientBrush(path)
			{
				//CenterPoint = new PointF(target.Width / 2, target.Height / 2),
				CenterColor = highlightColor.Value,
				SurroundColors = new Color[] { fillColor },
				WrapMode = WrapMode.Clamp
				//Blend = blend
			};

			if (centerPoint is not null)
			{
				pathBrush.CenterPoint = centerPoint.Value;
			}

			// for some reason, PathGradientBrush.GammaCorrection isn't in the API,
			// so we have to call it natively ourselves.
			pathBrush.SetPathGammaCorrection(true);
			//pathBrush.SetSigmaBellShape(0);

			using SolidBrush fillBrush = new(fillColor);

			g.FillRectangle(fillBrush, rect);
			g.FillRectangle(pathBrush, rect);
		}

		/// <summary>
		/// Fills a rectangle with a linear gradient.
		/// </summary>
		/// <param name="g">The <see cref="Graphics"/> to draw into.</param>
		/// <param name="fillColor">The color to start the gradient with.</param>
		/// <param name="rect">The rectangle to fill.</param>
		/// <param name="highlightColor">An optional color to end the gradient with. If null, will use <see cref="ControlPaint.Light(Color)"/> to calculate one.</param>
		/// <param name="angle">The angle of the gradient. 0 is horizontal from left to right. Should be between 0 and 360.</param>
		public static void FillRectangleLinearGradient(this Graphics g, Color fillColor, RectangleF rect, Color? highlightColor = null, float angle = 0.0f)
		{
			// if we have a rect with 0 area, there's nothing to fill.
			if (rect.Width <= 0 || rect.Height <= 0)
			{
				return;
			}

			Color highlight = highlightColor ?? ControlPaint.Light(fillColor);

			angle = (angle is > 360 or < 0) ? 0 : angle;

			using LinearGradientBrush brush = new(rect, fillColor, highlight, angle, true);

			brush.GammaCorrection = true;

			g.FillRectangle(brush, rect);
		}

		/// <summary>
		/// Draws an 'error rectangle' (red outline with a red cross through it)
		/// to the given <see cref="Graphics"/> instance.
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="rect">The <see cref="Rectangle"/> dimensions to draw.</param>
		/// <param name="penWidth">The width of the <see cref="Pen"/> to draw with.</param>
		public static void DrawErrorRectangle(this Graphics g, Rectangle rect, float penWidth = 2.0f)
			=> g.DrawErrorRectangle((RectangleF)rect, penWidth);

		/// <summary>
		/// Draws an 'error rectangle' (red outline with a red cross through it)
		/// to the given <see cref="Graphics"/> instance.
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="rect">The <see cref="Rectangle"/> dimensions to draw.</param>
		/// <param name="penWidth">The width of the <see cref="Pen"/> to draw with.</param>
		public static void DrawErrorRectangle(this Graphics g, RectangleF rect, float penWidth = 2.0f)
		{
			// deflate the rect so it doesn't draw out of the bounds if the width is thicker.
			int rectDeflatePixels = (int)penWidth - 1;
			rect = RectangleF.Inflate(rect, -rectDeflatePixels, -rectDeflatePixels);

			using Pen redPen = new(Color.Red, penWidth);

			g.DrawLine(redPen, rect.TopLeft(), rect.BottomRight());
			g.DrawLine(redPen, rect.TopRight(), rect.BottomLeft());

			g.DrawRectangle(redPen, rect);
		}

		/// <inheritdoc cref="Graphics.DrawRectangle(Pen, Rectangle)"/>
		public static void DrawRectangle(this Graphics g, Pen pen, RectangleF rect)
			=> g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

		/// <summary>
		/// Draws an image into the specified <see cref="Graphics"/> instance,
		/// using the given <paramref name="rect"/> as it's target area, but determining
		/// position based on <paramref name="alignment"/> and size from <paramref name="image"/>
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="image">The <see cref="Image"/> to draw.</param>
		/// <param name="rect">The <see cref="RectangleF"/> area to draw into.</param>
		/// <param name="alignment">The <see cref="ContentAlignment"/> to draw the image at.</param>
		/// <returns>The calculated <see cref="Rectangle"/> that was used to draw the image.</returns>
		public static RectangleF DrawImage(this Graphics g, Image image, RectangleF rect, ContentAlignment alignment)
			=> g.DrawImage(image, Rectangle.Truncate(rect), alignment);

		/// <summary>
		/// Draws an image into the specified <see cref="Graphics"/> instance,
		/// using the given <paramref name="rect"/> as it's target area, but determining
		/// position based on <paramref name="alignment"/> and size from <paramref name="image"/>
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="image">The <see cref="Image"/> to draw.</param>
		/// <param name="rect">The <see cref="Rectangle"/> area to draw into.</param>
		/// <param name="alignment">The <see cref="ContentAlignment"/> to draw the image at.</param>
		/// <returns>The calculated <see cref="Rectangle"/> that was used to draw the image.</returns>
		public static Rectangle DrawImage(this Graphics g, Image image, Rectangle rect, ContentAlignment alignment)
		{
			rect = image.CalcImageRenderBounds(rect, alignment);
			g.DrawImage(image, rect.X, rect.Y, image.Width, image.Height);
			return rect;
		}

		/// <summary>
		/// Creates a SolidBrush, virtually identical to <see cref="SolidBrush"/>
		/// However, it uses the native call so that it can pass it to the native function
		/// <see cref="SetBackgroundBrush(Control, Native.BrushHandle)"/>, which normal brushes
		/// can't. Because for some reason, they hide their handle, and won't let us have nice things.
		/// </summary>
		/// <param name="_">Ignored. This is just so this function is an extension method on IntPtr.</param>
		/// <param name="color">The color of the brush to create.</param>
		/// <returns>The native brush handle.</returns>
		public static Native.BrushHandle CreateNativeSolidBrush(this Control _, Color color)
			=> CreateSolidBrush(ColorTranslator.ToWin32(color));

		/// <summary>
		/// Sets the window's background brush to the given native brush.
		/// </summary>
		/// <param name="hWnd">The handle to the window of the background to set.</param>
		/// <param name="hBrush">The handle to the native brush.</param>
		/// <returns>The handle of the previous background brush if successful, or Zero on error.</returns>
		public static UIntPtr SetBackgroundBrush(this Control hWnd, Native.BrushHandle hBrush)
			=> hWnd.SetClassLongPtr(GCL_HBRBACKGROUND, hBrush.DangerousGetHandle());

		#region Arrow Drawing

		/// <summary>
		/// Draws a simple arrow from origin to destination in the desired color and width.
		/// </summary>
		/// <param name="g">The graphics to draw to.</param>
		/// <param name="origin">Where the 'tail' end of the arrow should start.</param>
		/// <param name="destination">Where the 'tip' end of the arrow should end.</param>
		/// <param name="color">The color of the arrow.</param>
		/// <param name="width">The width of the arrow.</param>
		public static void DrawArrow2(this Graphics g, PointF origin, PointF destination, Color color, float width = 1.0f)
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
		private static readonly PointF[] TransformedArrowPositions2 =
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
		public static void DrawArrow2(this Graphics g, PointF origin, PointF destination, Color color, float width = 1.0f, float multiplier = 1.0f)
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
			pointX = origin.X > destination.X
				? origin.X - (System.Math.Cos(arrowAngle) * (arrowLength - (3 * multiplier)))
				: (System.Math.Cos(arrowAngle) * (arrowLength - (3 * multiplier))) + origin.X;

			pointY = origin.Y > destination.Y
				? origin.Y - (System.Math.Sin(arrowAngle) * (arrowLength - (3 * multiplier)))
				: (System.Math.Sin(arrowAngle) * (arrowLength - (3 * multiplier))) + origin.Y;

			var arrowPointBack = new PointF((float)pointX, (float)pointY);

			//get the secondary angle of the left tip
			double angleB = System.Math.Atan2((3 * multiplier), (arrowLength - (3 * multiplier)));

			double angleC = System.Math.PI * (90 - (arrowAngle * (180 / System.Math.PI)) - (angleB * (180 / System.Math.PI))) / 180;

			//get the secondary length
			double secondaryLength = 3 * multiplier / System.Math.Sin(angleB);

			pointX = origin.X > destination.X
				? origin.X - (System.Math.Sin(angleC) * secondaryLength)
				: (System.Math.Sin(angleC) * secondaryLength) + origin.X;

			pointY = origin.Y > destination.Y
				? origin.Y - (System.Math.Cos(angleC) * secondaryLength)
				: (System.Math.Cos(angleC) * secondaryLength) + origin.Y;

			//get the left point
			var arrowPointLeft = new PointF((float)pointX, (float)pointY);

			//move to the right point
			angleC = arrowAngle - angleB;

			pointX = origin.X > destination.X
				? origin.X - (System.Math.Cos(angleC) * secondaryLength)
				: (System.Math.Cos(angleC) * secondaryLength) + origin.X;

			pointY = origin.Y > destination.Y ? origin.Y - (System.Math.Sin(angleC) * secondaryLength) : (System.Math.Sin(angleC) * secondaryLength) + origin.Y;

			var arrowPointRight = new PointF((float)pointX, (float)pointY);

			//create the point list
			TransformedArrowPositions2[0] = arrowPoint;
			TransformedArrowPositions2[1] = arrowPointLeft;
			TransformedArrowPositions2[2] = arrowPointBack;
			TransformedArrowPositions2[3] = arrowPointRight;

			//draw the outline
			g.DrawPolygon(pen, TransformedArrowPositions2);

			//fill the polygon
			using var brush = new SolidBrush(color);
			g.FillPolygon(brush, TransformedArrowPositions2);
		}

		/// <summary>
		/// Arrow shape information, for DrawArrow
		/// </summary>
		private static readonly PointF[] ArrowPositions = new PointF[]
		{
			new(4.0f, 0.0f),
			new(0.0f, 4.0f),
			new(0.0f, -4.0f),
		};

		/// <summary>
		/// A set of points to save the transformed arrow shape info in.
		/// </summary>
		private static readonly PointF[] TransformedArrowPositions =
			new PointF[] { new(), new(), new() };

		/// <summary>
		/// Draws an fancy arrow from a given point rotated by the given angle, scaled by the length.
		/// This, as opposed to <see cref="DrawArrow2(Graphics, PointF, PointF, Color, float)"/>,
		/// is intended more as a use for drawing a tangent/normal/ray, rather than an arrow with a specific start/stop.
		/// </summary>
		/// <param name="g">The graphics device to draw with.</param>
		/// <param name="origin">Where the arrow should start.</param>
		/// <param name="angle">The angle it should point.</param>
		/// <param name="length">How long (in pixels) should the arrow be?</param>
		/// <param name="color">The color to draw the arrow with.</param>
		public static void DrawArrow(this Graphics g, PointF origin, float angle, float length, Color color)
		{
			using var pen = new Pen(color);
			using var brush = new SolidBrush(color);

			float sin = (float)System.Math.Sin(angle);
			float cos = (float)System.Math.Cos(angle);

			var pt = new PointF(origin.X + (cos * length), origin.Y - (sin * length));
			g.DrawLine(pen, origin, pt);

			// transform points.
			for (int i = 0; i < ArrowPositions.Length; ++i)
			{
				TransformedArrowPositions[i].X =
					pt.X + ((cos * ArrowPositions[i].X) - (sin * ArrowPositions[i].Y));
				TransformedArrowPositions[i].Y =
					pt.Y - ((sin * ArrowPositions[i].X) + (cos * ArrowPositions[i].Y));
			}

			g.FillPolygon(brush, TransformedArrowPositions);
		}

		#endregion Arrow Drawing

		#region String Format

		/// <summary>
		/// Converts a <see cref="ContentAlignment"/> into a pair of <see cref="StringAlignment"/>s.
		/// </summary>
		/// <param name="alignment">The <see cref="ContentAlignment"/> to convert.</param>
		/// <returns>A tuple representing the horizontal and vertical <see cref="StringAlignment"/>s</returns>
		public static (StringAlignment Horizontal, StringAlignment Vertical) ToStringAlignments(this ContentAlignment alignment)
		{
			return alignment switch
			{
				ContentAlignment.TopLeft => (StringAlignment.Near, StringAlignment.Near),
				ContentAlignment.TopCenter => (StringAlignment.Center, StringAlignment.Near),
				ContentAlignment.TopRight => (StringAlignment.Far, StringAlignment.Near),
				ContentAlignment.MiddleLeft => (StringAlignment.Near, StringAlignment.Center),
				ContentAlignment.MiddleCenter => (StringAlignment.Center, StringAlignment.Center),
				ContentAlignment.MiddleRight => (StringAlignment.Far, StringAlignment.Center),
				ContentAlignment.BottomLeft => (StringAlignment.Near, StringAlignment.Far),
				ContentAlignment.BottomCenter => (StringAlignment.Center, StringAlignment.Far),
				ContentAlignment.BottomRight => (StringAlignment.Far, StringAlignment.Far),
				_ => throw new InvalidEnumArgumentException($"Unexpected {nameof(ContentAlignment)} value: {alignment}."),
			};
		}
		/// <summary>
		/// Converts a <see cref="ContentAlignment"/> into a pair of alignments.
		/// </summary>
		/// <param name="alignment">The <see cref="ContentAlignment"/> to convert.</param>
		/// <returns>A tuple representing the <see cref="HorizontalAlignment"/> and <see cref="VerticalAlignment"/>.</returns>
		public static (HorizontalAlignment Horizontal, VerticalAlignment Vertical) ToAlignments(this ContentAlignment alignment)
		{
			return alignment switch
			{
				ContentAlignment.TopLeft => (HorizontalAlignment.Left, VerticalAlignment.Top),
				ContentAlignment.TopCenter => (HorizontalAlignment.Center, VerticalAlignment.Top),
				ContentAlignment.TopRight => (HorizontalAlignment.Right, VerticalAlignment.Top),
				ContentAlignment.MiddleLeft => (HorizontalAlignment.Left, VerticalAlignment.Center),
				ContentAlignment.MiddleCenter => (HorizontalAlignment.Center, VerticalAlignment.Center),
				ContentAlignment.MiddleRight => (HorizontalAlignment.Right, VerticalAlignment.Center),
				ContentAlignment.BottomLeft => (HorizontalAlignment.Left, VerticalAlignment.Bottom),
				ContentAlignment.BottomCenter => (HorizontalAlignment.Center, VerticalAlignment.Bottom),
				ContentAlignment.BottomRight => (HorizontalAlignment.Right, VerticalAlignment.Bottom),
				_ => throw new InvalidEnumArgumentException($"Unexpected {nameof(ContentAlignment)} value: {alignment}."),
			};
		}

		/// <summary>
		/// Converts a <see cref="StringFormat"/> into a <see cref="ContentAlignment"/>
		/// </summary>
		/// <param name="format">The <see cref="StringFormat"/> to convert.</param>
		/// <returns>
		/// A <see cref="ContentAlignment"/> representing the <see cref="StringFormat.Alignment"/>
		/// and <see cref="StringFormat.LineAlignment"/> values.
		/// </returns>
		public static ContentAlignment GetContentAlignment(this StringFormat format)
		{
			return format.Alignment switch
			{
				StringAlignment.Near when format.LineAlignment == StringAlignment.Near => ContentAlignment.TopLeft,
				StringAlignment.Center when format.LineAlignment == StringAlignment.Near => ContentAlignment.TopCenter,
				StringAlignment.Far when format.LineAlignment == StringAlignment.Near => ContentAlignment.TopRight,
				StringAlignment.Near when format.LineAlignment == StringAlignment.Center => ContentAlignment.MiddleLeft,
				StringAlignment.Center when format.LineAlignment == StringAlignment.Center => ContentAlignment.MiddleCenter,
				StringAlignment.Far when format.LineAlignment == StringAlignment.Center => ContentAlignment.MiddleRight,
				StringAlignment.Near when format.LineAlignment == StringAlignment.Far => ContentAlignment.BottomLeft,
				StringAlignment.Center when format.LineAlignment == StringAlignment.Far => ContentAlignment.BottomCenter,
				StringAlignment.Far when format.LineAlignment == StringAlignment.Far => ContentAlignment.BottomRight,
				_ => throw new InvalidEnumArgumentException($"{nameof(format)} had unexpected alignment enum values."),
			};
		}

		/// <summary>
		/// Applies a <see cref="ContentAlignment"/> value to the <see cref="StringFormat.Alignment"/>
		/// and <see cref="StringFormat.LineAlignment"/> properties.
		/// </summary>
		/// <param name="format">The <see cref="StringFormat"/> to modify.</param>
		/// <param name="alignment">The <see cref="ContentAlignment"/> to apply.</param>
		/// <returns>The same, but modified <see cref="StringFormat"/></returns>
		public static StringFormat SetAlignments(this StringFormat format, ContentAlignment alignment)
		{
			(format.Alignment, format.LineAlignment) = alignment.ToStringAlignments();
			return format;
		}

		/// <summary>
		/// Updates a <see cref="StringFormat"/>'s flags to properly reflect
		/// the given <see cref="Control"/>'s <see cref="Control.RightToLeft"/> setting.
		/// </summary>
		/// <param name="format">The format to update.</param>
		/// <param name="c">The control's setting to use.</param>
		public static void UpdateRightToLeft(this StringFormat format, Control c)
		{
			switch (c.RightToLeft)
			{
				case RightToLeft.Yes:
					format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
					break;
				case RightToLeft.No:
					format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
					break;
				default:
					format.UpdateRightToLeft(c.Parent);
					break;
			}
		}

		#endregion String Format

		#endregion Text
	}
}
