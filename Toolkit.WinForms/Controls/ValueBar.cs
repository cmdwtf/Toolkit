using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using cmdwtf.Toolkit.WinForms.Extensions;

using static cmdwtf.Toolkit.WinForms.Native.Windows;

using DBConstants = cmdwtf.Toolkit.WinForms.DoubleBuffered.Constants;

namespace cmdwtf.Toolkit.WinForms.Controls
{
	/// <summary>
	/// A custom <see cref="StyledProgressBar"/> (<see cref="ProgressBar"/>)
	/// that draws it's value on itself, as optionally
	/// controlled by <see cref="ShowValueText"/>. As well, the value
	/// can be adjusted by the user via mouse input with clicking on the control.
	/// </summary>
	[ToolboxItem(true)]
	[DefaultEvent(nameof(ValueChangedFromMouseInput))]
	public class ValueBar : StyledProgressBar
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
		[DefaultValue(typeof(Color), nameof(Color.Black))]
		[Description("The color to draw the text if the value bar is shorter than the text length.")]
		public virtual Color LowValueTextColor { get; set; } = Color.Black;

		/// <summary>
		/// Gets or sets the color to draw the text if the value bar is longer than the text length.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(typeof(Color), nameof(Color.White))]
		[Description("The color to draw the text if the value bar is longer than the text length.")]
		public virtual Color HighValueTextColor { get; set; } = Color.White;

		/// <summary>
		/// Gets or sets a color that will be drawn as the end of the value bar gradient.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(typeof(Color), nameof(SystemColors.Highlight))]
		[Description("The color at the maximum end of the bar to fade to over a linear gradient.")]
		public virtual Color ForeColorGradientEnd { get; set; } = SystemColors.Highlight;

		/// <summary>
		/// Gets or sets a value that is drawn when the bar is at the minimum value.
		/// If empty, will always draw the value.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue("")]
		[Description("The optional text to draw when the value is at the minimum.")]
		public virtual string MinimumText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value that is drawn when the bar is at the minimum value.
		/// If empty, will always draw the value.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue("")]
		[Description("The optional text to draw when the value is at the maximum.")]
		public virtual string MaximumText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value that indicates if the control
		/// will allow it's value to be changed by mouse input.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(true)]
		[Description("If true, left mouse clicks/drags will change the bar's value.")]
		public virtual bool UpdateByMouseInput { get; set; } = true;

		/// <summary>
		/// Gets a value that represents what percent of the way the
		/// bar's value is from minimum to maximum.
		/// </summary>
		[Browsable(false)]
		public virtual float ValuePercent => (float)Value / (Maximum + Minimum);

		/// <summary>
		/// Gets or sets a value that indicates if the control
		/// should draw the value text.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(true)]
		[Description("If true, the control will render its value as text.")]
		public virtual bool ShowValueText { get; set; } = true;

		/// <summary>
		/// Gets or sets a value that indicates if the control
		/// should draw its value bar using the system's progress bar rendering.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(false)]
		[Description("If true, the control will render using system progress bar rendering.")]
		public virtual bool UseNativeProgressBarStyle
		{
			get => _useNativeProgressBarStyle;
			set
			{
				if (value != _useNativeProgressBarStyle)
				{
					SetStyle(DBConstants.UserPaintStyles, !value);
					Invalidate();
				}

				_useNativeProgressBarStyle = value;
			}
		}

		/// <summary>
		/// An event raised if a users' mouse click changes the bar's value.
		/// </summary>
		[Category("Action")]
		[DefaultValue(null)]
		[Description("Raised when the bar's value changes from mouse input.")]
		public event EventHandler<EventArgs> ValueChangedFromMouseInput;

		private Font _scaledFont = null;
		private readonly StringFormat _stringFormatHorizontal = new StringFormat().SetAlignments(ContentAlignment.MiddleLeft);
		private readonly StringFormat _stringFormatVertical = new StringFormat().SetAlignments(ContentAlignment.MiddleLeft);
		private Rectangle _valueBarRect = Rectangle.Empty;
		private bool _useNativeProgressBarStyle = false;

