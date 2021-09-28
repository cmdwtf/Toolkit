using System;
using System.ComponentModel;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// A class that is just a <see cref="System.Windows.Forms.TreeView"/>, but doubled buffered.
	/// </summary>
	[ToolboxItem(true)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Matching Win32 symbols.")]
	public class TreeView : System.Windows.Forms.TreeView
	{
		/// <summary>
		/// <see cref="System.Windows.Forms.TreeView"/> doesn't provide the
		/// ability to set double buffered like most controls, as
		/// <see cref="System.Windows.Forms.TreeView.DoubleBuffered"/> does not
		/// affect the control. Instead, we react to the creation of the native
		/// control's handle, and send the windows message to tell the native
		/// control to enable the double buffered mode.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs"/> containing the event data.</param>
		protected override void OnHandleCreated(EventArgs e)
		{
			_ = Native.User32.SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
			base.OnHandleCreated(e);
		}

		private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
		//private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
		private const int TVS_EX_DOUBLEBUFFER = 0x0004;
	}
}

