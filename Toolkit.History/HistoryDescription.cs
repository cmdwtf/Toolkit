using System;

namespace cmdwtf.Toolkit.History
{
	/// <summary>
	/// A class that defines a historical state.
	/// </summary>
	public class HistoryDescription
	{
		/// <summary>
		/// Gets the information.
		/// </summary>
		/// <value>
		/// The information.
		/// </value>
		public string Info { get; private set; }

		/// <summary>
		/// Gets the delta from current.
		/// </summary>
		/// <value>
		/// The delta from current.
		/// </value>
		public int DeltaFromCurrent { get; private set; }

		/// <summary>
		/// Gets the steps from current.
		/// </summary>
		/// <value>
		/// The steps from current.
		/// </value>
		public int StepsFromCurrent => Math.Abs(DeltaFromCurrent);

		/// <summary>
		/// Gets a value indicating whether this instance is the current state.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is the current state; otherwise, <c>false</c>.
		/// </value>
		public bool IsCurrentState => DeltaFromCurrent == 0;

		/// <summary>
		/// Gets a value indicating whether this instance is a previous state.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is a previous state; otherwise, <c>false</c>.
		/// </value>
		public bool IsPreviousState => DeltaFromCurrent < 0;

		/// <summary>
		/// Gets a value indicating whether this instance is a future state.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is a future state; otherwise, <c>false</c>.
		/// </value>
		public bool IsFutureState => DeltaFromCurrent > 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="HistoryDescription"/> class.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <param name="deltaFromCurrent">The positive or negitive number of steps from the current state.</param>
		public HistoryDescription(string info, int deltaFromCurrent)
		{
			Info = info;
			DeltaFromCurrent = deltaFromCurrent;
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="HistoryDescription"/> to <see cref="string"/>.
		/// </summary>
		/// <param name="that">The description to convert.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static implicit operator string(HistoryDescription that)
		{
			if (that.IsCurrentState)
			{
				return $"*{that.Info}";
			}

			//return $"[{that.DeltaFromCurrent}] {that.Info}";
			return $"{that.Info}";
		}
	}
}
