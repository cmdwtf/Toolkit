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
			public const uint FIRST = 0x2000;
			public const uint LAST = FIRST + 0x200;

			public const uint SETBKCOLOR = FIRST + 1;
			public const uint SETCOLORSCHEME = FIRST + 2;
			public const uint GETCOLORSCHEME = FIRST + 3;
			public const uint GETDROPTARGET = FIRST + 4;
			public const uint SETUNICODEFORMAT = FIRST + 5;
			public const uint GETUNICODEFORMAT = FIRST + 6;
			public const uint SETVERSION = FIRST + 0x7;
			public const uint GETVERSION = FIRST + 0x8;
			public const uint SETNOTIFYWINDOW = FIRST + 0x9;
			// 0xA skipped
			public const uint SETWINDOWTHEME = FIRST + 0xB;
			public const uint DPISCALE = FIRST + 0xC; // wParam = Awareness
		}
	}
}
