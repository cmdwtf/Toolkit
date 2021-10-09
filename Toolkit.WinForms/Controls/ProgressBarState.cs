
using System.ComponentModel;

using static cmdwtf.Toolkit.WinForms.Native.ComCtl32.ProgressBar;

namespace cmdwtf.Toolkit.WinForms.Controls
{
	/// <summary>
	/// Progress bar states.
	/// </summary>
	public enum ProgressBarState
	{
		/// <summary>
		/// An unknown progress bar state. Do not attempt to set this
		/// value directly. This is the value returned if a control's handle
		/// hasn't been created and the user asks for the state.
		/// </summary>
		[Browsable(false)]
		Unknown = 0,
		/// <summary>
		/// The normal progress bar state. Typically displays as green.
		/// </summary>
		Normal = PBST.NORMAL,
		/// <summary>
		/// The error progress bar state. Typically displays as red.
		/// </summary>
		Error = PBST.ERROR,
		/// <summary>
		/// The paused progress bar state. Typically displays as yellow.
		/// </summary>
		Paused = PBST.PAUSED,
	}
}
