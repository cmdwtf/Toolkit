using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using SDColor = System.Drawing.Color;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// A <see cref="PictureBox"/> that draws it's image
	/// tinted with the set <see cref="TintColor"/> instead of normally.
	/// Setting the color to <see cref="SDColor.White"/> will cause it to
	/// draw normally.
	/// </summary>
	[ToolboxItem(true)]
	public class TintedPictureBox : PictureBox
	{
		/// <summary>
		/// Gets or sets a color that will be used to tint
		/// the image when drawn.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(typeof(SDColor), nameof(SDColor.White))]
		[Description("The color tint to draw the picture with.")]
		public SDColor TintColor
		{
			get => _tintColor;
			set
			{
				_tintColor = value;
				Invalidate();
			}
		}

		private SDColor _tintColor = SDColor.White;

		/// <summary>
		/// Handles the paint event, drawing the image tinted.
		/// </summary>
		/// <param name="pe">The event data.</param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			if (TintColor == SDColor.Empty)
			{
				// no color, nothing to draw.
				return;
			}
			else if (TintColor == SDColor.White)
			{
				// white tint is just the normal image.
				base.OnPaint(pe);
				return;
			}
			else
			{
				// paint the tinted image.
				pe.Graphics.DrawImageTinted(Image, new Rectangle(0, 0, Image.Width, Image.Height), TintColor);
			}
		}
	}
}
