using System.Drawing;

using static System.Math;
using static cmdwtf.Toolkit.Math;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Math tools for tasks related to drawing, and the structs in the
	/// <see cref="System.Drawing"/> namespace.
	/// </summary>
	public static class DrawingMath
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
		/// Adds two points together.
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
		/// Gets the distance between two points.
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
		public static float ShortestSide(RectangleF rect)
			=> Min(rect.Width, rect.Height);

		/// <inheritdoc cref="ShortestSide(RectangleF)"/>
		public static int ShortestSide(Rectangle rect)
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
	}
}
