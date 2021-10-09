using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cmdwtf.Toolkit.WinForms.Controls
{
	/// <summary>
	/// A RichTextBox that cannot be edited, with a hidden caret.
	/// </summary>
	[ToolboxItem(true)]
	public class ReadOnlyRichTextBox : RichTextBox, IDisposable
	{
		/// <summary>
		/// <see cref="ReadOnlyRichTextBox"/> is always read only. This will always return true.
		/// This hides <see cref="TextBoxBase.ReadOnly"/>.
		/// </summary>
		[DefaultValue(true)]
		public static new bool ReadOnly
		{
			get => true;
			set { }
		}

		/// <summary>
		/// <see cref="ReadOnlyRichTextBox"/> is never a tab stop. This will always return false.
		/// This hides <see cref="Control.TabStop"/>.
		/// </summary>
		[DefaultValue(false)]
		public static new bool TabStop
		{
			get => false;
			set { }
		}

		/// <summary>
		/// Creates a new instance of the <see cref="ReadOnlyRichTextBox"/> class.
		/// </summary>
		public ReadOnlyRichTextBox()
		{
			MouseDown += new MouseEventHandler(ReadOnlyRichTextBox_Mouse);
			MouseUp += new MouseEventHandler(ReadOnlyRichTextBox_Mouse);
			MouseMove += new MouseEventHandler(ReadOnlyRichTextBox_Mouse);
			base.ReadOnly = true;
			base.TabStop = false;
			_ = Native.User32.HideCaret(Handle);
		}

		private void ReadOnlyRichTextBox_Mouse(object sender, MouseEventArgs e)
		{
			SelectionLength = 0;
			_ = Native.User32.HideCaret(Handle);
		}

		private void ReadOnlyRichTextBox_Resize(object sender, EventArgs e) => _ = Native.User32.HideCaret(Handle);

		#region RichTextBox overrides

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.
		/// As well, hides the caret.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			_ = Native.User32.HideCaret(Handle);
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event.
		/// As well, hides the caret.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnEnter(EventArgs e)
		{
			_ = Native.User32.HideCaret(Handle);
			base.OnEnter(e);
		}

		#endregion RichTextBox overrides
	}
}
