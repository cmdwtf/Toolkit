
using STimeSpan = System.TimeSpan;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// Extensions to get a little more millage out of <see cref="System.TimeSpan" />.
	/// </summary>
	public static class TimeSpan
	{
		private const double AverateDaysPerYearGregorian = ((291 * 366) + (909 * 365)) / 1200.0; // 365.2425
		private const double AverageDaysPerYearJulian = ((365 * 3) + 366) / 4.0; // 365.25
		private const double AverageDaysPerMonthGregorian = AverateDaysPerYearGregorian / 12.0; // 30.436875
		private const double AverageDaysPerMonthJulian = AverageDaysPerYearJulian / 12.0; // 30.4375

		/// <summary>
		/// Gets the approximate number of years represented by the
		/// <see cref="STimeSpan"/>.
		/// </summary>
		/// <param name="timespan">The time to inspect.</param>
		/// <param name="useGregorianYear">If <c>true</c>, the calculation will
		/// use the average length of a Gregorian year. Otherwise, Julian.</param>
		/// <returns>A whole number of years.</returns>
		public static int GetApproximateYears(this STimeSpan timespan, bool useGregorianYear = true)
			=> (int)(timespan.Days / (useGregorianYear ? AverateDaysPerYearGregorian : AverageDaysPerYearJulian));

		/// <summary>
		/// Gets the approximate number of months represented by the
		/// <see cref="STimeSpan"/>.
		/// </summary>
		/// <param name="timespan">The time to inspect.</param>
		/// <param name="useGregorianMonth">If <c>true</c>, the calculation will
		/// use the average length of a Gregorian year. Otherwise, Julian.</param>
		/// <returns>A whole number of months.</returns>
		public static int GetApproximateMonths(this STimeSpan timespan, bool useGregorianMonth = true)
			=> (int)(timespan.Days / (useGregorianMonth ? AverageDaysPerMonthGregorian : AverageDaysPerMonthJulian));

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
			int totalMonths = span.GetApproximateMonths();
			int totalYears = span.GetApproximateYears();
			return span switch
			{
				{ TotalSeconds: <= 1 } => $@"{span:s\.ff} second{(span.Milliseconds != 1 ? "s" : "")}",
				{ TotalMinutes: <= 1 } => $@"{span:%s} second{(span.Seconds != 1 ? "s" : "")}",
				{ TotalHours: <= 1 } => $@"{span:%m} minute{(span.Minutes != 1 ? "s" : "")}",
				{ TotalDays: <= 1 } => $@"{span:%h} hour{(span.Hours != 1 ? "s" : "")}",
				_ when totalMonths <= 1 => $@"{span:%d} day{(span.Days != 1 ? "s" : "")}",
				_ when totalYears <= 1 => $@"{totalMonths} month{(totalMonths != 1 ? "s" : "")}",
				_ => $@"{totalYears} year{(totalYears != 1 ? "s" : "")}",
			};
		}
	}
}
