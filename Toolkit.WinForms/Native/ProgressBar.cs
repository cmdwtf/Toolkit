
using static cmdwtf.Toolkit.WinForms.Native.ComCtl32;
using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Native interop for <see cref="System.Windows.Forms.ProgressBar"/>.
	/// </summary>
	public static class ProgressBar
	{
		/// <summary>
		/// Progress bar messages -- CommCtrl.h
		/// </summary>
		public static class PBM
		{
			public const uint SETRANGE = WM_USER + 1;
			public const uint SETPOS = WM_USER + 2;
			public const uint DELTAPOS = WM_USER + 3;
			public const uint SETSTEP = WM_USER + 4;
			public const uint STEPIT = WM_USER + 5;
			public const uint SETRANGE32 = WM_USER + 6;
			public const uint GETRANGE = WM_USER + 7;
			public const uint GETPOS = WM_USER + 8;
			public const uint SETBARCOLOR = WM_USER + 9;
			public const uint SETBKCOLOR = CCM.SETBKCOLOR;
			public const uint SETMARQUEE = WM_USER + 10;
			public const uint GETSTEP = WM_USER + 13;
			public const uint GETBKCOLOR = WM_USER + 14;
			public const uint GETBARCOLOR = WM_USER + 15;
			public const uint SETSTATE = WM_USER + 16; // wParam = PBST.[state]
			public const uint GETSTATE = WM_USER + 17;
		}

		/// <summary>
		/// Progress bar styles -- CommCtrl.h
		/// </summary>
		public static class PBS
		{
			public const uint SMOOTH = 0x01;
			public const uint VERTICAL = 0x04;
			public const uint MARQUEE = 0x08;
			public const uint SMOOTHREVERSE = 0x10;
		}

		/// <summary>
		/// Progress bar states -- CommCtrl.h
		/// </summary>
		public static class PBST
		{
			public const uint NORMAL = 0x0001;
			public const uint ERROR = 0x0002;
			public const uint PAUSED = 0x0003;
		}
	}
}
