
using Stopwatch = System.Diagnostics.Stopwatch;

namespace cmdwtf.Toolkit.Timers
{
	/// <summary>
	/// A simple class representing a countdown timer, based on
	/// <see cref="System.Diagnostics.Stopwatch"/>
	/// </summary>
	public class Countdown
	{
		private readonly Stopwatch _watch = new();
		private readonly float _countdownTime;

		/// <summary>
		/// Creates a new instance of the Countdown class.
		/// </summary>
		/// <param name="countdownTime">The amount of time to count down.</param>
		/// <param name="startNow">If true, start counting immediately.</param>
		public Countdown(float countdownTime, bool startNow = false)
		{
			_countdownTime = countdownTime;

			if (startNow)
			{
				Reset();
				Start();
			}
		}

		/// <summary>
		/// Starts or resumes the countdown timer.
		/// </summary>
		public void Start() => _watch.Start();

		/// <summary>
		/// Pauses the countdown timer.
		/// </summary>
		public void Stop() => _watch.Stop();

		/// <inheritdoc cref="Stop"/>
		public void Pause() => _watch.Stop();

		/// <summary>
		/// Stops and clears the countdown timer to be reused.
		/// </summary>
		public void Reset()
		{
			_watch.Stop();
			_watch.Reset();
		}

		/// <summary>
		/// Is true while the countdown is active.
		/// </summary>
		public bool IsRunning => _watch.IsRunning && !TimeUp;

		/// <summary>
		/// Returns the amount of seconds remaining in the countdown.
		/// </summary>
		public float TimeLeft => Math.Clamp((float)(_countdownTime - _watch.Elapsed.TotalSeconds), 0.0f, float.MaxValue);

		/// <summary>
		/// Is true if the countdown has expired.
		/// </summary>
		public bool TimeUp => TimeLeft <= 0;

		/// <summary>
		/// An integer version of the time remaining.
		/// </summary>
		public int SecondsLeft => (int)TimeLeft;
	}
}