		/// <summary>
		/// Creates a new instance of <see cref="ValueBar"/>.
		/// </summary>
		public ValueBar()
		{
			SetStyle(DBConstants.UserPaintStyles, !UseNativeProgressBarStyle);
		}

		/// <summary>
		/// Handles updating the volume bar as the cursor is dragged.
		/// </summary>
		private void UpdateByCursor()
		{
			Point mouse = PointToClient(Cursor.Position);
			float percent = Orientation == Orientation.Horizontal
				? (float)mouse.X / Width
				: 1.0f - ((float)mouse.Y / Height);

			percent = percent.Clamp(0.0f, 1.0f);

			int previousValue = Value;
			Value = (int)System.Math.Round((percent * (Maximum + Minimum)) - Minimum);

			if (previousValue != Value)
			{
				OnValueChangedFromMouseInput(new EventArgs());
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

			// get text: optional minimum/maximum text values, or the value itself.
			string text = Value == Maximum && !string.IsNullOrEmpty(MaximumText)
				? MaximumText
				: Value == Minimum && !string.IsNullOrEmpty(MinimumText)
					? MinimumText
					: $"{Value}";

			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}

			_valueBarRect = GetValueBarRect(e.ClipRectangle, ValuePercent, isFillRectangle: true);

			int textHeightArea = Orientation == Orientation.Horizontal
				? _valueBarRect.Height
				: _valueBarRect.Width;

			// get a font that is determined by the height (or width for vertical) of the control
			if (_scaledFont is null || _scaledFont.SizeInPoints > textHeightArea)
			{
				_scaledFont?.Dispose();
				float scale = 0.65f; // scale determined by randomly trying until i found something that worked nicely.
				float textEm = System.Math.Max(textHeightArea * scale, 6.0f);
				_scaledFont = new Font(Font.FontFamily, textEm, Font.Style);
			}

			// get the size to measure the text in,
			// 'rotating' it if we are vertical.
			SizeF measureSize = Orientation == Orientation.Horizontal
				? e.ClipRectangle.Size
				: new SizeF(e.ClipRectangle.Size.Height, e.ClipRectangle.Size.Width);

			// measure the text size, and get the percentage of how much of the control
			// it occupies depending on the orientation.
			SizeF textSize = e.Graphics.MeasureString(text, _scaledFont, measureSize);
			float textPercent = textSize.Width / (Orientation == Orientation.Horizontal
				? e.ClipRectangle.Width
				: e.ClipRectangle.Height);

			// choose the high or low value brush depending on where the value is at.
			using SolidBrush textBrush = textPercent < ValuePercent
				? new(HighValueTextColor)
				: new(LowValueTextColor);

			// we want to bump the text in just a smidge.
			int twoPixels = LogicalToDeviceUnits(2);

			// draw the text based on the orientation.
			if (Orientation == Orientation.Horizontal)
			{
				// horizontal will draw to the left of the control, centered vertically.
				float x = twoPixels;
				float y = Height / 2.0f;
				e.Graphics.DrawString(text, _scaledFont, textBrush, x, y, _stringFormatHorizontal);
			}
			else
			{
				// vertical will draw at the bottom of the control, centered horizontally,
				// and rotated -90 degrees.
				float x = Width / 2.0f;
				float y = Height - twoPixels;
				float angle = 270.0f;
				e.Graphics.DrawString(text, _scaledFont, textBrush, x, y, angle, _stringFormatVertical);
			}

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
			Size previousSize = clipRectangle.Size;
			if (Orientation == Orientation.Horizontal)
			{
				// scale the width by the percent value
				clipRectangle.Width = (int)System.Math.Round(clipRectangle.Width * valueScale);
			}
			else
			{
				// like above, get percentage of height, but also
				// move the rect to start at the bottom so it grows 'up'
				// rather than from the top down.
				// this is the native progress bar behavior.
				clipRectangle.Height = (int)System.Math.Round(clipRectangle.Height * valueScale);
				clipRectangle.Y += previousSize.Height - clipRectangle.Height;
			}

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

		/// <summary>
		/// Raises the <see cref="ValueChangedFromMouseInput"/> event.
		/// </summary>
		/// <param name="args">A <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnValueChangedFromMouseInput(EventArgs args)
			=> ValueChangedFromMouseInput?.Invoke(this, args);
	}
}

