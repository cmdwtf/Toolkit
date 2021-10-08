using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// Linux related tools. Sparse at the moment.
	/// </summary>
	public static class Linux
	{
		/// <summary>
		/// Gets a value indicating whether host instance is linux.
		/// </summary>
		/// <value>
		///   <c>true</c> if this host is linux; otherwise, <c>false</c>.
		/// </value>
		/// <exception cref="System.NotSupportedException"></exception>
		public static bool IsLinux =>
#if NET471_OR_GREATER || NETSTANDARD1_0 || NET5_0_OR_GREATER
			RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#else
			throw new System.NotSupportedException($"{nameof(IsLinux)} depends on RuntimeInformation.IsOSPlatform, which isn't available until .NET 4.7.1");
#endif // NET5_0_OR_GREATER
	}
}
