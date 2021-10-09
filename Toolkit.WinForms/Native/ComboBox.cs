namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Native methods, constants, and structs for CommCtl32 operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal static partial class ComCtl32
	{
		/// <summary>
		/// ComboBox styles and messages.
		/// </summary>
		public static class ComboBox
		{

			/// <summary>
			/// Native ComboBox messages. This type is incomplete.
			/// </summary>
			public enum CB
			{
				/// <summary>
				/// The set cue banner message.
				/// </summary>
				SETCUEBANNER = 0x1703
			}
		}

	}
}
