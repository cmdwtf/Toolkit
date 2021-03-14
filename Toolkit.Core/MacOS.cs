

using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// MacOS related tools. Sparse at the moment.
	/// </summary>
	public static class MacOS
	{
		public static bool IsMacOS =>
#if NET471_OR_GREATER || NETSTANDARD1_0 || NET5_0_OR_GREATER
			RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#else
			throw new System.NotSupportedException($"{nameof(IsMacOS)} depends on RuntimeInformation.IsOSPlatform, which isn't available until .NET 4.7.1");
#endif // NET5_0_OR_GREATER
	}
}
