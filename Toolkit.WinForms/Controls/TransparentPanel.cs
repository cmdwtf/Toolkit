using System.ComponentModel;
using System.Windows.Forms;

using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms.Controls
{
	/// <summary>
	/// A <see cref="Panel"/>, but with the WS_EX_TRANSPARENT
	/// window style set, and a noop <see cref="OnPaintBackground(PaintEventArgs)"/>.
	/// </summary>
	[ToolboxItem(true)]
	public class TransparentPanel : Panel
	{
		/// <summary>
		/// Initializes <see cref="CreateParams"/> with
		/// the WS_EX_TRANSPARENT extended window style enabled.
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS.EX.TRANSPARENT;
				return cp;
			}
		}

		/// <summary>
		/// Handles the paint background event, turning it into a nop.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{

		}
	}
}
