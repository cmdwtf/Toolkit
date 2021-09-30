

using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// MacOS related tools. Sparse at the moment.
	/// </summary>
	public static class MacOS
	{
		/// <summary>
		/// Gets a value indicating whether this host is mac os.
		/// </summary>
		/// <value>
		///   <c>true</c> if this host is mac os; otherwise, <c>false</c>.
		/// </value>
		public static bool IsMacOS =>
#if NET471_OR_GREATER || NETSTANDARD1_0 || NET5_0_OR_GREATER
			RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#else
			false;
#endif // NET5_0_OR_GREATER
	}
}
