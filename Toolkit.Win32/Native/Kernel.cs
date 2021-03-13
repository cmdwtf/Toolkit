using System;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.Win32.Native
{
	/// <summary>
	/// Native methods, constants, and structs for Kernel operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal class Kernel
	{
		[DllImport(Libraries.Kernel, CharSet = CharSet.Auto)]
		public static extern bool CloseHandle(IntPtr handle);
	}
}
