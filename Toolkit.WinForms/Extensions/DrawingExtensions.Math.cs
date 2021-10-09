using System.Drawing;

using static System.Math;
using static cmdwtf.Toolkit.Math;

using Image = System.Drawing.Image;

namespace cmdwtf.Toolkit.WinForms.Extensions
{
	/// <summary>
	/// Math tools for tasks related to drawing, and the structs in the
	/// <see cref="System.Drawing"/> namespace.
	/// </summary>
	public static partial class DrawingExtensions
	{
		/// <summary>
		/// Creates a rectangle based on two points.
		/// </summary>
		/// <param name="point1">Point 1</param>
		/// <param name="point2">Point 2</param>
		/// <returns>A rectangle with both points inside it.</returns>
		public static RectangleF GetRectangle(PointF point1, PointF point2)
		{
			float top = Min(point1.Y, point2.Y);
			float bottom = Max(point1.Y, point2.Y);
			float left = Min(point1.X, point2.X);
			float right = Max(point1.X, point2.X);

			var rect = RectangleF.FromLTRB(left, top, right, bottom);

			return rect;
		}

		/// <summary>
		/// Creates a rectangle based on two points.
		/// </summary>
		/// <param name="point1">Point 1</param>
		/// <param name="point2">Point 2</param>
		/// <returns>A rectangle with both points inside it.</returns>
		public static Rectangle GetRectangle(Point point1, Point point2)
		{
			int top = Min(point1.Y, point2.Y);
			int bottom = Max(point1.Y, point2.Y);
			int left = Min(point1.X, point2.X);
			int right = Max(point1.X, point2.X);

			var rect = Rectangle.FromLTRB(left, top, right, bottom);

			return rect;
		}

		/// <summary>
		/// Gets a <see cref="Point"/> representing the top left corner of the
		/// given <see cref="Rectangle"/>.
		/// </summary>
		/// <param name="rectangle">The rectangle to get the point from.</param>
		/// <returns>A <see cref="Point"/> representing the top left of the <see cref="Rectangle"/></returns>
		public static Point TopLeft(this Rectangle rectangle) => new(rectangle.Left, rectangle.Top);

		/// <summary>
		/// Gets a <see cref="Point"/> representing the top right corner of the
		/// given <see cref="Rectangle"/>.
		/// </summary>
		/// <param name="rectangle">The rectangle to get the point from.</param>
		/// <returns>A <see cref="Point"/> representing the top right of the <see cref="Rectangle"/></returns>
		public static Point TopRight(this Rectangle rectangle) => new(rectangle.Right, rectangle.Top);

		/// <summary>
		/// Gets a <see cref="Point"/> representing the bottom left corner of the
		/// given <see cref="Rectangle"/>.
		/// </summary>
		/// <param name="rectangle">The rectangle to get the point from.</param>
		/// <returns>A <see cref="Point"/> representing the bottom left of the <see cref="Rectangle"/></returns>
		public static Point BottomLeft(this Rectangle rectangle) => new(rectangle.Left, rectangle.Bottom);

		/// <summary>
		/// Gets a <see cref="Point"/> representing the bottom right corner of the
		/// given <see cref="Rectangle"/>.
		/// </summary>
		/// <param name="rectangle">The rectangle to get the point from.</param>
		/// <returns>A <see cref="Point"/> representing the bottom right of the <see cref="Rectangle"/></returns>
		public static Point BottomRight(this Rectangle rectangle) => new(rectangle.Right, rectangle.Bottom);



		/// <summary>
		/// Gets a <see cref="PointF"/> representing the top left corner of the
		/// given <see cref="RectangleF"/>.
		/// </summary>
		/// <param name="rectangle">The rectangle to get the point from.</param>
		/// <returns>A <see cref="PointF"/> representing the top left of the <see cref="RectangleF"/></returns>
		public static PointF TopLeft(this RectangleF rectangle) => new(rectangle.Left, rectangle.Top);

