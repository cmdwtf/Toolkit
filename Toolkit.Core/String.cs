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
		/// Converts a byte count to a human readable string.
		/// </summary>
		/// <param name="byteCount">The amount of bytes</param>
		/// <returns>The human-readable string</returns>
		public static string ByteCountToHumanReadableString(long byteCount)
		{
			string[] suffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

			if (byteCount == 0)
			{
				return $"0 {suffix[0]}";
			}

			long bytes = System.Math.Abs(byteCount);
			int place = Convert.ToInt32(System.Math.Floor(System.Math.Log(bytes, 1024)));
			double num = System.Math.Round(bytes / System.Math.Pow(1024, place), 1);
			return (System.Math.Sign(byteCount) * num).ToString() + $" {suffix[place]}";
		}
	}
}
