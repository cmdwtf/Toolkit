using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// Linux related tools. Sparse at the moment.
	/// </summary>
	public static class Linux
	{
		public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
	}
}
