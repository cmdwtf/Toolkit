using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.Win32.Native
{
	/// <summary>
	/// Constant values for names of DLLs to use in <see cref="DllImportAttribute"/>
	/// </summary>
	internal static class Libraries
	{
		public const string AdvancedApi = "advapi32.dll";
		public const string Kernel = "kernel32.dll";
	}
}
