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
		/// <summary>
		/// Gets a W/LPARAM value from a given low/high value.
		/// </summary>
		/// <param name="low">The low value.</param>
		/// <param name="high">The high value.</param>
		/// <returns>The PARAM value.</returns>
		public static nint FromLowHigh(int low, int high)
			=> ToInt(low, high);

		/// <summary>
		/// Gets a W/LPARAM value from a given low/high value.
		/// </summary>
		/// <param name="low">The low value.</param>
		/// <param name="high">The high value.</param>
		/// <returns>The PARAM value.</returns>
		public static nint FromLowHighUnsigned(int low, int high)
			// Convert the int to an uint before converting it to a pointer type,
			// which ensures the high dword being zero for 64-bit pointers.
			// This corresponds to the logic of the MAKELPARAM/MAKEWPARAM/MAKELRESULT
			// macros.
			=> (nint)(uint)ToInt(low, high);

		/// <summary>
		/// Gets a W/LPARAM value from a given low/high value.
		/// </summary>
		/// <param name="low">The low value.</param>
		/// <param name="high">The high value.</param>
		/// <returns>The PARAM value as an int.</returns>
		public static int ToInt(int low, int high)
			=> (high << 16) | (low & 0xffff);

		/// <summary>
		/// Gets the HIWORD value from the given <see langword="int"/>.
		/// </summary>
		/// <param name="n">The PARAM value.</param>
		/// <returns>The HIWORD.</returns>
		public static int HIWORD(int n)
			=> (n >> 16) & 0xffff;

		/// <summary>
		/// Gets the HIWORD value from the given <see langword="nint"/>.
		/// </summary>
		/// <param name="n">The PARAM value.</param>
		/// <returns>The HIWORD.</returns>
		public static int HIWORD(nint n)
			=> HIWORD(unchecked((int)n));

		/// <summary>
		/// Gets the signed HIWORD value from the given <see langword="nint"/>.
		/// </summary>
		/// <param name="n">The PARAM value.</param>
		/// <returns>The HIWORD.</returns>
		public static int SignedHIWORD(nint n)
			=> SignedHIWORD(unchecked((int)n));

		/// <summary>
		/// Gets the signed HIWORD value from the given <see langword="int"/>.
		/// </summary>
		/// <param name="n">The PARAM value.</param>
		/// <returns>The HIWORD.</returns>
		public static int SignedHIWORD(int n)
			=> (short)HIWORD(n);

		/// <summary>
		/// Gets the LOWORD value from the given <see langword="int"/>.
		/// </summary>
		/// <param name="n">The PARAM value.</param>
		/// <returns>The LOWORD.</returns>
		public static int LOWORD(int n)
			=> n & 0xffff;

		/// <summary>
		/// Gets the LOWORD value from the given <see langword="nint"/>.
		/// </summary>
		/// <param name="n">The PARAM value.</param>
		/// <returns>The LOWORD.</returns>
		public static int LOWORD(nint n)
			=> LOWORD(unchecked((int)n));

		/// <summary>
		/// Gets the signed LOWORD value from the given <see langword="nint"/>.
		/// </summary>
		/// <param name="n">The PARAM value.</param>
		/// <returns>The LOWORD.</returns>
		public static int SignedLOWORD(nint n)
			=> SignedLOWORD(unchecked((int)n));

		/// <summary>
		/// Gets the signed LOWORD value from the given <see langword="int"/>.
		/// </summary>
		/// <param name="n">The PARAM value.</param>
		/// <returns>The LOWORD.</returns>
		public static int SignedLOWORD(int n)
			=> (short)LOWORD(n);

		/// <summary>
		/// Gets a <see langword="nint"/> value from the given bool.
		/// </summary>
		/// <param name="value">if set to <c>true</c> [value].</param>
		/// <returns>1 if true, 0 otherwise.</returns>
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
