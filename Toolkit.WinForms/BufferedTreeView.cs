using System;
using System.Windows.Forms;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// A class that is just a <see cref="System.Windows.Forms.TreeView"/>, but doubled buffered.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Matching Win32 symbols.")]
	class BufferedTreeView : TreeView
	{
		protected override void OnHandleCreated(EventArgs e)
		{
			Native.User32.SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
			base.OnHandleCreated(e);
		}

		private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
		private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
		private const int TVS_EX_DOUBLEBUFFER = 0x0004;
	}
}
