using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using DWORD = System.UInt32;
using System.Diagnostics;
using cmdwtf.Toolkit.Win32.Native;

namespace cmdwtf.Toolkit.Win32
{
	/// <summary>
	/// A utility class for stealing focus from another window by injecting into it's input loop.
	/// Raymond Chen says don't steal focus. But if you're going to, just be prepared to suffer
	/// for your malfeasance. If you're interested in learning more, read on.
	///
	/// Okay, so. Let's talk about focus. In general, Windows thinks it is totally not kosher to steal focus.
	/// In general, I have to agree with them. Could you imagine a world where every application thought it's more
	/// important than other ones? It'd be absolute hell on the user!
	/// 
	/// Raymond Chen goes into why focus is like love: you can't steal it, it has to be given to you:
	/// https://devblogs.microsoft.com/oldnewthing/20090220-00/?p=19083
	/// 
	/// However, we know better, and we are better than every other application! (lol)
	/// Seems some folx found a way to work around this denial-of-service when it comes to SetForegroundWindow()
	/// 
	/// Turns out, if you impersonate the other application by attaching to their thread input, you get to call
	/// other functions more ore less as if you were them. You can see this workaround depicted here:
	/// http://www.tek-tips.com/faqs.cfm?fid=4262 and here: http://stackoverflow.com/a/20691831/944605
	/// 
	/// What you'll find in this class is a solution based on those that I've found
	/// works in several cases, including for Unity3D, and may not work
	/// at all with newer versions of Windows. This isn't pretty, and it's probably
	/// a violation of some sort of intergalactic law. But we're not here to judge.
	/// </summary>
	/// <seealso href="https://devblogs.microsoft.com/oldnewthing/20090220-00/?p=19083"/>
	public class FocusThief
	{
		private readonly HWND _targetHwnd = HWND.Zero;

		/// <summary>
		/// Creates a new <see cref="FocusThief"/> instance, targeting the currently active window.
		/// This should be created on the thread that owns that window.
		/// </summary>
		public FocusThief()
		{
			_targetHwnd = User32.GetActiveWindow();

			if (_targetHwnd != HWND.Zero)
			{
				Debug.WriteLine($"Stored hwnd: 0x{_targetHwnd:X8}");
			}
			else
			{
				Debug.WriteLine("Couldn't get the hwnd for our window! Make sure you're initializing from the main thread.");
			}
		}

		/// <summary>
		/// <see langword="true"/>, if the current foreground window is one of our windows.
		/// </summary>
		public static bool IsActivated
		{
			get
			{
				HWND activeHwnd = User32.GetForegroundWindow();
				if (activeHwnd == HWND.Zero)
				{
					// No window is currently activated
					return false;
				}

				int ourPid =
#if NET5_0_OR_GREATER
					System.Environment.ProcessId;
#else
					Process.GetCurrentProcess().Id;
#endif // NET471_OR_GREATER

				_ = User32.GetWindowThreadProcessId(activeHwnd, out int activeWindowPid);

				return activeWindowPid == ourPid;
			}
		}

		/// <summary>
		/// Steals focus from the active foreground window.
		/// </summary>
		/// <returns><see langword="true"/>, if I managed to be a master thief.</returns>
		/// <exception cref="System.InvalidOperationException">If the target HWND is zero.</exception>
		/// <exception cref="System.ComponentModel.Win32Exception">
		/// If the call to the native <see cref="User32.ShowWindow(HWND, int)"/>
		/// or <see cref="User32.AttachThreadInput(DWORD, DWORD, bool)"/> fails.
		/// </exception>
		public bool StealFocus() => ShowWindow(User32.SW.SHOW);

