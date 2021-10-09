using System;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Native methods, constants, and structs for Kernel operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	internal class Kernel32
	{
		public const string NativeLibrary = "kernel32.dll";

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AllocConsole();

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool AttachConsole(int pid);

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr handle);
	}
}
