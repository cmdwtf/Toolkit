using System;
using System.Runtime.InteropServices;

using static cmdwtf.Toolkit.WinForms.Native.Windows;

using DWORD = System.UInt32;
using HWND = System.IntPtr;
using HWNDRef = System.Runtime.InteropServices.HandleRef;
using LPARAM = System.IntPtr;
using LRESULT = System.Int32;
using WPARAM = System.UIntPtr;

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

		[StructLayout(LayoutKind.Sequential)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Win32 Naming")]
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
		public static extern IntPtr GetDC(HWND hWnd);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetSysColorBrush(int nIndex);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT HideCaret(HWND hWnd);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT SendMessage(HWNDRef hWnd, WM wMsg, IntPtr wParam, LPARAM lParam);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT SendMessage(HWND hWnd, WM wMsg, bool wParam, LPARAM lParam);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT SendMessage(HWND hWnd, uint wMsg, WPARAM wParam, LPARAM lParam);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT SendMessage(HWND hWnd, uint wMsg, DWORD wParam, DWORD lParam);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT SendMessage(HWND hWnd, ComboxBox.CB wMsg, WPARAM wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

		[DllImport(NativeLibrary, EntryPoint = "SetClassLong", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern DWORD SetClassLongPtr32(HWND hWnd, int nIndex, DWORD dwNewLong);

		[DllImport(NativeLibrary, EntryPoint = "SetClassLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern UIntPtr SetClassLongPtr64(HWND hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShowWindow(HWNDRef hWnd, ShowWindowCommands flags);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ValidateRect(HWND hWnd, ref Gdi32.RECT lpRect);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ValidateRect(HWND hWnd, IntPtr lpRect);
	}
}
