
using System;

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
			Normal = Native.ProgressBar.PBST.NORMAL,
			Error = Native.ProgressBar.PBST.ERROR,
			Paused = Native.ProgressBar.PBST.PAUSED,
		}

		public static int SetState(this SWFProgressBar progressBar, State state)
		{
			UIntPtr wParam = new((uint)state);
			return Native.User32.SendMessage(progressBar.Handle, Native.ProgressBar.PBM.SETSTATE, wParam, IntPtr.Zero);
		}
	}
}
