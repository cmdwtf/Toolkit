using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using static cmdwtf.Toolkit.WinForms.Native.Gdi32;
using static cmdwtf.Toolkit.WinForms.Native.User32;
using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Extensions relating to windows themselves. (As in, the application window,
	/// not the operating system Windows.)
	/// </summary>
	public static partial class Forms
	{
		/// <summary>
		/// The ValidateRect function validates the client area within a rectangle
		/// by removing the rectangle from the update region of the specified window.
		/// </summary>
		/// <param name="window">The window to validate an area on.</param>
		/// <param name="rect">The rectangle of area to validate.</param>
		/// <returns>True on succcess, false on failure.</returns>
		public static bool ValidateRect(this IWin32Window window, Rectangle rect)
		{
			var nativeRect = new RECT(rect);
			return Native.User32.ValidateRect(window.Handle, ref nativeRect);
		}

		/// <summary>
		/// Replaces the specified 32-bit (long) value at the specified offset into the extra class
		/// memory or the WNDCLASSEX structure for the class to which the specified window belongs.
		/// </summary>
		/// <param name="hWnd">The handle to the window of the background to set.</param>
		/// <param name="nIndex">The value to be replaced.</param>
		/// <param name="dwNewLong">The replacement value.</param>
		/// <returns>
		/// If the function succeeds, the return value is the previous value of the specified 32-bit integer.
		/// If the value was not previously set, the return value is zero.
		/// If the function fails, the return value is zero.To get extended error information,
		/// call GetLastError.
		/// </returns>
		/// <seealso>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setclasslonga</seealso>
		public static IntPtr SetClassLong(this IntPtr hWnd, int nIndex, IntPtr dwNewLong)
		{
			unchecked
			{
				//check for x64
				if (IntPtr.Size > 4)
				{
					return SetClassLongPtr64(hWnd, nIndex, dwNewLong);
				}
				else
				{
					return SetClassLongPtr32(hWnd, nIndex, (uint)dwNewLong.ToInt32());
				}
			}
		}

		/// <summary>
		/// Shows a window without activating the window or giving it focus.
		/// </summary>
		/// <param name="window">The window to show.</param>
		/// <returns>True, if successful.</returns>
		public static bool ShowWithNoFoucs(this IWin32Window window)
			=> ShowWindow(new HandleRef(window, window.Handle), ShowWindowCommands.ShowNA);
	}
}
