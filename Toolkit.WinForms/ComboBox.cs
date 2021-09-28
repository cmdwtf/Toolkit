using System;
using System.Windows.Forms;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Extensions related to comboboxes.
	/// </summary>
	public static class ComboxBox
	{
		public enum CB
		{
			SETCUEBANNER = 0x1703
		}

		public static int ComboBoxSetCueBanner(IntPtr handle, string cueBanner)
			=> Native.User32.SendMessage(handle, CB.SETCUEBANNER, UIntPtr.Zero, cueBanner);

		public static int SetCueBanner(this ComboBox comboBox, string cueBanner)
			=> ComboBoxSetCueBanner(comboBox.Handle, cueBanner);
	}
}
