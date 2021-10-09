using System;
using System.Diagnostics;
using System.Windows.Forms;

using STimeSpan = System.TimeSpan;

namespace cmdwtf.Toolkit.WinForms.Forms
{
	/// <summary>
	/// A simple modal message box dialog that can be shown while the user
	/// is waiting on an action that is taking longer than expected. They may
	/// optionally retry or cancel the operation after a set amount of time.
	/// As well, the dialog can be shown 'Hidden' so that it will modally block
	/// while the background task processes, but without showing the form to the user.
	/// This allows quicker operations to complete without ever showing the dialog,
	/// but still blocks user input for the moments they are running.
	/// </summary>
	public partial class WaitingDialog : Form
	{
		/// <summary>
		/// The large, main instruction displayed on the dialog.
		/// </summary>
		public string MainInstruction
		{
			get => labelMainText.Text;
			set => labelMainText.Text = value;
		}

		/// <summary>
		/// THe smaller, longer text content dispalyed on the dialog.
		/// </summary>
		public string Content
		{
			get => labelBodyText.Text;
			set => labelBodyText.Text = value;
		}

		private string _timeoutMainInstruction = string.Empty;
		private string _timeoutContent = string.Empty;
		private STimeSpan _retryTimeout = STimeSpan.FromSeconds(1);
		private readonly Stopwatch _retryStopwatch = new();
		private object _retryContext = null;

		private readonly Stopwatch _hideDialogStopwatch = new();
		private STimeSpan _hideDialogTimeout = STimeSpan.FromSeconds(1);
		private bool _hideDialog = true;

		/// <summary>
		/// A delegate representing a callback from <see cref="WaitingDialog"/> that
		/// is invoked periodically to see if the task being waited on has finished.
		/// If the delegate returns true, the dialog will close.
		/// </summary>
		/// <returns><c>true</c> if the background task is done, otherwise <c>false</c>.</returns>
		public delegate bool CheckForDoneWaitingDelegate();

		/// <summary>
		/// A delegate representing a callback from <see cref="WaitingDialog"/> that
		/// can be invoked by the user pressing the 'Retry' button on the dialog.
		/// </summary>
		/// <param name="context">The context that was provided when the <see cref="WaitingDialog"/> was shown.</param>
		public delegate void RetryActionDelegate(object context);

		private CheckForDoneWaitingDelegate _checkForDoneFunc = null;
		private RetryActionDelegate _retryAction = null;

		private bool _inTimeoutMode = false;

		private WaitingDialog()
		{
			InitializeComponent();
			SetUiEnabledForTimeout(false);
		}

		private void StartTimer(STimeSpan disableButtonsTimeout, STimeSpan hideDialogTimeout)
		{
			timerTimeout.Interval = (int)disableButtonsTimeout.TotalMilliseconds;
			_inTimeoutMode = false;

			timerTimeout.Start();

			_hideDialogTimeout = hideDialogTimeout;

			if (_hideDialogTimeout > STimeSpan.Zero)
			{
				_hideDialogStopwatch.Reset();
				_hideDialogStopwatch.Start();
				_hideDialog = true;
				Opacity = 0;
			}
			else
			{
				_hideDialog = false;
				Opacity = 1;
			}

			_retryStopwatch.Reset();
			_retryStopwatch.Start();
		}

		/// <inheritdoc cref="Show(IWin32Window, string, string, CheckForDoneWaitingDelegate, string, string, RetryActionDelegate, object, STimeSpan, STimeSpan, STimeSpan)"/>
		public static DialogResult Show(
			string mainInstruction,
			string content,
			CheckForDoneWaitingDelegate checkForDoneFunc,
			string timeoutMainInstruction = null,
			string timeoutContent = null,
			RetryActionDelegate retryAction = null,
			object retryContext = null,
			STimeSpan retryTimeout = default,
			STimeSpan disableButtonsFor = default,
			STimeSpan hideDialogFor = default
			)
			=> Show(null, mainInstruction, content, checkForDoneFunc, timeoutMainInstruction, timeoutContent,
				retryAction, retryContext, retryTimeout, disableButtonsFor, hideDialogFor);

