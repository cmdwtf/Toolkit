using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using static cmdwtf.Toolkit.WinForms.Native.Windows;

using SDColor = System.Drawing.Color;
using SWFProgressBar = System.Windows.Forms.ProgressBar;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// A custom <see cref="SWFProgressBar"/> that draws it's value
	/// on itself, as optionally controlled by <see cref="ShowValue"/>.
	/// </summary>
	[ToolboxItem(true)]
	public class ValueBar : SWFProgressBar
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

		/// <summary>
		/// Gets or sets the color to draw the text if the value bar is shorter than the text length.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(typeof(SDColor), nameof(SDColor.Black))]
		[Description("The color to draw the text if the value bar is shorter than the text length.")]
		public SDColor LowValueTextColor { get; set; } = SDColor.Black;

		/// <summary>
		/// Gets or sets the color to draw the text if the value bar is longer than the text length.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(typeof(SDColor), nameof(SDColor.White))]
		[Description("The color to draw the text if the value bar is longer than the text length.")]
		public SDColor HighValueTextColor { get; set; } = SDColor.White;

		/// <summary>
		/// Gets or sets a color that will be drawn as the end of the value bar gradient.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(typeof(SDColor), nameof(SystemColors.Highlight))]
		[Description("The color at the maximum end of the bar to fade to over a linear gradient.")]
		public SDColor ForeColorGradientEnd { get; set; } = SystemColors.Highlight;

		/// <summary>
		/// Gets or sets a value that is drawn when the bar is at the minimum value.
		/// If empty, will always draw the value.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue("")]
		[Description("The optional text to draw when the value is at the minimum.")]
		public string MinimumText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value that is drawn when the bar is at the minimum value.
		/// If empty, will always draw the value.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue("")]
		[Description("The optional text to draw when the value is at the maximum.")]
		public string MaximumText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value that indicates if the control
		/// will allow it's value to be changed by mouse input.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(true)]
		[Description("If true, left mouse clicks/drags will change the bar's value.")]
		public bool UpdateByMouseInput { get; set; } = true;

		/// <summary>
		/// Gets a value that represents what percent of the way the
		/// bar's value is from minimum to maximum.
		/// </summary>
		[Browsable(false)]
		public float ValuePercent => (float)Value / (Maximum + Minimum);

		/// <summary>
		/// Gets or sets a value that indicates if the control
		/// should draw the value text.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(true)]
		[Description("If true, the control will render its value as text.")]
		public bool ShowValueText { get; set; } = true;

		/// <summary>
		/// Gets or sets a value that indicates if the control
		/// should draw its value bar using the system's progress bar rendering.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(false)]
		[Description("If true, the control will render using system progress bar rendering.")]
		public bool UseNativeProgressBarStyle
		{
			get => GetStyle(userPaintStyles) == false;
			set
			{
				SetStyle(userPaintStyles, !value);
				Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates if the control
		/// should draw its value bar using the system's progress bar rendering.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(ProgressBar.State.Normal)]
		[Description("What state to draw the native progress bar in, if used.")]
		public ProgressBar.State NativeProgressBarState
		{
			get => this.GetState();
			set => this.SetState(value);
		}

		/// <summary>
		/// An event raised if a users's mouse click changes the bar's value.
		/// </summary>
		[Category("Action")]
		[DefaultValue(null)]
		[Description("Raised when the bar's value changes from mouse input.")]
		public event EventHandler<EventArgs> ValueChangedFromMouseInput;

		private Font _scaledFont = null;
		private readonly StringFormat _stringFormat = new StringFormat().SetAlignments(ContentAlignment.MiddleLeft);
		private Rectangle _valueBarRect = Rectangle.Empty;
		private ControlStyles userPaintStyles = ControlStyles.UserPaint |
			ControlStyles.AllPaintingInWmPaint |
			ControlStyles.OptimizedDoubleBuffer;

		/// <summary>
		/// Creates a new instance of <see cref="ValueBar"/>.
		/// </summary>
		public ValueBar()
		{

		}

		/// <summary>
		/// Handles updating the volume bar as the cursor is dragged.
		/// </summary>
		private void UpdateByCursor()
		{
			int mouseX = PointToClient(System.Windows.Forms.Cursor.Position).X;
			float percent = (float)mouseX / Width;

			percent = Math.Clamp(percent, 0.0f, 1.0f);

			int previousValue = Value;
			Value = (int)System.Math.Round((percent * (Maximum + Minimum)) - Minimum);

			if (previousValue != Value)
			{
				ValueChangedFromMouseInput?.Invoke(this, new EventArgs());
			}
		}

		/// <summary>
		/// Handles mouse input.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			bool mouseLeftDown = MouseButtons.HasFlag(MouseButtons.Left);

			if (UpdateByMouseInput && mouseLeftDown)
			{
				UpdateByCursor();
			}
		}

		/// <summary>
		/// Draws the <see cref="ValueBar"/> background.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (ProgressBarRenderer.IsSupported)
			{
				ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
			}
			else
			{
				// backup rendering.
				Brush pbBackground = SystemBrushes.ControlDark;
				Pen pbPen = SystemPens.ActiveBorder;
				e.Graphics.FillRectangle(pbBackground, e.ClipRectangle);
				e.Graphics.DrawRectangle(pbPen, e.ClipRectangle);
			}

			_valueBarRect = GetValueBarRect(e.ClipRectangle, ValuePercent, isFillRectangle: true);

			if (ForeColor != ForeColorGradientEnd)
			{
				// draw two colors as a gradient
				e.Graphics.FillRectangleLinearGradient(ForeColor, _valueBarRect, ForeColorGradientEnd);
			}
			else
			{
				using SolidBrush foreBrush = new(ForeColor);
				e.Graphics.FillRectangle(foreBrush, _valueBarRect);
			}
		}

		/// <summary>
		/// Draws the <see cref="ValueBar"/>.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (ShowValueText == false)
			{
				return;
			}

			_valueBarRect = GetValueBarRect(e.ClipRectangle, ValuePercent, isFillRectangle: true);

			// get text: optional minimum/maximum text values, or the value itself.
			string text = Value == Maximum && !string.IsNullOrEmpty(MaximumText)
				? MaximumText
				: Value == Minimum && !string.IsNullOrEmpty(MinimumText)
					? MinimumText
					: $"{Value}";

			if (_scaledFont is null || _scaledFont.SizeInPoints > _valueBarRect.Height)
			{
				_scaledFont?.Dispose();
				float scale = 0.65f; // scale determined by randomly trying until i found something that worked nicely.
				float textEm = System.Math.Max(_valueBarRect.Height * scale, 6.0f);
				_scaledFont = new Font(Font.FontFamily, textEm, Font.Style);
			}

			SizeF textSize = e.Graphics.MeasureString(text, _scaledFont, e.ClipRectangle.Size);
			float textPercent = textSize.Width / (float)e.ClipRectangle.Width;

			using SolidBrush textBrush = (textPercent < ValuePercent)
				? new(HighValueTextColor)
				: new(LowValueTextColor);

			Rectangle textLayoutRect = GetValueBarRect(e.ClipRectangle, 1.0f, isFillRectangle: false);

			e.Graphics.DrawString(text, _scaledFont, textBrush, textLayoutRect, _stringFormat);
			// debug rect:
			//e.Graphics.DrawRectangle(Pens.Red, textLayoutRect.X, textLayoutRect.Y, textLayoutRect.Width, textLayoutRect.Height);
		}

		/// <summary>
		/// Gets a rect representing the value bar to draw inside the control proper,
		/// based on the clipping rectangle as the full size.
		/// </summary>
		/// <param name="clipRectangle">The full control rectangle.</param>
		/// <param name="valueScale">The value to scale the width by, in percentage. (0-1)</param>
		/// <param name="isFillRectangle">If false, the rectangle will be further reduced by a pixel to account for not being a 'fill' rectangle.</param>
		/// <returns>The deflated, width-scaled value rectang.e</returns>
		private Rectangle GetValueBarRect(Rectangle clipRectangle, float valueScale, bool isFillRectangle)
		{
			// scale the width by the percent value
			clipRectangle.Width = (int)(clipRectangle.Width * valueScale);

			int twoPixels = LogicalToDeviceUnits(2);
			int onePixel = LogicalToDeviceUnits(1);

			// we need to draw *inside* the progress bar proper.
			clipRectangle.Inflate(-twoPixels, -twoPixels);

			// deflating doesn't quite get us where we want to be with a non-fill rectangle.
			// so we need to squish it a pixel.
			if (isFillRectangle == false)
			{
				clipRectangle.Width -= onePixel;
				clipRectangle.Height -= onePixel;
			}
			return clipRectangle;
		}
	}
}

