using System.Windows.Forms;

using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms.DoubleBuffered
{

	/// <summary>
	/// An implicitly double buffered <see cref="System.Windows.Forms.Form"/>.
	/// </summary>
	/// <remarks>
	/// See: https://stackoverflow.com/questions/2612487, and https://stackoverflow.com/questions/25872849.
	/// </remarks>
	public class Form : System.Windows.Forms.Form
	{
		/// <summary>
		/// Initializes <see cref="CreateParams"/> with
		/// the WS_EX_COMPOSITED extended window style optionally enabled.
		/// An implementing class can override <see cref="UseWsExComposited"/>,
		/// and return `false` to prevent this behavior.
		/// This behavior only takes place if not in <see cref="System.ComponentModel.Component.DesignMode"/>.
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
				cp.ExStyle |= UseWsExComposited ? WS.EX.COMPOSITED : 0;
				return cp;
			}
		}

		/// <summary>
		/// Gets a value indicating if this <see cref="Form"/> should
		/// enable the WS_EX_COMPOSITIED style on creation. Defaults to `true`.
		/// </summary>
		protected virtual bool UseWsExComposited => true;

		/// <summary>
		/// Creates a new instance of the <see cref="Form"/> class.
		/// It enables <see cref="DoubleBuffered"/>.
		/// This behavior only takes place if not in <see cref="System.ComponentModel.Component.DesignMode"/>.
		/// </summary>
		public Form()
		{
			if (DesignMode)
			{
				return;
			}

			DoubleBuffered = true;
		}
	}
}
