using System;
using System.Collections.Generic;

using static System.Math;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// Math utilities, and some simple interpolations.
	/// </summary>
	public static class Math
	{
		// Math.Clamp wasn't there til .NET Standard 2.1... (.NET 5/.NET Core 3.0)
#if !NET5_0_OR_GREATER || !NETCOREAPP3_0_OR_GREATER || !NET471_OR_GREATER
		/// <summary>
		/// Clamps a value between min and max, inclusive.
		/// </summary>
		/// <param name="d">The value to clamp.</param>
		/// <param name="min">The minimum allowable value.</param>
		/// <param name="max">The maximum allowable value.</param>
		/// <returns>The clamped value.</returns>
		public static decimal Clamp(this decimal d, decimal min, decimal max)
		{
			if (d < min)
			{
				return min;
			}
			else if (d > max)
			{
				return max;
			}

			return d;
		}

		/// <inheritdoc cref="Clamp(decimal, decimal, decimal)"/>
		public static double Clamp(this double d, double min, double max)
		{
			if (d < min)
			{
				return min;
			}
			else if (d > max)
			{
				return max;
			}

			return d;
		}

		/// <inheritdoc cref="Clamp(decimal, decimal, decimal)"/>
		public static float Clamp(this float d, float min, float max)
		{
			if (d < min)
			{
				return min;
			}
			else if (d > max)
			{
				return max;
			}

			return d;
		}

		/// <inheritdoc cref="Clamp(decimal, decimal, decimal)"/>
		public static int Clamp(this int i, int min, int max)
		{
			if (i < min)
			{
				return min;
			}
			else if (i > max)
			{
				return max;
			}

			return i;
		}
#endif // !NETSTANDARD2_1_OR_GREATER

		/// <summary>
		/// Linearly interpolates between by the amount specified. The result is unclamped.
		/// This method does *not* guarantee result = value1 when t = 1, due to floating-point arithmetic error.
		/// For a precise method, use <see cref="Lerp(double, double, double)"/> if you need the 0 to 1 precision
		/// instead of the monotonic results.
		/// This version may be preferable when the hardware has a native fused multiply-add instruction.
		/// </summary>
		/// <param name="value0">The start value.</param>
		/// <param name="value1">The end value.</param>
		/// <param name="t">The amount of way to interpolate where 0.0f is the start value, and 1.0f is the end value.</param>
		/// <returns>The interpolated value.</returns>
		/// <remarks>See also: https://en.wikipedia.org/wiki/Linear_interpolation#Programming_language_support </remarks>
		public static double LerpMonotonic(this double value0, double value1, double t)
			=> value0 + ((value1 - value0) * t);

		/// <inheritdoc cref="Lerp(double, double, double)"/>
		public static float LerpMonotonic(this float value0, float value1, float t)
			=> value0 + ((value1 - value0) * t);

		/// <summary>
		/// Linearly interpolates between by the amount specified. The result is unclamped.
		/// This method *does* guarantee result = value1 when t = 1. However, this method is
		/// *only* monotonic when (value0 * value1 &lt; 0). Lerping between the same values might not produce
		/// the same result.
		/// If you need the result to be monotonic, use  <see cref="LerpMonotonic(double, double, double)"/>
		/// instead.
		/// </summary>
		/// <param name="value0">The start value.</param>
		/// <param name="value1">The end value.</param>
		/// <param name="t">The amount of way to interpolate where 0.0f is the start value, and 1.0f is the end value.</param>
		/// <returns>The interpolated value.</returns>
		/// <remarks>See also: https://en.wikipedia.org/wiki/Linear_interpolation#Programming_language_support </remarks>
		public static double Lerp(this double value0, double value1, double t)
			=> ((1 - t) * value0) + (t * value1);

		/// <inheritdoc cref="Lerp(double, double, double)"/>
		public static float Lerp(this float value0, float value1, float t)
			=> ((1 - t) * value0) + (t * value1);

		/// <summary>
		/// Calculates the percent of the way from the minimum to maximum the value is,
		/// assuming a linear interpolation.
		/// </summary>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <param name="value">The value to resolve the percentage of.</param>
		/// <returns>
		/// The percentage of the way the value falls between min and max.
		/// 0.0 => min, 1.0 => max. Numbers less than 0 are less than min,
		/// and numbers greater than 1.0 are greater than max.
		/// </returns>
		public static double InverseLerp(this double min, double max, double value)
		{
			if (Abs(max - min) < double.Epsilon)
			{
				return min;
			}

			return (value - min) / (max - min);
		}

		/// <inheritdoc cref="InverseLerp(double, double, double)"/>
		public static float InverseLerp(float min, float max, float t)
		{
			float distance = max - min;
			float percent = (t - min) / distance;

			if (t < min)
			{
				percent = MakeNegitive(percent);
			}

			return percent;
		}

		/// <inheritdoc cref="InverseLerp(double, double, double)"/>
		public static float PercentOf(float min, float max, float t)
			=> InverseLerp(min, max, t);

		/// <summary>
		/// Returns the given number as a positive value.
		/// </summary>
		/// <param name="f">The number to make positive.</param>
		/// <returns>The number, positive.</returns>
		public static float MakePositive(this float f)
			=> f < 0 ? f * -1 : f;

		/// <summary>
		/// Returns the given number as a negative value.
		/// </summary>
		/// <param name="f">The number to make negative.</param>
		/// <returns>The number, negative.</returns>
		public static float MakeNegitive(this float f)
			=> f > 0 ? f * -1 : f;

		/// <inheritdoc cref="MakePositive(double)"/>
		public static double MakePositive(this double f)
			=> f < 0 ? f * -1 : f;

		/// <inheritdoc cref="MakeNegitive(double)"/>
		public static double MakeNegitive(this double f)
			=> f > 0 ? f * -1 : f;

		/// <summary>
		/// Returns true if the floating point number a is greater than b,
		/// including within the given epsilon.
		/// </summary>
		/// <param name="a">The value a.</param>
		/// <param name="b">The value b.</param>
		/// <param name="epsilon">The epsilon to compare with.</param>
		/// <returns>True, if a &gt; b</returns>
		public static bool GreaterThanEpsilon(float a, float b, float epsilon)
			=> a > b || a + epsilon > b || a - epsilon > b;

		/// <summary>
		/// Returns true if the floating point number a is less than b,
		/// including within the given epsilon.
		/// </summary>
		/// <param name="a">The value a.</param>
		/// <param name="b">The value b.</param>
		/// <param name="epsilon">The epsilon to compare with.</param>
		/// <returns>True, if a &lt; b</returns>
		public static bool LessThanEpsilon(float a, float b, float epsilon)
			=> a < b || a + epsilon < b || a - epsilon < b;

		/// <summary>
		/// Returns true if the floating point number a is greater or equal to b,
		/// including within the given epsilon.
		/// </summary>
		/// <param name="a">The value a.</param>
		/// <param name="b">The value b.</param>
		/// <param name="epsilon">The epsilon to compare with.</param>
		/// <returns>True, if a &gt;= b</returns>
		public static bool GreaterThanEqualEpsilon(float a, float b, float epsilon)
			=> a >= b || a + epsilon >= b || a - epsilon >= b;

		/// <summary>
		/// Returns true if the floating point number a is less or equal to b,
		/// including within the given epsilon.
		/// </summary>
		/// <param name="a">The value a.</param>
		/// <param name="b">The value b.</param>
		/// <param name="epsilon">The epsilon to compare with.</param>
		/// <returns>True, if a &lt;= b</returns>
		public static bool LessThanEqualEpsilon(float a, float b, float epsilon)
			=> a <= b || a + epsilon <= b || a - epsilon <= b;


		/// <summary>
		/// Gets the distance between two points.
		/// </summary>
		/// <param name="x1">Point 1 X</param>
		/// <param name="y1">Point 1 Y</param>
		/// <param name="x2">Point 2 X</param>
		/// <param name="y2">Point 2 Y</param>
		/// <returns>The distance between the two points.</returns>
		public static double Distance(double x1, double y1, double x2, double y2)
			=> Sqrt(Pow(x2 - x1, 2) + Pow(y2 - y1, 2));

		/// <summary>
		/// Calculate the RMS (Root Mean Square or quadratic mean) of a list of values.
		/// </summary>
		/// <param name="values">The values to calculate the RMS of.</param>
		/// <returns>The RMS.</returns>
		public static double RootMeanSquare(this IList<double> values)
			=> values.RootMeanSquare(0, values.Count);

		/// <summary>
		/// Calculate the RMS (Root Mean Square or quadratic mean) of a list of values, with a
		/// specified start and end point in the list.
		/// </summary>
		/// <param name="values">The values to calculate the RMS of.</param>
		/// <param name="start">Where in the list to start looking at values to include in the calculation.</param>
		/// <param name="end">Where in the list to finish looking at values to include in the calculation.</param>
		/// <returns>The RMS.</returns>
		public static double RootMeanSquare(this IList<double> values, int start, int end)
		{
			int range = end - start;
			int count = values.Count;

			if (start < 0)
			{
				throw new ArgumentException("Start must be equal to or greater than 0.", nameof(start));
			}

			if (end >= count)
			{
				throw new ArgumentException("End must be less than the count of items in the list.", nameof(end));
			}

			if (range <= 0)
			{
				throw new ArgumentException("The range of start and end must be greater than zero.");
			}

			if (values.Count < range)
			{
				throw new ArgumentException("The amount of items in the list must be equal to or larger than the specified range.", nameof(values));
			}

			double sum = 0;

			for (int scan = start; scan < end; ++scan)
			{
				sum += Pow(values[scan], 2);
			}

			return Sqrt(sum / range);
		}
	}
}
