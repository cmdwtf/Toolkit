using System.Runtime.InteropServices;

using DWORD = System.UInt32;
using HWND = System.IntPtr;

namespace cmdwtf.Toolkit.Win32.Native
{
	/// <summary>
	/// Native methods, constants, and structs for User32 operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	internal class User32
	{
		public const string NativeLibrary = "user32.dll";

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern HWND GetActiveWindow();

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetForegroundWindow(HWND hWnd);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern HWND GetForegroundWindow();

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern DWORD GetWindowThreadProcessId(HWND handle, out int processId);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern HWND SetFocus(HWND hwnd);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool AttachThreadInput(DWORD idAttach, DWORD idAttachTo, bool fAttach);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool ShowWindow(HWND hWnd, int nCmdShow);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool BringWindowToTop(HWND hWnd);

		/// <summary>
		/// <see cref="ShowWindow(HWND, int)"/> nCmdShows.
		/// </summary>
		public static class SW
		{
			public const int MAXIMIZE = 3;
			public const int MINIMIZE = 6;
			public const int RESTORE = 9;
			public const int SHOW = 9;
		}
	}
}