		/// <summary>
		/// Show the stored window. Equivalent to <seealso cref="StealFocus"/>.
		/// </summary>
		/// <returns><see langword="true"/> if successful.</returns>
		/// <exception cref="System.InvalidOperationException">If the target HWND is zero.</exception>
		/// <exception cref="System.ComponentModel.Win32Exception">
		/// If the call to the native <see cref="User32.ShowWindow(HWND, int)"/>
		/// or <see cref="User32.AttachThreadInput(DWORD, DWORD, bool)"/> fails.
		/// </exception>
		public bool Show() => ShowWindow(User32.SW.SHOW);

		/// <summary>
		/// Restore the stored window.
		/// </summary>
		/// <returns><see langword="true"/> if successful.</returns>
		/// <exception cref="System.InvalidOperationException">If the target HWND is zero.</exception>
		/// <exception cref="System.ComponentModel.Win32Exception">
		/// If the call to the native <see cref="User32.ShowWindow(HWND, int)"/>
		/// or <see cref="User32.AttachThreadInput(DWORD, DWORD, bool)"/> fails.
		/// </exception>
		public bool Restore() => ShowWindow(User32.SW.RESTORE);

		/// <summary>
		/// Minimize the stored window.
		/// </summary>
		/// <returns><see langword="true"/> if successful.</returns>
		/// <exception cref="System.InvalidOperationException">If the target HWND is zero.</exception>
		/// <exception cref="System.ComponentModel.Win32Exception">
		/// If the call to the native <see cref="User32.ShowWindow(HWND, int)"/>
		/// or <see cref="User32.AttachThreadInput(DWORD, DWORD, bool)"/> fails.
		/// </exception>
		public bool Minimize() => ShowWindow(User32.SW.MINIMIZE);

		/// <summary>
		/// Maximize the stored window.
		/// </summary>
		/// <returns><see langword="true"/> if successful.</returns>
		/// <exception cref="System.InvalidOperationException">If the target HWND is zero.</exception>
		/// <exception cref="System.ComponentModel.Win32Exception">
		/// If the call to the native <see cref="User32.ShowWindow(HWND, int)"/>
		/// or <see cref="User32.AttachThreadInput(DWORD, DWORD, bool)"/> fails.
		/// </exception>
		public bool Maximize() => ShowWindow(User32.SW.MAXIMIZE);

		/// <summary>
		/// Show the stored window with the given window state.
		/// </summary>
		/// <param name="state">The state to pass to the native ShowWindow function.</param>
		/// <returns><see langword="true"/> if successful.</returns>
		/// <exception cref="System.InvalidOperationException">If the target HWND is zero.</exception>
		/// <exception cref="System.ComponentModel.Win32Exception">
		/// If the call to the native <see cref="User32.ShowWindow(HWND, int)"/>
		/// or <see cref="User32.AttachThreadInput(DWORD, DWORD, bool)"/> fails.
		/// </exception>
		public bool ShowWindow(int state) => ShowWindow(state, _targetHwnd);

		private static bool ShowWindow(int state, HWND hwnd)
		{
			if (hwnd == HWND.Zero)
			{
				throw new System.InvalidOperationException("Can't StealFocus: Not initialized.");
			}

			DWORD dwCurrentThread = Kernel32.GetCurrentThreadId();

			DWORD dwForegroundThread = User32.GetWindowThreadProcessId(User32.GetForegroundWindow(), out _);

			if (!User32.AttachThreadInput(dwCurrentThread, dwForegroundThread, true))
			{
				int error = Marshal.GetLastWin32Error();
				throw new System.ComponentModel.Win32Exception(error);
			}

			User32.BringWindowToTop(hwnd);
			bool status = User32.ShowWindow(hwnd, state);

			if (status == false)
			{
				int error = Marshal.GetLastWin32Error();
				throw new System.ComponentModel.Win32Exception(error);
			}

			if (!User32.AttachThreadInput(dwCurrentThread, dwForegroundThread, false))
			{
				int error = Marshal.GetLastWin32Error();
				throw new System.ComponentModel.Win32Exception(error);
			}

			return status;
		}
	}
}