		/// <summary>
		/// Gets a <see cref="PointF"/> representing the top right corner of the
		/// given <see cref="RectangleF"/>.
		/// </summary>
		/// <param name="rectangle">The rectangle to get the point from.</param>
		/// <returns>A <see cref="PointF"/> representing the top right of the <see cref="RectangleF"/></returns>
		public static PointF TopRight(this RectangleF rectangle) => new(rectangle.Right, rectangle.Top);

		/// <summary>
		/// Gets a <see cref="PointF"/> representing the bottom left corner of the
		/// given <see cref="RectangleF"/>.
		/// </summary>
		/// <param name="rectangle">The rectangle to get the point from.</param>
		/// <returns>A <see cref="PointF"/> representing the bottom left of the <see cref="RectangleF"/></returns>
		public static PointF BottomLeft(this RectangleF rectangle) => new(rectangle.Left, rectangle.Bottom);

		/// <summary>
		/// Gets a <see cref="PointF"/> representing the bottom right corner of the
		/// given <see cref="RectangleF"/>.
		/// </summary>
		/// <param name="rectangle">The rectangle to get the point from.</param>
		/// <returns>A <see cref="PointF"/> representing the bottom right of the <see cref="RectangleF"/></returns>
		public static PointF BottomRight(this RectangleF rectangle) => new(rectangle.Right, rectangle.Bottom);

		/// <summary>
		/// Adds two <see cref="Point"/>s together.
		/// </summary>
		/// <param name="point1">First point.</param>
		/// <param name="point2">Second point.</param>
		/// <returns></returns>
		public static Point Add(this Point point1, Point point2)
			=> new(point1.X + point2.X, point1.Y + point2.Y);

		/// <summary>
		/// Subtracts point2 from point 1.
		/// </summary>
		/// <param name="point2"></param>
		/// <param name="point1"></param>
		/// <returns></returns>
		public static Point Subtract(this Point point2, Point point1)
			=> new(point2.X - point1.X, point2.Y - point1.Y);


		/// <summary>
		/// Gets the distance between two <see cref="Point"/>s.
		/// </summary>
		/// <param name="point1">The first point.</param>
		/// <param name="point2">The second point.</param>
		/// <returns>The distance between the two points.</returns>
		public static double DistanceFrom(this Point point1, Point point2)
			=> Distance(point1.X, point1.Y, point2.X, point2.Y);


		/// <summary>
		/// Adds a specified size to a rectangle, and returns the new rectangle.
		/// </summary>
		/// <param name="rect">The base rectangle.</param>
		/// <param name="size">The width and height to add.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle Add(this Rectangle rect, Size size)
			=> new(rect.Location, Size.Add(rect.Size, size));

		/// <summary>
		/// Adds a specified height and width to a rectangle, and returns the new rectangle.
		/// </summary>
		/// <param name="rect">The base rectangle.</param>
		/// <param name="width">The width to add.</param>
		/// <param name="height">The height to add.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle Add(this Rectangle rect, int width, int height)
			=> new(rect.Location, new Size(rect.Width + width, rect.Height + height));

		/// <summary>
		/// Removes a specified size to a rectangle, and returns the new rectangle.
		/// </summary>
		/// <param name="rect">The base rectangle.</param>
		/// <param name="size">The width and height to remove.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle Subtract(this Rectangle rect, Size size)
			=> new(rect.Location, Size.Subtract(rect.Size, size));

		/// <summary>
		/// Removes a specified height and width from a rectangle, and returns the new rectangle.
		/// </summary>
		/// <param name="rect">The base rectangle.</param>
		/// <param name="width">The width to remove.</param>
		/// <param name="height">The height to remove.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle Subtract(this Rectangle rect, int width, int height)
			=> new(rect.Location, new Size(rect.Width - width, rect.Height - height));

		/// <summary>
		/// An alias for getting the shortest side of the rectangle.
		/// </summary>
		/// <param name="rect">The rectangle to check.</param>
		/// <returns>A float representing the length of the shortest side.</returns>
		public static float ShortestSide(this RectangleF rect)
			=> Min(rect.Width, rect.Height);

