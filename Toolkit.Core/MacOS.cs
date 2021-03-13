

using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// MacOS related tools. Sparse at the moment.
	/// </summary>
	public static class MacOS
	{
		public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
	}
}
