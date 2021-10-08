using System;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.Win32.Native
{
	/// <summary>
	/// Native methods, constants, and structs for Kernel operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal class Kernel32
	{
		public const string NativeLibrary = "kernel32.dll";

		[DllImport(NativeLibrary, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr handle);
	}
}
