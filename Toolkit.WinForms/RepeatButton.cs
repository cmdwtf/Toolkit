using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// A <see cref="Button"/> that handles being clicked repeatedly while it's held down.
	/// </summary>
	public class RepeatButton : Button
	{
		/// <summary>
		/// Raised when the button is no longer held down.
		/// </summary>
		[Category("Action")]
		public event EventHandler<MouseEventArgs> ClickFinished = null;

		/// <summary>
		/// If true, the button is being held down.
		/// </summary>
		public bool IsDown
		{
			get
			{
				return _repeatButtonTimer.Enabled;
			}
		}

		/// <summary>
		/// Creates a new instance of the <see cref="RepeatButton"/> class.
		/// </summary>
		public RepeatButton()
		{
			_repeatButtonTimer.Interval = 10;
		}

		private readonly Timer _repeatButtonTimer = new();
		private bool _mousingUp = false;

		#region Button overrides

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
				OnClick(EventArgs.Empty);
				_repeatButtonTimer.Tick += new EventHandler(RepeatButtonTimer_Tick);
				_repeatButtonTimer.Start();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_mousingUp = true;

			base.OnMouseUp(e);

			if (e.Button == MouseButtons.Left)
			{
				_repeatButtonTimer.Stop();
				_repeatButtonTimer.Tick -= new EventHandler(RepeatButtonTimer_Tick);

				ClickFinished?.Invoke(this, e);
			}

			_mousingUp = false;
		}

		protected override void OnClick(EventArgs e)
		{
			if (_mousingUp)
			{
				return;
			}

			base.OnClick(e);
		}

		private void RepeatButtonTimer_Tick(object sender, EventArgs e) => OnClick(EventArgs.Empty);

		#endregion Button overrides
	}
}
