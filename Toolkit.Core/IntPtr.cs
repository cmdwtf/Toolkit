
namespace cmdwtf.Toolkit
{
	namespace Pointers
	{
		/// <summary>
		/// Tools and utilities related to <see cref="IntPtr"/>
		/// </summary>
		public static class IntPtr
		{
			/// <summary>
			/// An easy to use reference to the -1 that many Win32 API calls use as a failure condition.
			/// </summary>
			public static readonly System.IntPtr MinusOne = System.IntPtr.Zero - 1;
		}
	}
}
