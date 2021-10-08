using System;
using System.Runtime.InteropServices;
using System.Threading;

using static cmdwtf.Toolkit.Native.Winmm;

namespace cmdwtf.Toolkit.Win32
{
	/// <summary>
	/// A straightforward way to use WINMM time periods through
	/// a managed interface.
	/// Via <see href="https://stackoverflow.com/a/24198773">StackOverflow</see>
	/// </summary>
	public sealed class TimePeriod : IDisposable
	{
		private static readonly TIMECAPS _timeCapabilities;

		private static int _inTimePeriod;
		private readonly int _periodMs;
		private int _disposed;


		static TimePeriod()
		{
			int result = TimeGetDevCaps(ref _timeCapabilities, Marshal.SizeOf(typeof(TIMECAPS)));
			if (result != 0)
			{
				throw new InvalidOperationException(
					$"The request to get time capabilities was not completed because an unexpected error with code {result} occurred.");
			}
		}

		internal TimePeriod(int periodMs)
		{
			if (Interlocked.Increment(ref _inTimePeriod) != 1)
			{
				Interlocked.Decrement(ref _inTimePeriod);
				throw new NotSupportedException("The process is already within a time period. Nested time periods are not supported.");
			}

			if (periodMs < _timeCapabilities.wPeriodMin || periodMs > _timeCapabilities.wPeriodMax)
			{
				throw new ArgumentOutOfRangeException(nameof(periodMs), "The request to begin a time period was not completed because the resolution specified is out of range.");
			}

			int result = TimeBeginPeriod(periodMs);
			if (result != 0)
			{
				throw new InvalidOperationException(
					$"The request to begin a time period was not completed because an unexpected error with code {result} occurred.");
			}

			_periodMs = periodMs;
		}

		internal static int MinimumPeriod => _timeCapabilities.wPeriodMin;

		internal static int MaximumPeriod => _timeCapabilities.wPeriodMax;

		internal int PeriodMs
		{
			get
			{
				if (_disposed > 0)
				{
					throw new ObjectDisposedException("The time period instance has been disposed.");
				}

				return _periodMs;
			}
		}

		/// <summary>
		/// Disposes the <see cref="TimePeriod"/> instance,
		/// returning the time period to the original value.
		/// </summary>
		public void Dispose()
		{
			if (Interlocked.Increment(ref _disposed) == 1)
			{
				_ = TimeEndPeriod(_periodMs);
				Interlocked.Decrement(ref _inTimePeriod);
			}
			else
			{
				Interlocked.Decrement(ref _disposed);
			}
		}
	}
}
