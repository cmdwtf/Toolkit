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
		/// Tree-View window styles -- CommCtrl.h
		/// </summary>
		public static class ListView
		{

			/// <summary>
			/// Generic Tree-View Constants.
			/// </summary>
			public static class TV
			{
				public const uint FIRST = 0x1100;
			}

			/// <summary>
			/// Tree-View styles -- CommCtrl.h
			/// </summary>
			public static class TVS
			{
				public const uint HASBUTTONS = 0x0001;
				public const uint HASLINES = 0x0002;
				public const uint LINESATROOT = 0x0004;
				public const uint EDITLABELS = 0x0008;
				public const uint DISABLEDRAGDROP = 0x0010;
				public const uint SHOWSELALWAYS = 0x0020;
				public const uint RTLREADING = 0x0040;
				public const uint NOTOOLTIPS = 0x0080;
				public const uint CHECKBOXES = 0x0100;
				public const uint TRACKSELECT = 0x0200;
				public const uint SINGLEEXPAND = 0x0400;
				public const uint INFOTIP = 0x0800;
				public const uint FULLROWSELECT = 0x1000;
				public const uint NOSCROLL = 0x2000;
				public const uint NONEVENHEIGHT = 0x4000;
				public const uint NOHSCROLL = 0x8000;  // NOSCROLL overrides this

				/// <summary>
				/// Tree-View extended styles -- CommCtrl.h
				/// </summary>
				public static class EX
				{
					public const uint NOSINGLECOLLAPSE = 0x0001;
					public const uint MULTISELECT = 0x0002;
					public const uint DOUBLEBUFFER = 0x0004;
					public const uint NOINDENTSTATE = 0x0008;
					public const uint RICHTOOLTIP = 0x0010;
					public const uint AUTOHSCROLL = 0x0020;
					public const uint FADEINOUTEXPANDOS = 0x0040;
					public const uint PARTIALCHECKBOXES = 0x0080;
					public const uint EXCLUSIONCHECKBOXES = 0x0100;
					public const uint DIMMEDCHECKBOXES = 0x0200;
					public const uint DRAWIMAGEASYNC = 0x0400;
				}
			}

			#region Visual Styles

			/// <summary>
			/// Class names -- vsstyle.h
			/// </summary>
			public static class VSCLASS
			{
				public const string LISTVIEWSTYLE = "LISTVIEWSTYLE";
				public const string LISTVIEW = "LISTVIEW";

				public static class Undocumented
				{
					/// <remarks>
					/// Thanks to <see href="https://stackoverflow.com/a/3026207">this StackOverflow user</see>
					/// for tracking down this class name.
					/// </remarks>
					public static class Explorer
					{
						public const string ExplorerListView = "Explorer::ListView";
					}
				}
			}

			/// <summary>
			/// List View parts -- vsstyle.h as "LISTVIEWPARTS"
			/// </summary>
			public static class LVP
			{
				public const int LISTITEM = 1;
				public const int LISTGROUP = 2;
				public const int LISTDETAIL = 3;
				public const int LISTSORTEDDETAIL = 4;
				public const int EMPTYTEXT = 5;
				public const int GROUPHEADER = 6;
				public const int GROUPHEADERLINE = 7;
				public const int EXPANDBUTTON = 8;
				public const int COLLAPSEBUTTON = 9;
				public const int COLUMNDETAIL = 10;
			}

			/// <summary>
			/// Tree View item states -- vsstyle.h as "LISTITEMSTATES"
			/// </summary>
			public static class LISS
			{
				public const int NORMAL = 1;
				public const int HOT = 2;
				public const int SELECTED = 3;
				public const int DISABLED = 4;
				public const int SELECTEDNOTFOCUS = 5;
				public const int HOTSELECTED = 6;
			}


			/// <summary>
			/// List View header states -- vsstyle.h as "GROUPHEADERSTATES"
			/// </summary>
			public static class LVGH
			{
				public const int OPEN = 1;
				public const int OPENHOT = 2;
				public const int OPENSELECTED = 3;
				public const int OPENSELECTEDHOT = 4;
				public const int OPENSELECTEDNOTFOCUSED = 5;
				public const int OPENSELECTEDNOTFOCUSEDHOT = 6;
				public const int OPENMIXEDSELECTION = 7;
				public const int OPENMIXEDSELECTIONHOT = 8;
				public const int CLOSE = 9;
				public const int CLOSEHOT = 10;
				public const int CLOSESELECTED = 11;
				public const int CLOSESELECTEDHOT = 12;
				public const int CLOSESELECTEDNOTFOCUSED = 13;
				public const int CLOSESELECTEDNOTFOCUSEDHOT = 14;
				public const int CLOSEMIXEDSELECTION = 15;
				public const int CLOSEMIXEDSELECTIONHOT = 16;
			}

			/// <summary>
			/// List View header line states -- vsstyle.h as "GROUPHEADERLINESTATES"
			/// </summary>
			public static class LVGHL
			{
				public const int OPEN = 1;
				public const int OPENHOT = 2;
				public const int OPENSELECTED = 3;
				public const int OPENSELECTEDHOT = 4;
				public const int OPENSELECTEDNOTFOCUSED = 5;
				public const int OPENSELECTEDNOTFOCUSEDHOT = 6;
				public const int OPENMIXEDSELECTION = 7;
				public const int OPENMIXEDSELECTIONHOT = 8;
				public const int CLOSE = 9;
				public const int CLOSEHOT = 10;
				public const int CLOSESELECTED = 11;
				public const int CLOSESELECTEDHOT = 12;
				public const int CLOSESELECTEDNOTFOCUSED = 13;
				public const int CLOSESELECTEDNOTFOCUSEDHOT = 14;
				public const int CLOSEMIXEDSELECTION = 15;
				public const int CLOSEMIXEDSELECTIONHOT = 16;
			}

			/// <summary>
			/// List View expand glyph states -- vsstyle.h as "EXPANDBUTTONSTATES"
			/// </summary>
			public static class LVEB
			{
				public const int NORMAL = 1;
				public const int HOVER = 2;
				public const int PUSHED = 2;
			}

			/// <summary>
			/// List View collapse glyph states -- vsstyle.h as "COLLAPSEBUTTONSTATES"
			/// </summary>
			public static class LVCB
			{
				public const int NORMAL = 1;
				public const int HOVER = 2;
				public const int PUSHED = 2;
			}

			#endregion Visual Styles
		}
	}
}
