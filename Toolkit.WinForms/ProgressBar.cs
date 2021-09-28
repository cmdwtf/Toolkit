
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
			Unknown = 0,
			Normal = PBST.NORMAL,
			Error = PBST.ERROR,
			Paused = PBST.PAUSED,
		}

		/// <summary>
		/// Gets the current state of the progress bar.
		/// </summary>
		/// <param name="progressBar"></param>
		/// <returns></returns>
		public static State GetState(this SWFProgressBar progressBar)
		{
			int state = Native.User32.SendMessage(progressBar.Handle, PBM.GETSTATE, UIntPtr.Zero, IntPtr.Zero);
			return (State)state;
		}

		/// <summary>
		/// Sets the progress bar's state.
		/// </summary>
		/// <param name="progressBar"></param>
		/// <param name="state"></param>
		/// <returns></returns>
		public static int SetState(this SWFProgressBar progressBar, State state)
		{
			UIntPtr wParam = new((uint)state);
			return Native.User32.SendMessage(progressBar.Handle, PBM.SETSTATE, wParam, IntPtr.Zero);
		}
	}
}
