using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.Native
{
	/// <summary>
	/// Native methods, constants, and structs for Multimedia operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal static class Multimedia
	{

		public delegate void TimerCallbackDelegate(uint id, uint msg, ref uint userCtx, uint rsv1, uint rsv2);
		[DllImport(Libraries.Multimedia, SetLastError = true, EntryPoint = "timeSetEvent")]
		public static extern uint TimeSetEvent(uint msDelay, uint msResolution, TimerCallbackDelegate callback, ref uint userCtx, uint eventType);

		[DllImport(Libraries.Multimedia, SetLastError = true, EntryPoint = "timeKillEvent")]
		public static extern uint TimeKillEvent(uint uTimerId);
	}
}
