using System;

using Microsoft.Win32.SafeHandles;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// A wrapper class for a safe Brush handle.
	/// </summary>
	public class BrushHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BrushHandle"/> class.
		/// </summary>
		protected BrushHandle() : this(IntPtr.Zero) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BrushHandle"/> class.
		/// </summary>
		/// <param name="handle">The handle this instance should own.</param>
		public BrushHandle(IntPtr handle)
			: base(true)
		{
			SetHandle(handle);
		}

		/// <summary>
		/// Frees the native brush handle.
		/// </summary>
		/// <returns>
		///   <see langword="true" /> if the handle is released successfully; otherwise, in the event of a catastrophic failure, <see langword="false" />. In this case, it generates a releaseHandleFailed Managed Debugging Assistant.
		/// </returns>
		protected override bool ReleaseHandle()
		{
			Gdi32.DeleteObject(handle);
			return true;
		}
	}
}
