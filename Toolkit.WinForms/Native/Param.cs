namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	///  Helpers for creating W/LPARAM arguments for messages.
	/// </summary>
	/// <remarks>
	/// Licensed under the MIT license.
	/// Copyright (c) .NET Foundation and Contributors
	/// <see href="https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms.Primitives/src/Interop/Interop.PARAM.cs">Interop.PARAM.cs</see>
	/// </remarks>
	public static class Param
	{
		public static nint FromLowHigh(int low, int high)
			=> ToInt(low, high);

		public static nint FromLowHighUnsigned(int low, int high)
			// Convert the int to an uint before converting it to a pointer type,
			// which ensures the high dword being zero for 64-bit pointers.
			// This corresponds to the logic of the MAKELPARAM/MAKEWPARAM/MAKELRESULT
			// macros.
			=> (nint)(uint)ToInt(low, high);

		public static int ToInt(int low, int high)
			=> (high << 16) | (low & 0xffff);

		public static int HIWORD(int n)
			=> (n >> 16) & 0xffff;

		public static int LOWORD(int n)
			=> n & 0xffff;

		public static int LOWORD(nint n)
			=> LOWORD(unchecked((int)n));

		public static int HIWORD(nint n)
			=> HIWORD(unchecked((int)n));

		public static int SignedHIWORD(nint n)
			=> SignedHIWORD(unchecked((int)n));

		public static int SignedLOWORD(nint n)
			=> SignedLOWORD(unchecked((int)n));

		public static int SignedHIWORD(int n)
			=> (short)HIWORD(n);

		public static int SignedLOWORD(int n)
			=> (short)LOWORD(n);

		public static nint FromBool(bool value)
			=> value ? 1 : 0;

		/// <summary>
		///  Hard casts to <see langword="int" /> without bounds checks.
		/// </summary>
		public static int ToInt(nint param) => (int)param;

		/// <summary>
		///  Hard casts to <see langword="uint" /> without bounds checks.
		/// </summary>
		public static uint ToUInt(nint param) => (uint)param;
	}
}
