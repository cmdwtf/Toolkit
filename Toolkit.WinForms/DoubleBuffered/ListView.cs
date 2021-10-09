using System.ComponentModel;
using System.Windows.Forms;

using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms.DoubleBuffered
{
	/// <summary>
	/// An implicitly double buffered <see cref="System.Windows.Forms.ListView"/>.
	/// </summary>
	/// <remarks>
	/// See: https://stackoverflow.com/questions/2612487, and https://stackoverflow.com/questions/25872849.
	/// </remarks>
	[ToolboxItem(true)]
	public class ListView : System.Windows.Forms.ListView
	{
		/// <summary>
		/// Initializes <see cref="CreateParams"/> with
		/// the WS_EX_COMPOSITED extended window style enabled.
		/// This behavior only takes place if not in <see cref="Component.DesignMode"/>.
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				if (DesignMode)
				{
					return base.CreateParams;
				}

				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS.EX.COMPOSITED;
				return cp;
			}
		}

		/// <summary>
		/// Creates a new instance of the <see cref="ListView"/> class.
		/// It enables <see cref="DoubleBuffered"/>, and sets the styles:
		/// <see cref="ControlStyles.OptimizedDoubleBuffer"/> and <see cref="ControlStyles.AllPaintingInWmPaint"/>.
		/// This behavior only takes place if not in <see cref="Component.DesignMode"/>.
		/// </summary>
		public ListView()
		{
			if (DesignMode)
			{
				return;
			}

			DoubleBuffered = true;
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}
	}
}
