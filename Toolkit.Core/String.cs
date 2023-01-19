using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// Tools for manipulating or formating strings.
	/// </summary>
	public static class String
	{
		private const int DefaultByteStringDigits = 2;
		private const double ByteOrderOfMagnitudeStandard = 1024d;
		private const double ByteOrderOfMagnitudeSI = 1000d;
		private static readonly string[] BytesSuffixStandard = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
		private static readonly string[] BytesSuffixModern = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" };

		/// <summary>
		/// Turns an IEnumerable of strings into a <see cref="StringCollection"/>.
		/// Some older APIs only take StringCollections.
		/// </summary>
		/// <param name="enumerable">The enumerable to convert.</param>
		/// <returns>A string collection of the given strings.</returns>
		public static StringCollection ToStringCollection(this IEnumerable<string> enumerable)
		{
			var collection = new StringCollection();
			collection.AddRange(enumerable.ToArray());
			return collection;
		}

		/// <summary>
		/// Converts a byte count to a human readable string, using classic (a.k.a.: "SI") units
		/// and a base of 1024.
		/// </summary>
		/// <param name="byteCount">The amount of bytes</param>
		/// <returns>The human-readable string.</returns>
		public static string ByteCountToHumanReadableString(long byteCount)
			=> ByteCountToHumanReadableString(byteCount, DefaultByteStringDigits);

		/// <summary>
		/// Converts a byte count to a human readable string, using classic (a.k.a.: "SI") units
		/// and a base of 1024.
		/// </summary>
		/// <param name="byteCount">The amount of bytes</param>
		/// <param name="digits">The number of post-decimal place digits to include.</param>
		/// <returns>The human-readable string.</returns>
		public static string ByteCountToHumanReadableString(long byteCount, int digits) =>
			ByteCountToHumanReadableInternal(byteCount, digits, ByteOrderOfMagnitudeStandard, BytesSuffixStandard);

		/// <summary>
		/// Converts a byte count to a human readable string, using "SI" (e.g.: "MB", "GB") units
		/// and a base of 1000.
		/// </summary>
		/// <param name="byteCount">The amount of bytes</param>
		/// <param name="digits">The number of post-decimal place digits to include.</param>
		/// <returns>The human-readable string.</returns>
		public static string ByteCountToHumanReadableStringSi(long byteCount, int digits = DefaultByteStringDigits) =>
			ByteCountToHumanReadableInternal(byteCount, digits, ByteOrderOfMagnitudeSI, BytesSuffixStandard);

		/// <summary>
		/// Converts a byte count to a human readable string, using modern (e.g.: "MiB", "GiB") units
		/// and a base of 1024.
		/// </summary>
		/// <param name="byteCount">The amount of bytes</param>
		/// <param name="digits">The number of post-decimal place digits to include.</param>
		/// <returns>The human-readable string.</returns>
		public static string ByteCountToHumanReadableStringModern(long byteCount, int digits = DefaultByteStringDigits) =>
			ByteCountToHumanReadableInternal(byteCount, digits, ByteOrderOfMagnitudeStandard, BytesSuffixModern);

		// function to build human readable byte string based on user choices from overloads
		private static string ByteCountToHumanReadableInternal(long byteCount, int digits, double magnitudeScale, string[] suffixes)
		{
			if (byteCount == 0)
			{
				return $"0 {suffixes[0]}";
			}

			(int magnitude, double value) = ConvertByteCountToBase(byteCount, magnitudeScale, digits);
			magnitude = magnitude.Clamp(0, suffixes.Length - 1);
			return $"{value} {suffixes[magnitude]}";
		}

		// logarithmic function to calculate the order of magnitude and remaining value of a number of bytes
		private static (int Magnitude, double Value) ConvertByteCountToBase(long byteCount, double magnitudeScale, int digits = 2)
		{
			long bytesAbsolute = System.Math.Abs(byteCount);
			int sign = System.Math.Sign(byteCount);
			int magnitude = Convert.ToInt32(System.Math.Floor(System.Math.Log(bytesAbsolute, magnitudeScale)));
			double num = System.Math.Round(bytesAbsolute / System.Math.Pow(magnitudeScale, magnitude), digits);

			return (magnitude, num * sign);
		}

#if !NETSTANDARD2_1_OR_GREATER

		/// <inheritdoc cref="string.Contains(string)"/>
		internal static bool Contains(this string s, char value)
			=> s.Contains(value.ToString());

#if NET5_0_OR_GREATER

		/// <inheritdoc cref="string.Contains(string, StringComparison)"/>
		internal static bool Contains(this string s, char value, StringComparison comparisonType)
			=> s.Contains(value.ToString(), comparisonType);

#endif // NET5_0_OR_GREATER

#endif // !NETSTANDARD2_1_OR_GREATER
	}
}
