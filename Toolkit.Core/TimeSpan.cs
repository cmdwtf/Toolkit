
using STimeSpan = System.TimeSpan;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// Extensions to get a little more millage out of <see cref="System.TimeSpan" />.
	/// </summary>
	public static class TimeSpan
	{

		/// <summary>
		/// Gets the percentage of how much the a partial span is of a full time span.
		/// </summary>
		/// <param name="partialTime">The partial time.</param>
		/// <param name="fullTime">The full amount of time.</param>
		/// <returns>A value representing the percentage normalized from 0m to 1m. Unclamped.</returns>
		public static decimal GetPercentageOf(this STimeSpan partialTime, STimeSpan fullTime)
		{
			if (fullTime.TotalMilliseconds == 0)
			{
				return 0m;
			}

			return (decimal)(partialTime.TotalMilliseconds / fullTime.TotalMilliseconds);
		}

		/// <summary>
		/// Gets a <see cref="System.TimeSpan"/>
		/// </summary>
		/// <param name="fullTime">The span to get the percentage of.</param>
		/// <param name="percentage">What percent of the full time should be returned.</param>
		/// <param name="roundTo">A timespan representing the nearast value the result should round to. Optional.</param>
		/// <returns>A timespan that is a percentage of the full time.</returns>
		public static STimeSpan GetPercentage(this STimeSpan fullTime, decimal percentage, STimeSpan? roundTo = null)
		{
			long totalTicks = fullTime.Ticks;
			var result = STimeSpan.FromTicks((long)(totalTicks * percentage));

			if (roundTo != null)
			{
				return result.RoundToNearest(roundTo.Value);
			}

			return result;
		}

		/// <summary>
		/// Gets a <see cref="System.TimeSpan"/>
		/// </summary>
		/// <param name="span">The span to get the percentage of.</param>
		/// <param name="percentage">What percent of the full time should be returned.</param>
		/// <param name="roundTo">A timespan representing the nearast value the result should round to. Optional.</param>
		/// <returns>A timespan that is a percentage of the full time.</returns>
		/// <remarks>
		/// this is not just calling the <see cref="GetPercentage(STimeSpan, decimal, STimeSpan?)"/>
		/// because double and decimal have different precisions.
		/// </remarks>
		public static STimeSpan GetPercentage(this STimeSpan span, double percentage, STimeSpan? roundTo = null)
		{
			long totalTicks = span.Ticks;
			var result = STimeSpan.FromTicks((long)(totalTicks * percentage));

			if (roundTo != null)
			{
				return result.RoundToNearest(roundTo.Value);
			}

			return result;
		}

		/// <summary>
		/// Rounds a timespan to the nearest value based on the given round amount.
		/// </summary>
		/// <param name="span">The timespan to round.</param>
		/// <param name="roundTo">How wide to round.</param>
		/// <returns>The rounded timespan.</returns>
		public static STimeSpan RoundToNearest(this STimeSpan span, STimeSpan roundTo)
		{
			long ticks = (long)(System.Math.Round(span.Ticks / (double)roundTo.Ticks) * roundTo.Ticks);
			return new STimeSpan(ticks);
		}

		/// <summary>
		/// Formats a string from a given timespan to a nice, human readable,
		/// based on the most prominent unit set in the span.
		/// </summary>
		/// <param name="span">The timespan to format.</param>
		/// <returns>A pleasant string.</returns>
		public static string ToHumanReadableString(this STimeSpan span)
		{
			if (span.TotalSeconds <= 1)
			{
				return $@"{span:s\.ff} second{(span.Milliseconds != 1 ? "s" : "")}";
			}
			if (span.TotalMinutes <= 1)
			{
				return $@"{span:%s} second{(span.Seconds != 1 ? "s" : "")}";
			}
			if (span.TotalHours <= 1)
			{
				return $@"{span:%m} minute{(span.Minutes != 1 ? "s" : "")}";
			}
			if (span.TotalDays <= 1)
			{
				return $@"{span:%h} hour{(span.Hours != 1 ? "s" : "")}";
			}

			// #TODO: Months/Years support.

			return $@"{span:%d} day{(span.Days != 1 ? "s" : "")}";
		}
	}
}
