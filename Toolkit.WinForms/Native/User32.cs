using System;
using System.Runtime.InteropServices;

using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Native methods, constants, and structs for User32 operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal class User32
	{
		public const string NativeLibrary = "user32.dll";

		public struct IconInfo
		{
			public bool fIcon;
			public int xHotspot;
			public int yHotspot;
			public IntPtr hbmMask;
			public IntPtr hbmColor;
		}

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyIcon(IntPtr hIcon);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetSysColorBrush(int nIndex);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int SendMessage(HandleRef hWnd, WM wMsg, IntPtr wParam, IntPtr lParam);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int SendMessage(IntPtr hWnd, WM wMsg, bool wParam, int lParam);

		[DllImport(NativeLibrary, EntryPoint = "SetClassLong", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetClassLongPtr32(IntPtr hWnd, int nIndex, uint dwNewLong);

		[DllImport(NativeLibrary, EntryPoint = "SetClassLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetClassLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShowWindow(HandleRef handle, ShowWindowCommands flags);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ValidateRect(IntPtr hWnd, ref Gdi32.RECT lpRect);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ValidateRect(IntPtr hWnd, IntPtr lpRect);
	}
}
