using System.Runtime.InteropServices;


namespace cmdwtf.Toolkit
{
	/// <summary>
	/// FreeBSD related tools. Sparse at the moment.
	/// </summary>
	public static class FreeBSD
	{
		public static bool IsFreeBSD =>
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
			RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
#else
			throw new System.NotSupportedException($"{nameof(IsFreeBSD)} depends on OSPlatform.FreeBSD, which isn't available until .NET 5.0. / .NET Core 3.0");
#endif // NET5_0_OR_GREATER
	}
}
