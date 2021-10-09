using System.Runtime.InteropServices;

using static cmdwtf.Toolkit.WinForms.Native.ComCtl32;
using static cmdwtf.Toolkit.WinForms.Native.Windows;

using DWORD = System.UInt32;
using HWND = System.IntPtr;
using HWNDRef = System.Runtime.InteropServices.HandleRef;
using LONG = System.Int32;
using LONG_PTR = System.IntPtr;
using LPARAM = System.IntPtr;
using LRESULT = System.Int32;
using ULONG_PTR = System.UIntPtr;
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

		/// <summary>
		/// <see cref="GetWindowLong(HWND, LONG)"/> indexes.
		/// </summary>
		public static class GWL
		{
			public const int STYLE = -16;
			public const int EXSTYLE = -20;
		}

		/// <summary>
		/// <see cref="GetWindowLongPtr(HWND, LONG)"/> indexes.
		/// </summary>
		public static class GWLP
		{
			public const int WNDPROC = -4;
			public const int HINSTANCE = -6;
			public const int HWNDPARENT = -8;
			public const int ID = -12;
			public const int USERDATA = -21;
		}

		/// <summary>
		/// <see cref="GetWindowLongPtr(HWND, LONG)"/> indexes, but
		/// specifically for when HWND specifies a dialog box.
		/// </summary>
		public static class DWLP
		{
			public const int MSGRESULT = 0;
			public const int DLGPROC = MSGRESULT + sizeof(int);
			public const int USER = DLGPROC + sizeof(int);
		}

		/// <summary>
		/// <see cref="GetClassLong(HWND, LONG)"/> indexes.
		/// </summary>
		public static class GCL
		{
			public const int CBWNDEXTRA = -18;
			public const int CBCLSEXTRA = -20;
			public const int STYLE = -26;
		}

		/// <summary>
		/// <see cref="GetClassLong(HWND, LONG)"/> indexes, but for words.
		/// </summary>
		public static class GCW
		{
			public const int ATOM = -32;
		}

		/// <summary>
		/// <see cref="GetClassLongPtr(HWND, LONG)"/> indexes.
		/// </summary>
		public static class GCLP
		{
			public const int MENUNAME = -8;
			public const int HBRBACKGROUND = -10;
			public const int HCURSOR = -12;
			public const int HICON = -14;
			public const int HMODULE = -16;
			public const int WNDPROC = -24;
			public const int HICONSM = -34;
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

		#region Class Longs

		// forwards to the 64-bit aware version.
		public static LONG GetClassLong(HWND hWnd, int nIndex)
		{
			ULONG_PTR ret = GetClassLongPtr(hWnd, nIndex);
			return (int)ret.ToUInt32();
		}

		[DllImport(NativeLibrary, EntryPoint = "GetClassLongPtr", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = false)]
		public static extern ULONG_PTR GetClassLongPtr(HWND hWnd, int nIndex);

		// forwards to the 64-bit aware version.
		public static DWORD SetClassLong(HWND hWnd, int nIndex, LONG dwNewLong)
		{
			ULONG_PTR ret = SetClassLongPtr(hWnd, nIndex, new LONG_PTR(dwNewLong));
			return ret.ToUInt32();
		}

		[DllImport(NativeLibrary, EntryPoint = "SetClassLongPtr", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = false)]
		public static extern ULONG_PTR SetClassLongPtr(HWND hWnd, int nIndex, LONG_PTR dwNewLong);

		#endregion Class Longs

		#region Window Longs

		// forwards to the 64-bit aware version.
		public static LONG GetWindowLong(HWND hWnd, int nIndex)
		{
			LONG_PTR ret = GetWindowLongPtr(hWnd, nIndex);
			return ret.ToInt32();
		}

		[DllImport(NativeLibrary, EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = false)]
		public static extern LONG_PTR GetWindowLongPtr(HWND hWnd, int nIndex);

		// forwards to the 64-bit aware version.
		public static LONG SetWindowLong(HWND hWnd, int nIndex, LONG dwNewLong)
		{
			LONG_PTR ret = SetWindowLongPtr(hWnd, nIndex, new LONG_PTR(dwNewLong));
			return ret.ToInt32();
		}

		[DllImport(NativeLibrary, EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = false)]
		public static extern LONG_PTR SetWindowLongPtr(HWND hWnd, int nIndex, LONG_PTR dwNewLong);

		#endregion Window Longs

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
