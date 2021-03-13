using System;

namespace cmdwtf.Toolkit.History
{
	public class HistoryDescription
	{
		public string Info { get; private set; }
		public int DeltaFromCurrent { get; private set; }
		public int StepsFromCurrent => Math.Abs(DeltaFromCurrent);
		public bool IsCurrentState => DeltaFromCurrent == 0;
		public bool IsPreviousState => DeltaFromCurrent < 0;
		public bool IsFutureState => DeltaFromCurrent > 0;

		public HistoryDescription(string info, int deltaFromCurrent)
		{
			Info = info;
			DeltaFromCurrent = deltaFromCurrent;
		}

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
