using System;
using System.Runtime.InteropServices;

using static cmdwtf.Toolkit.WinForms.Native.ComCtl32;
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
			public LPARAM hbmMask;
			public LPARAM hbmColor;
		}

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LPARAM CreateIconIndirect(ref IconInfo icon);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyIcon(LPARAM hIcon);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LPARAM GetDC(HWND hWnd);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetIconInfo(LPARAM hIcon, ref IconInfo pIconInfo);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LPARAM GetSysColorBrush(int nIndex);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT HideCaret(HWND hWnd);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT SendMessage(HWNDRef hWnd, WM wMsg, LPARAM wParam, LPARAM lParam);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT SendMessage(HWND hWnd, WM wMsg, bool wParam, LPARAM lParam);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT SendMessage(HWND hWnd, uint wMsg, WPARAM wParam, LPARAM lParam);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT SendMessage(HWND hWnd, uint wMsg, DWORD wParam, DWORD lParam);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern LRESULT SendMessage(HWND hWnd, ComboBox.CB wMsg, WPARAM wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

		[DllImport(NativeLibrary, EntryPoint = "SetClassLong", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern DWORD SetClassLongPtr32(HWND hWnd, int nIndex, DWORD dwNewLong);

		[DllImport(NativeLibrary, EntryPoint = "SetClassLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern WPARAM SetClassLongPtr64(HWND hWnd, int nIndex, LPARAM dwNewLong);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShowWindow(HWNDRef hWnd, ShowWindowCommands flags);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ValidateRect(HWND hWnd, ref Gdi32.RECT lpRect);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ValidateRect(HWND hWnd, LPARAM lpRect);
	}
}
