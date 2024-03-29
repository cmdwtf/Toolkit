using cmdwtf.Toolkit.WinForms.Native;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Tools related to debug functionality, particularly allocating a console window.
	/// </summary>
	public static class Debug
	{
		/// <summary>
		/// A constant value that is <c>true</c> if this is a debug build, otherwise <c>false</c>.
		/// </summary>
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
				Assembly.CallingAssembly != System.Reflection.Assembly.GetAssembly(typeof(Debug));

			if (shouldCreateConsole)
			{
				Kernel32.AllocConsole();
				Kernel32.AttachConsole(-1);
			}
		}
	}
}

