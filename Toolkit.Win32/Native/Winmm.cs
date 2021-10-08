using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit.Native
{
	/// <summary>
	/// Native methods, constants, and structs for Multimedia operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal static class Winmm
	{
		public const string NativeLibrary = "winmm.dll";
		public enum MMRESULT : uint
		{
			MMSYSERR_NOERROR = 0,
			MMSYSERR_ERROR = 1,
			MMSYSERR_BADDEVICEID = 2,
			MMSYSERR_NOTENABLED = 3,
			MMSYSERR_ALLOCATED = 4,
			MMSYSERR_INVALHANDLE = 5,
			MMSYSERR_NODRIVER = 6,
			MMSYSERR_NOMEM = 7,
			MMSYSERR_NOTSUPPORTED = 8,
			MMSYSERR_BADERRNUM = 9,
			MMSYSERR_INVALFLAG = 10,
			MMSYSERR_INVALPARAM = 11,
			MMSYSERR_HANDLEBUSY = 12,
			MMSYSERR_INVALIDALIAS = 13,
			MMSYSERR_BADDB = 14,
			MMSYSERR_KEYNOTFOUND = 15,
			MMSYSERR_READERROR = 16,
			MMSYSERR_WRITEERROR = 17,
			MMSYSERR_DELETEERROR = 18,
			MMSYSERR_VALNOTFOUND = 19,
			MMSYSERR_NODRIVERCB = 20,
			WAVERR_BADFORMAT = 32,
			WAVERR_STILLPLAYING = 33,
			WAVERR_UNPREPARED = 34
		}

		[StructLayout(LayoutKind.Sequential)]
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
		[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Win32 Naming")]
		public struct TIMECAPS
		{
			internal int wPeriodMin;

			internal int wPeriodMax;
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void TimerCallbackDelegate(uint id, uint msg, ref uint userCtx, uint rsv1, uint rsv2);

		[DllImport(NativeLibrary, EntryPoint = "timeKillEvent", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern MMRESULT TimeKillEvent(uint uTimerId);

		[DllImport(NativeLibrary, EntryPoint = "timeSetEvent", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern MMRESULT TimeSetEvent(uint msDelay, uint msResolution, TimerCallbackDelegate callback, ref uint userCtx, uint eventType);

		[DllImport(NativeLibrary, EntryPoint = "timeGetDevCaps", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int TimeGetDevCaps(ref TIMECAPS ptc, int cbtc);

		[DllImport(NativeLibrary, EntryPoint = "timeBeginPeriod", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int TimeBeginPeriod(int uPeriod);

		[DllImport(NativeLibrary, EntryPoint = "timeEndPeriod", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int TimeEndPeriod(int uPeriod);
	}
}
