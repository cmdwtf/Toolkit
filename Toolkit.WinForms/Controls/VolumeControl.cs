using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cmdwtf.Toolkit.WinForms.Controls
{
	/// <summary>
	/// A control that offers a mouse-intractable volume bar,
	/// and a mute/unmute button.
	/// </summary>
	[ToolboxItem(true)]
	public partial class VolumeControl : DoubleBuffered.UserControl
	{
		/// <summary>
		/// Gets or sets the volume value represented in the control.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(0)]
		[Description("The volume level represented by this control.")]
		public int Volume
		{
			get => volumeControlBar.Value;

			set
			{
				if (value == Volume)
				{
					return;
				}

				if (value > VolumeMaximum)
				{
					value = VolumeMaximum;
				}

				if (value < 0)
				{
					value = VolumeMinimum;
				}

				int oldVolume = Volume;
				volumeControlBar.Value = value;

				if (value != oldVolume)
				{
					UpdateMutedImage();
				}
			}
		}

		/// <summary>
		/// Gets or sets if the control is representing a muted volume.
		/// Muting will make <see cref="Volume"/> return 0. However, unmuting
		/// will allow the <see cref="VolumeControl"/> to return to the level it
		/// was at prior to muting.
		/// </summary>
		public bool Mute
		{
			get => Volume == 0;

			set
			{
				if (value == Mute)
				{
					return;
				}

				if (value == false)
				{
					Volume = _dragStartValue;
				}
				else
				{
					_dragStartValue = Volume;
					Volume = 0;
				}
			}
		}

		/// <summary>
		/// The maximum volume level represented by the control.
		/// </summary>
		[Category("Behavior")]
		[Description("The maximum volume level represented by this control.")]
		public int VolumeMaximum => volumeControlBar.Maximum;

		/// <summary>
		/// The minimum volume level represented by the control.
		/// </summary>
		[Category("Behavior")]
		[Description("The minimum volume level represented by this control.")]
		public int VolumeMinimum => volumeControlBar.Minimum;

		/// <summary>
		/// Gets or sets a value that controls if the volume bar portion of the control is visible.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(true)]
		[Description("True if the volume bar should be shown.")]
		public bool VolumeBarVisible { get => volumeControlBar.Visible; set => volumeControlBar.Visible = value; }

		/// <summary>
		/// Gets or sets a value that controls if the mute 'button' (picturebox) part of the control is visible.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(true)]
		[Description("True if the mute button should be shown.")]
		public bool MuteButtonVisible { get => pictureBoxMute.Visible; set => pictureBoxMute.Visible = value; }

		/// <summary>
		/// Gets or sets the image used to represent the volume being muted.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(null)]
		[Description("The image used for the mute button when the volume is zero.")]
		public Image UnmutedImage { get; set; } = null;

		/// <summary>
		/// Gets or sets the image used to represent the volume being unmuted.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(null)]
		[Description("The image used for the mute button when the volume is nonzero.")]
		public Image MutedImage { get; set; } = null;

		private int _dragStartValue;

		/// <summary>
		/// Creates a new instance of <see cref="VolumeControl"/>.
		/// </summary>
		public VolumeControl()
		{
			InitializeComponent();
			UpdateMutedImage();
			_dragStartValue = volumeControlBar.Value;
		}

		/// <summary>
		/// Handles the mute 'button' being clicked.
		/// </summary>
		/// <param name="sender">The origin of the event.</param>
		/// <param name="e">The event data.</param>
		private void PictureBoxMute_Click(object sender, EventArgs e)
			 => Mute = !Mute;

		/// <summary>
		/// Handles the volume bar being clicked.
		/// </summary>
		/// <param name="sender">The origin of the event.</param>
		/// <param name="e">The event data.</param>
		private void VolumeControlBar_MouseDown(object sender, MouseEventArgs e)
			 => _dragStartValue = volumeControlBar.Value;

		/// <summary>
		/// Handles the user changing the volume bar's value by clicking on it.
		/// </summary>
		/// <param name="sender">The origin of the event.</param>
		/// <param name="e">The event data.</param>
		private void VolumeControlBar_ValueChangedFromMouseInput(object sender, EventArgs e)
			 => UpdateMutedImage();

		/// <summary>
		/// Updates the mute/unmute picture box image by current volume.
		/// </summary>
		private void UpdateMutedImage()
		{
			// swap mute/unmute image based on volume change.
			if (Volume == 0)
			{
				// switched to mute.
				pictureBoxMute.BackgroundImage = MutedImage ?? Resources.Images.AudioMute_16x;
			}
			else if (Volume != 0)
			{
				// switched to unmuted.
				pictureBoxMute.BackgroundImage = UnmutedImage ?? Resources.Images.Audio_16x;
			}
		}
	}
}
