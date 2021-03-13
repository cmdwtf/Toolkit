using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// FreeBSD related tools. Sparse at the moment.
	/// </summary>
	public static class FreeBSD
	{
		public static bool IsFreeBSD => RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
	}
}