		/// <inheritdoc cref="ShortestSide(RectangleF)"/>
		public static int ShortestSide(this Rectangle rect)
			=> Min(rect.Width, rect.Height);

		/// <summary>
		/// Returns true if a size has a width and height greater than zero,
		/// and both are actual numbers.
		/// </summary>
		/// <param name="size">The size to check.</param>
		/// <returns>True, if the size is valid.</returns>
		public static bool IsValidSize(this SizeF size)
			=> !size.IsEmpty
			&& !float.IsNaN(size.Width)
			&& !float.IsNaN(size.Height);

		/// <inheritdoc cref="IsValidSize(SizeF)"/>
		public static bool IsValidSize(this Size size)
			=> !size.IsEmpty;

		/// <summary>
		/// Returns a value if the given content alignment is in the given mask.
		/// </summary>
		/// <param name="alignment">The alignment to check.</param>
		/// <param name="mask">The mask to check against.</param>
		/// <returns><c>true</c>, if the alignment value is in the mask; otherwise <c>false</c>.</returns>
		public static bool Is(this ContentAlignment alignment, ContentAlignment mask)
			=> ((alignment & mask) != 0);

		/// <summary>
		/// Creates a rectangle with the given size inside the outer rectangle with the specified alignment.
		/// </summary>
		/// <param name="outer">The rectangle to place the new rectangle in.</param>
		/// <param name="inner">The size of the inner rectangle to create.</param>
		/// <param name="align">The alignment the new rectangle should have inside the outer rectangle.</param>
		/// <returns>The rectangle, placed inside <paramref name="outer"/>.</returns>
		internal static Rectangle AlignInRectangle(Rectangle outer, Size inner, ContentAlignment align)
		{
			int x = 0;
			int y = 0;

			if (align.Is(ContentAlignmentMasks.AnyLeftAlign))
			{
				x = outer.X;
			}
			else if (align.Is(ContentAlignmentMasks.AnyCenterAlign))
			{
				x = Max(outer.X + ((outer.Width - inner.Width) / 2), outer.Left);
			}
			else if (align.Is(ContentAlignmentMasks.AnyRightAlign))
			{
				x = outer.Right - inner.Width;
			}

			if (align.Is(ContentAlignmentMasks.AnyTopAlign))
			{
				y = outer.Y;
			}
			else if (align.Is(ContentAlignmentMasks.AnyMiddleAlign))
			{
				y = outer.Y + ((outer.Height - inner.Height) / 2);
			}
			else if (align.Is(ContentAlignmentMasks.AnyBottomAlign))
			{
				y = outer.Bottom - inner.Height;
			}

			return new Rectangle(x, y, Min(inner.Width, outer.Width), Min(inner.Height, outer.Height));
		}

		/// <summary>
		/// Determines the size and location of the given <see cref="Size"/>
		/// within the <see cref="Rectangle"/> based on the alignment.
		/// </summary>
		/// <param name="outer">The rectangle to place the new rectangle in.</param>
		/// <param name="inner">The size of the inner rectangle to create.</param>
		/// <param name="align">The alignment the new rectangle should have inside the outer rectangle.</param>
		/// <returns>The rectangle, placed inside <paramref name="outer"/>.</returns>
		public static Rectangle CalculateAlignedRect(this Size inner, Rectangle outer, ContentAlignment align)
			=> Rectangle.Round(CalculateAlignedRect(inner, outer, align));

