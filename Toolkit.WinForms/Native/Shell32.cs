using System;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Native methods, constants, and structs for Shell32 operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal static class Shell32
	{
		public const string NativeLibrary = "shell32.dll";

		public const uint SHGFI_ICON = 0x000000100;

		public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

		public const uint SHGFI_LARGEICON = 0x000000000;
		public const uint SHGFI_SMALLICON = 0x000000001;
		public const uint SHGFI_OPENICON = 0x000000002;
		public const uint SHGFI_SYSSMALL = 0x000000003;
		public const uint SHGFI_SHELLICONSIZE = 0x000000004;
		public const uint SHGFI_ADDOVERLAYS = 0x000000020;
		public const uint SHGFI_SYSICONINDEX = 0x000004000;
		public const uint SHGFI_SELECTED = 0x000010000;

		public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct SHFILEINFO
		{
			public IntPtr hIcon;
			public IntPtr iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		};

		[DllImport(NativeLibrary, CharSet = CharSet.Unicode)]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);
	}
}
