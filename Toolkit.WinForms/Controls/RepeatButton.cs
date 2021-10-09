using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cmdwtf.Toolkit.WinForms.Controls
{
	/// <summary>
	/// A <see cref="Button"/> that handles being clicked repeatedly while it's held down.
	/// </summary>
	[ToolboxItem(true)]
	public class RepeatButton : Button
	{
		/// <summary>
		/// Raised when the button is no longer held down.
		/// </summary>
		[Category("Action")]
		[Description("Raised when the left mouse button is released.")]
		public event EventHandler<MouseEventArgs> ClickFinished = null;

		/// <summary>
		/// If true, the button is being held down.
		/// </summary>
		public bool IsDown => _repeatButtonTimer.Enabled;

		private readonly System.Timers.Timer _repeatButtonTimer;
		private bool _mousingUp = false;

		/// <summary>
		/// Creates a new instance of the <see cref="RepeatButton"/> class.
		/// </summary>
		public RepeatButton()
		{
			_repeatButtonTimer = new System.Timers.Timer(interval: 10);
			_repeatButtonTimer.Elapsed += RepeatButtonTimer_Elapsed;
		}

		#region Button overrides

		/// <summary>
		/// Raises the <see cref="Control.MouseDown"/> event.
		/// As well, starts the click repeating timer.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
				_repeatButtonTimer.Start();
			}
		}

		/// <summary>
		/// Raises the <see cref="Control.MouseUp"/> event.
		/// As well, stops the repeat timer and raises
		/// <see cref="ClickFinished"/> if the left button was released.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			_mousingUp = true;

			base.OnMouseUp(e);

			if (e.Button == MouseButtons.Left)
			{
				_repeatButtonTimer.Stop();

				ClickFinished?.Invoke(this, e);
			}

			_mousingUp = false;
		}

		/// <summary>
		/// Raises the <see cref="Control.Click"/> event.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnClick(EventArgs e)
		{
			if (_mousingUp)
			{
				return;
			}

			base.OnClick(e);
		}

		#endregion Button overrides

		/// <summary>
		/// Raises the <see cref="Control.Click"/> and <see cref="Control.MouseClick"/> events.
		/// </summary>
		/// <param name="sender">The origin of this event.</param>
		/// <param name="e">The event data.</param>
		private void RepeatButtonTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (InvokeRequired)
			{
				Invoke(() =>
				{
					RepeatButtonTimer_Elapsed(sender, e);
				});
				return;
			}

			Point pos = System.Windows.Forms.Cursor.Position;
			MouseEventArgs mea = new(MouseButtons.Left, 0, pos.X, pos.Y, 0);
			OnClick(mea);
			OnMouseClick(mea);
		}
	}
}