		/// <summary>
		/// Determines the size and location of the given <see cref="SizeF"/>
		/// within the <see cref="RectangleF"/> based on the alignment.
		/// </summary>
		/// <param name="outer">The rectangle to place the new rectangle in.</param>
		/// <param name="inner">The size of the inner rectangle to create.</param>
		/// <param name="align">The alignment the new rectangle should have inside the outer rectangle.</param>
		/// <returns>The rectangle, placed inside <paramref name="outer"/>.</returns>
		public static RectangleF CalculateAlignedRect(this SizeF inner, RectangleF outer, ContentAlignment align)
		{
			float x = outer.X + 2;

			if ((align & ContentAlignmentMasks.AnyRightAlign) != 0)
			{
				x = outer.X + outer.Width - 4 - inner.Width;
			}
			else if ((align & ContentAlignmentMasks.AnyCenterAlign) != 0)
			{
				x = outer.X + ((outer.Width - inner.Width) / 2);
			}

			float y = (align & ContentAlignmentMasks.AnyBottomAlign) == 0
				? ((align & ContentAlignmentMasks.AnyTopAlign) == 0
					? outer.Y + ((outer.Height - inner.Height) / 2)
					: outer.Y + 2)
				: outer.Y + outer.Height - 4 - inner.Height;

			return new RectangleF(x, y, inner.Width, inner.Height);
		}

		/// <summary>
		/// Determines the size and location of an image drawn within the <see cref="Rectangle"/> based on the alignment.
		/// </summary>
		/// <param name="image">The <see cref="Image"/> used to determine size and location when drawn within the rectangle.</param>
		/// <param name="r">A <see cref="Rectangle"/> that represents the area to draw the image in.</param>
		/// <param name="align">The alignment of content within the rectangle.</param>
		/// <returns>A <see cref="Rectangle"/> that represents the size and location of the specified image should draw into.</returns>
		/// <remarks>
		/// Licensed under the MIT license.
		/// Copyright ©️ 2021 Chris Marc Dailey (nitz)
		/// Copyright (c) .NET Foundation and Contributors
		/// <see href="https://github.com/dotnet/winforms/blob/441b19a3bf7ede62c103250fd698f5c2490c4006/src/System.Windows.Forms/src/System/Windows/Forms/Label.cs">Label.cs</see>
		/// </remarks>
		public static RectangleF CalcImageRenderBounds(this Image image, RectangleF r, ContentAlignment align)
			=> CalculateAlignedRect(image.Size, r, align);

		/// <inheritdoc cref="CalcImageRenderBounds(Image, RectangleF, ContentAlignment)"/>
		public static Rectangle CalcImageRenderBounds(this Image image, Rectangle r, ContentAlignment align)
			=> Rectangle.Round(image.CalcImageRenderBounds((RectangleF)r, align));

		/// <summary>
		/// A few shorthands for <see cref="ContentAlignment"/> flag sets.
		/// </summary>
		/// <remarks>
		/// Licensed under the MIT license.
		/// Copyright ©️ 2021 Chris Marc Dailey (nitz)
		/// Copyright (c) .NET Foundation and Contributors
		/// <see href="https://github.com/dotnet/winforms/blob/441b19a3bf7ede62c103250fd698f5c2490c4006/src/System.Windows.Forms/src/System/Windows/Forms/WinFormsUtils.cs">WinFormsUtils.cs</see>
		/// </remarks>
		public static class ContentAlignmentMasks
		{
			/// <summary>
			/// Any right <see cref="ContentAlignment"/>.
			/// </summary>
			public const ContentAlignment AnyRightAlign = ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight;
			/// <summary>
			/// Any left <see cref="ContentAlignment"/>.
			/// </summary>
			public const ContentAlignment AnyLeftAlign = ContentAlignment.TopLeft | ContentAlignment.MiddleLeft | ContentAlignment.BottomLeft;
			/// <summary>
			/// Any top <see cref="ContentAlignment"/>.
			/// </summary>
			public const ContentAlignment AnyTopAlign = ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight;
			/// <summary>
			/// Any bottom <see cref="ContentAlignment"/>.
			/// </summary>
			public const ContentAlignment AnyBottomAlign = ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight;
			/// <summary>
			/// Any middle <see cref="ContentAlignment"/>.
			/// </summary>
			public const ContentAlignment AnyMiddleAlign = ContentAlignment.MiddleLeft | ContentAlignment.MiddleCenter | ContentAlignment.MiddleRight;
			/// <summary>
			/// Any center <see cref="ContentAlignment"/>.
			/// </summary>
			public const ContentAlignment AnyCenterAlign = ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter;
		}
	}
}
