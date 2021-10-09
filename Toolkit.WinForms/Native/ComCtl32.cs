using System;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Native methods, constants, and structs for CommCtl32 operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal static partial class ComCtl32
	{
		public const string NativeLibrary = "comctl32.dll";

		public const int ILD_TRANSPARENT = 0x00000001;

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, int flags);

		/// <summary>
		/// Common Control Messages -- CommCtrl.h
		/// </summary>
		public static class CCM
		{
			public const int FIRST = 0x2000;
			public const int LAST = FIRST + 0x200;

			public const int SETBKCOLOR = FIRST + 1;
			public const int SETCOLORSCHEME = FIRST + 2;
			public const int GETCOLORSCHEME = FIRST + 3;
			public const int GETDROPTARGET = FIRST + 4;
			public const int SETUNICODEFORMAT = FIRST + 5;
			public const int GETUNICODEFORMAT = FIRST + 6;
			public const int SETVERSION = FIRST + 0x7;
			public const int GETVERSION = FIRST + 0x8;
			public const int SETNOTIFYWINDOW = FIRST + 0x9;
			// 0xA skipped
			public const int SETWINDOWTHEME = FIRST + 0xB;
			public const int DPISCALE = FIRST + 0xC; // wParam = Awareness
		}
	}
}
