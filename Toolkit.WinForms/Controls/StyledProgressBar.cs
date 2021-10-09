using System;
using System.ComponentModel;
using System.Windows.Forms;

using cmdwtf.Toolkit.WinForms.DoubleBuffered;
using cmdwtf.Toolkit.WinForms.Extensions;

using static cmdwtf.Toolkit.WinForms.Native.ComCtl32.ProgressBar;
using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms.Controls
{
	/// <summary>
	/// A <see cref="ProgressBar"/>, but with the functionality exposed to
	/// set it's <see cref="State"/>, as defined in Windows' visual styles.
	/// THANKS AREO
	/// </summary>
	public class StyledProgressBar : ProgressBar
	{
		/// <summary>
		/// Initializes <see cref="CreateParams"/> with
		/// the WS_EX_COMPOSITED extended window style enabled,
		/// and the WS_CLIPCHILDREN window style disabled. As well,
		/// enables the "SMOOTH" and "SMOOTHREVERSE" progress bar styles.
		/// This allows the progress bar to animate with positive
		/// and negative value changes.
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS.EX.COMPOSITED; // Turn on WS_EX_COMPOSITED
				cp.Style &= ~WS.CLIPCHILDREN; // Turn off WS_CLIPCHILDREN
				cp.Style |= SmoothAnimation ? PBS.SMOOTH : 0;
				cp.Style |= SmoothAnimationReverse ? PBS.SMOOTHREVERSE : 0;
				cp.Style |= Vertical ? PBS.VERTICAL : 0;
				return cp;
			}
		}

		/// <summary>
		/// Gets or sets if the <see cref="StyledProgressBar"/> should animate
		/// it's value as it increases.
		/// </summary>
		[Description($"Gets or sets a value for if the {nameof(StyledProgressBar)} should animate as it's value increases.")]
		[Category("Appearance")]
		[DefaultValue(true)]
		public virtual bool SmoothAnimation
		{
			get => _smooth;
			set
			{
				if (_smooth != value)
				{
					this.SetSmoothNative(value);
				}

				_smooth = value;
			}
		}

		/// <summary>
		/// Gets or sets if the <see cref="StyledProgressBar"/> should animate
		/// it's value as it decreases.
		/// </summary>
		[Description($"Gets or sets a value for if the {nameof(StyledProgressBar)} should animate as it's value decreases.")]
		[Category("Appearance")]
		[DefaultValue(true)]
		public virtual bool SmoothAnimationReverse
		{
			get => _smoothReverse;
			set
			{
				if (_smoothReverse != value)
				{
					this.SetSmoothReverseNative(value);
				}

				_smoothReverse = value;
			}
		}

		/// <summary>
		/// The visual state (style) the progress bar should have.
		/// </summary>
		[Description($"Gets or sets the {nameof(StyledProgressBar)} 'state'.")]
		[Category("Appearance")]
		[DefaultValue(ProgressBarState.Normal)]
		public virtual ProgressBarState State
		{
			get => _state;
			set
			{
				if (_state != value)
				{
					this.SetStateNative(value);
				}

				_state = value;
			}
		}

		/// <summary>
		/// Gets or sets the orientation of the <see cref="StyledProgressBar"/>.
		/// If <c>true</c>, displays vertically. Otherwise, horizontally.
		/// This is the same as setting the <see cref="Orientation"/> property.
		/// </summary>
		[Description($"Gets or sets the {nameof(StyledProgressBar)} orientation.")]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Vertical
		{
			get => Orientation == Orientation.Vertical;
			set => Orientation = value ? Orientation.Vertical : Orientation.Horizontal;
		}

		/// <summary>
		/// Gets or sets a value that corresponds to the orientation of the progress bar.
		/// </summary>
		[Description($"Gets or sets the {nameof(StyledProgressBar)} orientation.")]
		[Category("Appearance")]
		[DefaultValue(Orientation.Horizontal)]
		public virtual Orientation Orientation
		{
			get => _orientation;
			set
			{
				if (_orientation != value)
				{
					this.SetOrientationNative(value);
				}

				_orientation = value;
			}
		}

		/// <summary>
		/// The backing field for <see cref="SmoothAnimation"/>.
		/// </summary>
		protected bool _smooth = true;

		/// <summary>
		/// The backing field for <see cref="SmoothAnimationReverse"/>.
		/// </summary>
		protected bool _smoothReverse = true;


		/// <summary>
		/// The backing field for <see cref="State"/>.
		/// </summary>
		protected ProgressBarState _state = ProgressBarState.Normal;

		/// <summary>
		/// The backing field for <see cref="Orientation"/>.
		/// </summary>
		protected Orientation _orientation = Orientation.Horizontal;

		/// <summary>
		/// Creates a new instance of the <see cref="StyledProgressBar"/> class.
		/// </summary>
		public StyledProgressBar()
		{
			DoubleBuffered = true;
			SetStyle(Constants.DoubleBufferStyles, true);
		}

		/// <inheritdoc/>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (State != ProgressBarState.Normal)
			{
				this.SetStateNative(_state);
			}
		}

		/// <summary>
		/// Handles the windows messages sent to this class.
		/// </summary>
		/// <param name="m">The current message.</param>
		/// <remarks>
		/// Thanks to <see href="https://github.com/LorenzCK/WindowsFormsAero">LorenzCK</see>
		/// for this clever way of using SETPOS to switch states to work around
		/// the bar not updating it's value correctly.
		/// This function is licensed under the MS-RL.
		/// </remarks>
		protected override void WndProc(ref Message m)
		{
			// Intercept PBM_SETPOS messages that update the progressbar's value
			// and switch to normal state if the progress bar is paused or in error
			// (which prevents the progressbar from updating its value correctly).
			if (m.Msg == PBM.SETPOS && State != ProgressBarState.Normal)
			{
				this.SetStateNative(ProgressBarState.Normal);
			}

			base.WndProc(ref m);

			//Switch back to original state if needed
			if (m.Msg == PBM.SETPOS && State != ProgressBarState.Normal)
			{
				this.SetStateNative(State);
			}
		}

	}
}
