using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Constant values for names of DLLs to use in <see cref="DllImportAttribute"/>
	/// </summary>
	internal static class Libraries
	{
		public const string User32 = "user32.dll";
		public const string Gdi32 = "gdi32.dll";
		public const string Shell32 = "shell32.dll";
		public const string ComCtl32 = "comctl32.dll";
	}
}
