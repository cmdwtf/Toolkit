using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using static cmdwtf.Toolkit.WinForms.Native.Windows;

using DBConstants = cmdwtf.Toolkit.WinForms.DoubleBuffered.Constants;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// A <see cref="System.Windows.Forms.ProgressBar"/> that
	/// draws itself using the 'Meter' style as outlined by the
	/// <see href="https://docs.microsoft.com/en-us/windows/win32/uxguide/progress-bars?redirectedfrom=MSDN#meters">
	/// visual style guide</see>.
	/// </summary>
	public class ProgressMeter : System.Windows.Forms.ProgressBar
	{
		/// <summary>
		/// Initializes <see cref="CreateParams"/> with
		/// the WS_EX_COMPOSITED extended window style enabled,
		/// and the WS_CLIPCHILDREN window style disabled.
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS.EX.COMPOSITED; // Turn on WS_EX_COMPOSITED
				cp.Style &= ~WS.CLIPCHILDREN; // Turn off WS_CLIPCHILDREN
				return cp;
			}
		}

		private readonly VisualStyleRendererCache _renderers = new();

		/// <summary>
		/// Creates a new instance of <see cref="ProgressMeter"/>.
		/// </summary>
		public ProgressMeter()
		{
			if (VisualStyleRenderer.IsSupported == false)
			{
				System.Diagnostics.Debug.WriteLine($"{nameof(ProgressMeter)} can not render, visual style renderers aren't supported. Will use normal progress bar rendering.");
				return;
			}

			DoubleBuffered = true;
			SetStyle(DBConstants.UserPaintStyles, true);
		}

		/// <summary>
		/// Handles the painting of the foreground.
		/// Invokes base's implementation if visual styles aren't supported.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (VisualStyleRenderer.IsSupported == false)
			{
				base.OnPaint(e);
				return;
			}

			float percent = (float)Value / (Maximum + Minimum);
			Rectangle fillRect = e.ClipRectangle;
			fillRect.Width = (int)System.Math.Round(Width * percent);
			VisualStyleElement.ProgressBar.Fill.Meter.GetRenderer(_renderers)
				.DrawBackground(e.Graphics, fillRect);
		}

		/// <summary>
		/// Handles the painting of the background.
		/// Invokes base's implementation if visual styles aren't supported.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (VisualStyleRenderer.IsSupported == false)
			{
				base.OnPaintBackground(e);
				return;
			}

			VisualStyleElement.ProgressBar.TransparentBar.Meter.GetRenderer(_renderers)
				.DrawBackground(e.Graphics, e.ClipRectangle);
		}
	}
}