		/// <summary>
		/// Shows a waiting dialog modally, until the provided <paramref name="checkForDoneFunc"/> returns true.
		/// </summary>
		/// <param name="owner">The window that created this form.</param>
		/// <param name="mainInstruction">The large instruction text to display on the dialog.</param>
		/// <param name="content">The smaller, extra text to display on the dialog.</param>
		/// <param name="checkForDoneFunc">The function to be invoked to check for the background task being completed.</param>
		/// <param name="timeoutMainInstruction">Optional text to replace the main instruction with once the button disable timeout has been reached.</param>
		/// <param name="timeoutContent">Optional text to replace the content text with once the button disable timeout has been reached.</param>
		/// <param name="retryAction">An optional action that will be invoked periodically while the done function is not returning true.</param>
		/// <param name="retryContext">An object that will be passed to the <paramref name="retryAction"/> action to provide context.</param>
		/// <param name="retryTimeout">The amount of time between <paramref name="retryAction"/> invocations.</param>
		/// <param name="disableButtonsFor">The amount of time to disable the "Retry" and "Cancel" buttons.</param>
		/// <param name="hideDialogFor">The amount of time to keep the dialog invisible before presenting it to the user.</param>
		/// <returns
		/// >A <see cref="DialogResult"/> for the user pressing <see cref="DialogResult.Retry"/>
		/// or <see cref="DialogResult.Cancel"/>. If the dialog closes from
		/// <paramref name="checkForDoneFunc"/> returning <c>true</c>, the
		/// <see cref="DialogResult.Cancel"/> value will be returned.
		/// </returns>
		public static DialogResult Show(
			IWin32Window owner,
			string mainInstruction,
			string content,
			CheckForDoneWaitingDelegate checkForDoneFunc,
			string timeoutMainInstruction = null,
			string timeoutContent = null,
			RetryActionDelegate retryAction = null,
			object retryContext = null,
			STimeSpan retryTimeout = default,
			STimeSpan disableButtonsFor = default,
			STimeSpan hideDialogFor = default
			)
		{
			WaitingDialog wd = new();

			// set up owner
			wd.Owner = owner as Form;

			// setup dialog text
			wd.MainInstruction = mainInstruction;
			wd.Content = content;

			// set up the checking for done
			wd._checkForDoneFunc = checkForDoneFunc;

			// store timeout text, defaulting to the already
			// provided text if the user didn't supply any.
			wd._timeoutMainInstruction = timeoutMainInstruction ?? mainInstruction;
			wd._timeoutContent = timeoutContent ?? content;

			// setup the retry action data
			wd._retryAction = retryAction;
			wd._retryContext = retryContext;
			wd._retryTimeout = retryTimeout;

			// start the timer and show the dialog.
			wd.StartTimer(disableButtonsFor, hideDialogFor);
			return wd.ShowDialog(owner);
		}

		private void SetUiEnabledForTimeout(bool enabled)
		{
			_inTimeoutMode = enabled;

			buttonCancel.Enabled = _inTimeoutMode;
			buttonRetry.Enabled = _inTimeoutMode;

			UseWaitCursor = !_inTimeoutMode;

			if (_inTimeoutMode)
			{
				// switch to timeout text and stopped progress bar.
				MainInstruction = _timeoutMainInstruction;
				Content = _timeoutContent;
				progressBarInfo.Style = ProgressBarStyle.Continuous;
				progressBarInfo.Visible = false;
			}
			else
			{
				// switch to marquee progress bar
				progressBarInfo.Style = ProgressBarStyle.Marquee;
				progressBarInfo.Visible = true;
			}
		}

		private void TimerTimeout_Tick(object sender, EventArgs e)
		{
			timerTimeout.Stop();
			SetUiEnabledForTimeout(true);
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void ButtonRetry_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Retry;
			Close();
		}

		private void TimerCheckForDone_Tick(object sender, EventArgs e)
		{
			if (_checkForDoneFunc != null)
			{
				bool done = _checkForDoneFunc();

				if (done)
				{
					timerTimeout.Stop();
					DialogResult = DialogResult.Cancel;
					Close();
					return;
				}
			}

			if (_retryAction != null && _inTimeoutMode == false)
			{
				if (_retryStopwatch.Elapsed > _retryTimeout)
				{
					_retryStopwatch.Stop();
					_retryStopwatch.Reset();
					_retryStopwatch.Start();

					_retryAction(_retryContext);
				}
			}

			if (_hideDialog && _hideDialogStopwatch.Elapsed > _hideDialogTimeout)
			{
				_hideDialog = false;
				Opacity = 1;
			}
		}
	}
}
