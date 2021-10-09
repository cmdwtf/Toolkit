using static cmdwtf.Toolkit.WinForms.Native.Windows;

namespace cmdwtf.Toolkit.WinForms.Native
{
	/// <summary>
	/// Native methods, constants, and structs for CommCtl32 operations.
	/// See Win32 documentation for more detail.
	/// </summary>
	/// <remarks>Some information reproduced here from https://pinvoke.net/ </remarks>
	internal static partial class ComCtl32
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
				public const int SETRANGE = WM_USER + 1;
				public const int SETPOS = WM_USER + 2;
				public const int DELTAPOS = WM_USER + 3;
				public const int SETSTEP = WM_USER + 4;
				public const int STEPIT = WM_USER + 5;
				public const int SETRANGE32 = WM_USER + 6;
				public const int GETRANGE = WM_USER + 7;
				public const int GETPOS = WM_USER + 8;
				public const int SETBARCOLOR = WM_USER + 9;
				public const int SETBKCOLOR = CCM.SETBKCOLOR;
				public const int SETMARQUEE = WM_USER + 10;
				public const int GETSTEP = WM_USER + 13;
				public const int GETBKCOLOR = WM_USER + 14;
				public const int GETBARCOLOR = WM_USER + 15;
				public const int SETSTATE = WM_USER + 16; // wParam = PBST.[state]
				public const int GETSTATE = WM_USER + 17;
			}

			/// <summary>
			/// Progress bar styles -- CommCtrl.h
			/// </summary>
			public static class PBS
			{
				public const int SMOOTH = 0x01;
				public const int VERTICAL = 0x04;
				public const int MARQUEE = 0x08;
				public const int SMOOTHREVERSE = 0x10;
			}

			/// <summary>
			/// Progress bar states -- CommCtrl.h
			/// </summary>
			public static class PBST
			{
				public const int NORMAL = 0x0001;
				public const int ERROR = 0x0002;
				public const int PAUSED = 0x0003;
			}

			#region Visual Styles

			/// <summary>
			/// Class names -- vsstyle.h
			/// </summary>
			public static class VSCLASS
			{
				public const string PROGRESSSTYLE = "PROGRESSSTYLE";
				public const string PROGRESS = "PROGRESS";
			}

			/// <summary>
			/// Progress bar parts -- vsstyle.h as "PROGRESSPARTS"
			/// </summary>
			public static class PP
			{
				public const int BAR = 1;
				public const int BARVERT = 2;
				public const int CHUNK = 3;
				public const int CHUNKVERT = 4;
				public const int FILL = 5;
				public const int FILLVERT = 6;
				public const int PULSEOVERLAY = 7;
				public const int MOVEOVERLAY = 8;
				public const int PULSEOVERLAYVERT = 9;
				public const int MOVEOVERLAYVERT = 10;
				public const int TRANSPARENTBAR = 11;
				public const int TRANSPARENTBARVERT = 12;
			}

			/// <summary>
			/// Progress bar transparent bar states -- vsstyle.h as "TRANSPARENTBARSTATES"
			/// </summary>
			public static class PBBS
			{
				public const int NORMAL = 1;
				public const int PARTIAL = 2;
			}

			/// <summary>
			/// Progress bar transparent bar vertical states -- vsstyle.h as "TRANSPARENTBARVERTSTATES"
			/// </summary>
			public static class PBBVS
			{
				public const int NORMAL = 1;
				public const int PARTIAL = 2;
			}

			/// <summary>
			/// Progress bar fill states -- vsstyle.h as "FILLSTATES"
			/// </summary>
			public static class PBFS
			{
				public const int NORMAL = 0x0001;
				public const int ERROR = 0x0002;
				public const int PAUSED = 0x0003;
				public const int PARTIAL = 0x0004;
			}

			/// <summary>
			/// Progress bar vert fill states -- vsstyle.h as "FILLVERTSTATES"
			/// </summary>
			public static class PBVFS
			{
				public const int NORMAL = 0x0001;
				public const int ERROR = 0x0002;
				public const int PAUSED = 0x0003;
				public const int PARTIAL = 0x0004;
			}

			#endregion Visual Styles
		}
	}
}
