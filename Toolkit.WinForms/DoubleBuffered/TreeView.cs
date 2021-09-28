using System;
using System.ComponentModel;

using TV = cmdwtf.Toolkit.WinForms.Native.ComCtl32.TreeView.TV;
using TVS = cmdwtf.Toolkit.WinForms.Native.ComCtl32.TreeView.TVS;

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
			_ = Native.User32.SendMessage(Handle, TVM.SETEXTENDEDSTYLE, TVS.EX.DOUBLEBUFFER, TVS.EX.DOUBLEBUFFER);
			base.OnHandleCreated(e);
		}

		/// <summary>
		/// A subsection of Tree-View messages.
		/// </summary>
		private static class TVM
		{
			// many TVM_ skipped.
			public const uint SETEXTENDEDSTYLE = TV.FIRST + 44;
			public const uint GETEXTENDEDSTYLE = TV.FIRST + 45;
		}
	}
}

