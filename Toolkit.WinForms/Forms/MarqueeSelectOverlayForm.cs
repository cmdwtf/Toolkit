using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using cmdwtf.Toolkit.WinForms.Extensions;
using cmdwtf.Toolkit.WinForms.Native;

using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms.Forms
{
	/// <summary>
	/// A <see cref="Form"/> that can be used as a marquee selection rectangle.
	/// </summary>
	[ToolboxItem(true)]
	public partial class MarqueeSelectOverlayForm : Form
	{
		private BrushHandle _nativeBackgroundBrush = null;

		/// <summary>
		/// Initializes <see cref="CreateParams"/> with
		/// the windows styles needed for overlay:
		/// WS_EX_TRANSPARENT, WS_EX_LAYERED, WS_EX_NOACTIVATE.
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS.EX.TRANSPARENT;
				cp.ExStyle |= WS.EX.LAYERED;
				cp.ExStyle |= WS.EX.NOACTIVATE;
				return cp;
			}
		}

		/// <summary>
		/// Creates a new instance of the <see cref="MarqueeSelectOverlayForm"/> class.
		/// </summary>
		public MarqueeSelectOverlayForm()
		{
			AutoScaleMode = AutoScaleMode.None;
			AutoValidate = AutoValidate.Disable;
			BackColor = SystemColors.Highlight;
			ClientSize = new Size(50, 50);
			ControlBox = false;
			FormBorderStyle = FormBorderStyle.None;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = nameof(MarqueeSelectOverlayForm);
			Opacity = 0.25D;
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.Manual;
			Text = nameof(MarqueeSelectOverlayForm);
			TopMost = true;
			TransparencyKey = Color.Magenta;
			ResumeLayout(false);
			SetBackgroundBrush();
		}

		/// <summary>
		/// Updates the <see cref="MarqueeSelectOverlayForm"/>,
		/// assigning it to a new owner, and size/location.
		/// </summary>
		/// <param name="owner">The owner window.</param>
		/// <param name="screenRect">The size and location to display the form.</param>
		public void Update(IWin32Window owner, Rectangle screenRect)
		{
			Show(owner);
			Location = screenRect.Location;
			Size = screenRect.Size;
		}

		/// <summary>
		/// Shows the <see cref="MarqueeSelectOverlayForm"/>.
		/// </summary>
		/// <param name="owner">The window that owns this form.</param>
		public new void Show(IWin32Window owner)
		{
			if (Visible)
			{
				return;
			}

			Owner = owner as Form;
			this.ShowWithNoFoucs();
		}

		/// <summary>
		/// Creates a solid brush, and sets it as a background brush.
		/// </summary>
		private void SetBackgroundBrush()
		{
			if (_nativeBackgroundBrush != null)
			{
				_nativeBackgroundBrush.Dispose();
			}

			// using some native tricks so we can set our background
			// brush to match our back color, so everything draws nicely on resize.
			_nativeBackgroundBrush = this.CreateNativeSolidBrush(BackColor);
			this.SetBackgroundBrush(_nativeBackgroundBrush);
		}

		/// <summary>
		/// Handles <see cref="Control.BackColor"/> changing,
		/// to update our native background brush.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			SetBackgroundBrush();
		}

		/// <summary>
		/// Handles the form closing, dispoging of our background brush.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			_nativeBackgroundBrush.Dispose();
		}

		/// <summary>
		/// Handles the form painting, drawing 1px a border rectangle
		/// with the <see cref="Control.ForeColor"/>.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			using var borderPen = new Pen(ForeColor);
			e.Graphics.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
		}
	}
}
