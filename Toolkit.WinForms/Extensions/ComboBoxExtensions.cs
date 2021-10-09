using System;
using System.Windows.Forms;

using static cmdwtf.Toolkit.WinForms.Native.ComCtl32.ComboBox;

namespace cmdwtf.Toolkit.WinForms.Extensions
{
	/// <summary>
	/// Extensions related to comboboxes.
	/// </summary>
	public static class ComboxBoxExtensions
	{
		/// <summary>
		/// Sets the cue banner on a combobox handle.
		/// </summary>
		/// <param name="handle">The handle to the combobox.</param>
		/// <param name="cueBanner">The cue banner to set.</param>
		/// <returns>The result of the native SendMessage call.</returns>
		public static int ComboBoxSetCueBanner(IntPtr handle, string cueBanner)
			=> Native.User32.SendMessage(handle, CB.SETCUEBANNER, UIntPtr.Zero, cueBanner);

		/// <summary>
		/// Sets the cue banner on a combobox.
		/// </summary>
		/// <param name="comboBox">The combobox.</param>
		/// <param name="cueBanner">The cue banner to set.</param>
		/// <returns>The result of the native SendMessage call.</returns>
		public static int SetCueBanner(this ComboBox comboBox, string cueBanner)
			=> ComboBoxSetCueBanner(comboBox.Handle, cueBanner);
	}
}
