using System.Runtime.InteropServices;

using SRAssembly = System.Reflection.Assembly;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Tools related to debug functionality, particularly allocating a console window.
	/// </summary>
	public static class Debug
	{
#if DEBUG
		public const bool IsDebugBuild = true;
#else
		public const bool IsDebugBuild = false;
#endif

		/// <summary>
		/// Attempts to allocate and attach a console, if this was built in debug mode,
		/// or if the calling assembly is not this assembly.
		/// </summary>
		public static void CreateConsole()
		{
			bool shouldCreateConsole = IsDebugBuild ||
				SRAssembly.GetCallingAssembly() != SRAssembly.GetAssembly(typeof(Debug));

			if (shouldCreateConsole)
			{
				Native.AllocConsole();
				Native.AttachConsole(-1);
			}
		}

		private static class Native
		{
			[DllImport("kernel32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool AllocConsole();
			[DllImport("kernel32.dll")]
			public static extern bool AttachConsole(int pid);
		}
	}
}

