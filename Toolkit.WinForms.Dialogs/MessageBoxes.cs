using System;
using System.Drawing;

using Ookii.Dialogs.WinForms;

using DialogResult = System.Windows.Forms.DialogResult;
using IWin32Window = System.Windows.Forms.IWin32Window;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;

namespace cmdwtf.Toolkit.WinForms.Dialogs
{
	/// <summary>
	/// A collection of shortcuts to use <see cref="Ookii.Dialogs.WinForms.TaskDialog"/> with the ease of
	/// <see cref="System.Windows.Forms.MessageBox"/>.
	/// </summary>
	public static class MessageBoxes
	{
		internal static DialogResult Show(IWin32Window owner, string text, string caption,
			MessageBoxButtons buttons = MessageBoxButtons.OK,
			MessageBoxIcon image = MessageBoxIcon.None, string content = null)
			=> Show(owner, text, caption, buttons, image, null, content);

		internal static DialogResult Show(IWin32Window owner, string text, string caption,
			MessageBoxButtons buttons, MessageBoxIcon image, Icon customIcon, string content = null)
		{
			var box = new TaskDialog()
			{
				MainInstruction = text,
				Content = content,
				WindowTitle = caption ?? "Error",
				MainIcon = (customIcon != null) ? TaskDialogIcon.Custom : image.ToTaskDialogIcon(),
				AllowDialogCancellation = buttons.HasCancelButton(),
				CenterParent = owner != null,
				CustomMainIcon = customIcon,
			};

			switch (buttons)
			{
				default:
				case MessageBoxButtons.OK:
					box.Buttons.Add(new TaskDialogButton(ButtonType.Ok));
					break;
				case MessageBoxButtons.OKCancel:
					box.Buttons.Add(new TaskDialogButton(ButtonType.Ok));
					box.Buttons.Add(new TaskDialogButton(ButtonType.Cancel));
					break;
				case MessageBoxButtons.AbortRetryIgnore:
					box.Buttons.Add(new TaskDialogButton("&Abort"));
					box.Buttons.Add(new TaskDialogButton(ButtonType.Retry));
					box.Buttons.Add(new TaskDialogButton("&Ignore"));
					break;
				case MessageBoxButtons.YesNoCancel:
					box.Buttons.Add(new TaskDialogButton(ButtonType.Yes));
					box.Buttons.Add(new TaskDialogButton(ButtonType.No));
					box.Buttons.Add(new TaskDialogButton(ButtonType.Cancel));
					break;
				case MessageBoxButtons.YesNo:
					box.Buttons.Add(new TaskDialogButton(ButtonType.Yes));
					box.Buttons.Add(new TaskDialogButton(ButtonType.No));
					break;
				case MessageBoxButtons.RetryCancel:
					box.Buttons.Add(new TaskDialogButton(ButtonType.Retry));
					box.Buttons.Add(new TaskDialogButton(ButtonType.Cancel));
					break;
			}

			TaskDialogButton pressed = box.ShowDialog(owner);

			if (pressed == null)
			{
				return DialogResult.Cancel;
			}

			return pressed.ToDialogResult();
		}

		public static DialogResult Error(IWin32Window owner, string text, string caption = null, string content = null)
			=> Show(owner, text, caption ?? "Error", image: MessageBoxIcon.Error, content: content);
		public static DialogResult Error(string text, string caption = null, string content = null)
			=> Error(null, text, caption, content);

		public static DialogResult Success(IWin32Window owner, string text, string caption = null, string content = null)
			=> Show(owner, text, caption ?? "Success", image: MessageBoxIcon.Information, content: content);
		public static DialogResult Success(string text, string caption = null, string content = null)
			=> Success(null, text, caption, content);

		public static DialogResult Information(IWin32Window owner, string text, string caption = null, string content = null)
			=> Show(owner, text, caption ?? "Information", image: MessageBoxIcon.Information, content: content);
		public static DialogResult Information(string text, string caption = null, string content = null)
			=> Information(null, text, caption, content);

		public static DialogResult Warning(IWin32Window owner, string text, string caption = null, string content = null)
			=> Show(owner, text, caption ?? "Warning", image: MessageBoxIcon.Warning, content: content);
		public static DialogResult Warning(string text, string caption = null, string content = null)
			=> Warning(null, text, caption, content);

		public static DialogResult YesNo(IWin32Window owner, string text, string caption = null, string content = null)
			=> Show(owner, text, caption ?? "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, content: content);
		public static DialogResult YesNo(string text, string caption = null, string content = null)
			=> YesNo(null, text, caption, content);

		public static int TaskQuestion(string mainText, string text, string caption, string[] buttonOptions, string[] buttonNotes = null)
		{
			var td = new TaskDialog
			{
				AllowDialogCancellation = true,
				MainInstruction = mainText,
				Content = text,
				MainIcon = TaskDialogIcon.Warning,
				WindowTitle = caption,
				ButtonStyle = TaskDialogButtonStyle.CommandLinks,
				CenterParent = true,
			};

			for (int scan = 0; scan < buttonOptions.Length; ++scan)
			{
				string option = buttonOptions[scan];
				var button = new TaskDialogButton(option)
				{
					CommandLinkNote = buttonNotes?[scan] ?? null,
				};
				td.Buttons.Add(button);
			}

			TaskDialogButton userChoice = td.ShowDialog();

			if (userChoice == null)
			{
				return -1;
			}

			for (int scan = 0; scan < buttonOptions.Length; ++scan)
			{
				if (userChoice.Text == buttonOptions[scan])
				{
					return scan;
				}
			}

			return -1;
		}

		private static TaskDialogIcon ToTaskDialogIcon(this MessageBoxIcon icon)
		{
			return icon switch
			{
				MessageBoxIcon.Error => TaskDialogIcon.Error,
				MessageBoxIcon.Question => TaskDialogIcon.Information,
				MessageBoxIcon.Exclamation => TaskDialogIcon.Warning,
				MessageBoxIcon.Asterisk => TaskDialogIcon.Information,
				MessageBoxIcon.None => TaskDialogIcon.Custom,
				_ => TaskDialogIcon.Custom,
			};
		}

		private static bool HasCancelButton(this MessageBoxButtons buttons)
			=> buttons is MessageBoxButtons.OKCancel or MessageBoxButtons.YesNoCancel or MessageBoxButtons.RetryCancel;

		private static DialogResult ToDialogResult(this TaskDialogButton button)
		{
			return button.ButtonType switch
			{
				ButtonType.Custom => button.Text switch
				{
					"Abort" => DialogResult.Abort,
					"Ignore" => DialogResult.Ignore,
					_ => throw new ArgumentOutOfRangeException(nameof(button.Text), "Unhandled button text type.")
				},
				ButtonType.Ok => DialogResult.OK,
				ButtonType.Yes => DialogResult.Yes,
				ButtonType.No => DialogResult.No,
				ButtonType.Cancel => DialogResult.Cancel,
				ButtonType.Retry => DialogResult.Retry,
				ButtonType.Close => DialogResult.Cancel,
				_ => throw new ArgumentOutOfRangeException(nameof(button.ButtonType), "Unhandled button type.")
			};
		}
	}
}
