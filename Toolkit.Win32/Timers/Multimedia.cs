using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using static cmdwtf.Toolkit.Native.Winmm;

namespace cmdwtf.Toolkit.Timers
{
	/// <summary>
	/// A timer based on the multimedia timer API with 1ms precision.
	/// </summary>
	public class Multimedia : IDisposable
	{
		/// <summary>
		/// Creates an instance of the Multimedia Timer.
		/// </summary>
		public Multimedia()
		{
			_callback = new TimerCallbackDelegate(TimerCallbackMethod);
			Resolution = 0;
			Interval = 1;
		}

		/// <summary>
		/// Finalizer.
		/// </summary>
		~Multimedia()
		{
			Dispose(false);
		}

		private const int _eventTypeSingle = 0;
		private const int _eventTypePeriodic = 1;

		private static readonly Task _taskDone = Task.FromResult<object>(null);

		private bool _disposed = false;
		private int _interval;
		private int _resolution;
		private volatile uint _timerId;

		// Hold the timer callback to prevent garbage collection.
		private readonly TimerCallbackDelegate _callback;

		/// <summary>
		/// Event raised each time the timer's interval elapses.
		/// </summary>
		public event EventHandler Elapsed;

		/// <summary>
		/// The period of the timer in milliseconds.
		/// </summary>
		public int Interval
		{
			get
			{
				return _interval;
			}
			set
			{
				CheckDisposed();

				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				_interval = value;
				if (Resolution > Interval)
				{
					Resolution = value;
				}
			}
		}

		/// <summary>
		/// The resolution of the timer in milliseconds. The minimum resolution is 0, meaning highest possible resolution.
		/// </summary>
		public int Resolution
		{
			get
			{
				return _resolution;
			}
			set
			{
				CheckDisposed();

				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				_resolution = value;
			}
		}

		/// <summary>
		/// Gets whether the timer has been started yet.
		/// </summary>
		public bool IsRunning
		{
			get { return _timerId != 0; }
		}

		/// <summary>
		/// Uses the Multimedia timer to delay for a number of milliseconds.
		/// </summary>
		/// <param name="millisecondsDelay">The number of milliseconds to delay.</param>
		/// <param name="token">A token to request cancellation with.</param>
		/// <returns>The task to wait on to delay.</returns>
		public static Task Delay(int millisecondsDelay, CancellationToken token = default)
		{
			if (millisecondsDelay < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(millisecondsDelay), millisecondsDelay, "The value cannot be less than 0.");
			}

			if (millisecondsDelay == 0)
			{
				return _taskDone;
			}

			token.ThrowIfCancellationRequested();

			// allocate an object to hold the callback in the async state.
			object[] state = new object[1];
			var completionSource = new TaskCompletionSource<object>(state);
			TimerCallbackDelegate callback = (uint id, uint msg, ref uint uCtx, uint rsv1, uint rsv2) =>
			{
				// Note we don't need to kill the timer for one-off events.
				completionSource.TrySetResult(null);
			};

			state[0] = callback;
			uint userCtx = 0;

			MMRESULT result = TimeSetEvent((uint)millisecondsDelay, 0, callback, ref userCtx, _eventTypeSingle);

			if (result != MMRESULT.MMSYSERR_NOERROR)
			{
				int error = Marshal.GetLastWin32Error();
				throw new Win32Exception(error);
			}

			return completionSource.Task;
		}

		/// <summary>
		/// Starts the Multimedia timer which will begin raising <see cref="Elapsed"/> events.
		/// </summary>
		public void Start()
		{
			CheckDisposed();

			if (IsRunning)
			{
				return;
			}

			// Event type = 0, one off event
			// Event type = 1, periodic event
			uint userCtx = 0;
			_timerId = 0;

			MMRESULT result = TimeSetEvent((uint)Interval, (uint)Resolution, _callback, ref userCtx, _eventTypePeriodic);

			if (result == 0)
			{
				int error = Marshal.GetLastWin32Error();
				throw new Win32Exception(error);
			}

			_timerId = (uint)result;
		}

		/// <summary>
		/// Stops the Multimedia timer and halts <see cref="Elapsed"/> events.
		/// Note that you may still recieve an already triggered event after calling this method.
		/// </summary>
		public void Stop()
		{
			CheckDisposed();

			if (!IsRunning)
			{
				return;
			}

			StopInternal();
		}

		/// <summary>
		/// Restarts the timer.
		/// </summary>
		public void Restart()
		{
			Stop();
			Start();
		}

		/// <summary>
		/// Kills the time event in the kernel.
		/// </summary>
		private void StopInternal()
		{
			_ = TimeKillEvent(_timerId);
			_timerId = 0;
		}

		/// <summary>
		/// The IDisposoable implementation.
		/// </summary>
		void IDisposable.Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		/// <summary>
		/// The native callback invoked from the kernel timer event.
		/// </summary>
		private void TimerCallbackMethod(uint id, uint msg, ref uint userCtx, uint rsv1, uint rsv2)
			=> Elapsed?.Invoke(this, EventArgs.Empty);


		/// <summary>
		/// Throws an exception if tried to use after the object has disposed.
		/// </summary>
		private void CheckDisposed()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("MultimediaTimer");
			}
		}

		/// <summary>
		/// Internal dispose functionality.
		/// </summary>
		/// <param name="disposing">True, if disposing, false if finalizing.</param>
		private void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			_disposed = true;

			if (IsRunning)
			{
				StopInternal();
			}

			if (disposing)
			{
				Elapsed = null;
			}
		}
	}

}
