using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.Native
{
	/// <summary>
	/// Constant values for names of DLLs to use in <see cref="DllImportAttribute"/>
	/// </summary>
	internal static class Libraries
	{
		public const string AdvancedApi = "advapi32.dll";
		public const string Kernel = "kernel32.dll";
		public const string Multimedia = "winmm.dll";
	}
}
