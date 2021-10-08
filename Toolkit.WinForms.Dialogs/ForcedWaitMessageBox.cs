using System;
using System.Windows.Forms;

using STimeSpan = System.TimeSpan;

namespace cmdwtf.Toolkit.WinForms.Dialogs
{
	/// <summary>
	/// An "OK/Cancel" message box very similar to that shown by
	/// <see cref="MessageBox.Show(IWin32Window, string)"/>. However, the user
	/// is required to wait a <see cref="TimeOut"/> amount of time before the
	/// OK/Cancel buttons are enabled. This can be used to make sure the user
	/// is forced to wait before making a potentially destructive action.
	/// </summary>
	public partial class ForcedWaitMessageBox : Form
	{
		private readonly System.Diagnostics.Stopwatch _timeElapsed = new();
		private STimeSpan TimeOut { get; set; }

		private ForcedWaitMessageBox(IWin32Window owner, string message, string title, STimeSpan timeOut)
		{
			InitializeComponent();
			Owner = owner as Form;
			labelMessage.Text = message;
			Text = title;

			TimeOut = timeOut != default
				? timeOut
				: STimeSpan.FromSeconds(1);

			DialogResult = DialogResult.Cancel;

			// turn on the UI updater
			timerUi.Enabled = true;

			// disable the OK button
			buttonOk.Enabled = false;

			// start measuring time
			_timeElapsed.Stop();
			_timeElapsed.Reset();
			_timeElapsed.Start();
		}

		/// <summary>
		/// Shows a forced wait message box, modally.
		/// </summary>
		/// <param name="owner">The window that created this form.</param>
		/// <param name="message">The main content to display on the form.</param>
		/// <param name="title">The caption text to display on the form.</param>
		/// <param name="timeout">The amount of time that must elapse before the user can make a selection.</param>
		/// <returns>The choice the user made.</returns>
		public static DialogResult Show(IWin32Window owner, string message, string title, STimeSpan timeout = default)
		{
			ForcedWaitMessageBox mb = new(owner, message, title, timeout);
			return mb.ShowDialog(owner);
		}

		/// <summary>
		/// Shows a forced wait message box, modally.
		/// </summary>
		/// <param name="owner">The window that created this form.</param>
		/// <param name="message">The main content to display on the form.</param>
		/// <param name="title">The caption text to display on the form.</param>
		/// <param name="timeout">The amount of time that must elapse before the user can make a selection.</param>
		/// <param name="ok_text">The optional text to display on the "OK" button.</param>
		/// <param name="cancel_text">The optional text to display on the "Cancel" button.</param>
		/// <returns>The choice the user made.</returns>
		public static DialogResult Show(IWin32Window owner, string message, string title, STimeSpan timeout, string ok_text = "&OK", string cancel_text = "&Cancel")
		{
			ForcedWaitMessageBox mb = new(owner, message, title, timeout);

			mb.buttonOk.Text = ok_text;
			mb.buttonCancel.Text = cancel_text;

			return mb.ShowDialog(owner);
		}

		/// <inheritdoc cref="Show(IWin32Window, string, string, STimeSpan)"/>
		public static DialogResult Show(string message, string title, STimeSpan timeout)
			=> Show(null, message, title, timeout);

		private void TimerUi_Tick(object sender, EventArgs e)
		{
			STimeSpan remains = TimeOut - _timeElapsed.Elapsed;

			if (remains <= STimeSpan.Zero)
			{
				// time is up!
				remains = STimeSpan.Zero;
				timerUi.Enabled = false;
				labelTimeout.Visible = false;
				buttonOk.Enabled = true;
			}

			int remainsDisplay = (int)System.Math.Floor(remains.TotalSeconds);

			// update label
			labelTimeout.Text = "Please wait " + remainsDisplay.ToString() + " second" + (remainsDisplay != 1 ? "s" : "") + "...";
		}

		private void ButtonOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
