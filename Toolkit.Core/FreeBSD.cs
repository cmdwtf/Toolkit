using System.Runtime.InteropServices;


namespace cmdwtf.Toolkit
{
	/// <summary>
	/// FreeBSD related tools. Sparse at the moment.
	/// </summary>
	public static class FreeBSD
	{
		/// <summary>
		/// Gets a value indicating whether this host is FreeBSD.
		/// </summary>
		/// <value>
		///   <c>true</c> if this host is free BSD; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFreeBSD =>
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
			RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
#else
			false;
#endif // NET5_0_OR_GREATER
	}
}
