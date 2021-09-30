
using System;

using static cmdwtf.Toolkit.WinForms.Native.ComCtl32.ProgressBar;

using SWFProgressBar = System.Windows.Forms.ProgressBar;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Extension methods for <see cref="System.Windows.Forms.ProgressBar"/>
	/// </summary>
	public static class ProgressBar
	{
		/// <summary>
		/// Progress bar states.
		/// </summary>
		public enum State : uint
		{
			/// <summary>
			/// An unknown progress bar state.
			/// </summary>
			Unknown = 0,
			/// <summary>
			/// The normal progress bar state.
			/// </summary>
			Normal = PBST.NORMAL,
			/// <summary>
			/// The error progress bar state.
			/// </summary>
			Error = PBST.ERROR,
			/// <summary>
			/// The paused progress bar state.
			/// </summary>
			Paused = PBST.PAUSED,
		}

		/// <summary>
		/// Gets the current state of the progress bar.
		/// </summary>
		/// <param name="progressBar">The progress bar to get the state of.</param>
		/// <returns>The current progress bar state.</returns>
		public static State GetState(this SWFProgressBar progressBar)
		{
			int state = Native.User32.SendMessage(progressBar.Handle, PBM.GETSTATE, UIntPtr.Zero, IntPtr.Zero);
			return (State)state;
		}

		/// <summary>
		/// Sets the progress bar's state.
		/// </summary>
		/// <param name="progressBar">The progress bar to set the state of.</param>
		/// <param name="state">The desired state.</param>
		/// <returns>The result of the native SendMessage call.</returns>
		public static int SetState(this SWFProgressBar progressBar, State state)
		{
			UIntPtr wParam = new((uint)state);
			return Native.User32.SendMessage(progressBar.Handle, PBM.SETSTATE, wParam, IntPtr.Zero);
		}
	}
}
