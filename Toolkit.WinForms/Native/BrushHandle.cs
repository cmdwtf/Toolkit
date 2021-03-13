using System;

using Microsoft.Win32.SafeHandles;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// A wrapper class for a safe Brush handle.
	/// </summary>
	public class BrushHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public BrushHandle() : this(IntPtr.Zero) { }

		public BrushHandle(IntPtr handle)
			: base(true)
		{
			SetHandle(handle);
		}

		protected override bool ReleaseHandle()
		{
			Gdip.DeleteObject(handle);
			return true;
		}
	}
}
