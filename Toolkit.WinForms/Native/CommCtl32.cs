using System;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Native methods, constants, and structs for CommCtl32 operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal static class CommCtl32
	{
		public const string NativeLibrary = "comctl32.dll";

		public const int ILD_TRANSPARENT = 0x00000001;

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, int flags);
	}
}
