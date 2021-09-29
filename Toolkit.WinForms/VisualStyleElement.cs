
using LV = cmdwtf.Toolkit.WinForms.Native.ComCtl32.ListView;
using PB = cmdwtf.Toolkit.WinForms.Native.ComCtl32.ProgressBar;
using TV = cmdwtf.Toolkit.WinForms.Native.ComCtl32.TreeView;
using VSE = System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Some custom <see cref="VisualStyleElement"/>s.
	/// Classes modeled after <see cref="VSE"/>'s static instances.
	/// </summary>
	public static class VisualStyleElement
	{
		public static class ProgressBar
		{
			public static class Fill
			{
				/// <summary>
				/// A Progress Bar renderer for a 'Partial' fill.
				/// </summary>
				public static VSE Partial { get; } = VSE.CreateElement(
						PB.VSCLASS.PROGRESS,
						PB.PP.FILL,
						PB.PBFS.PARTIAL);

				/// <summary>
				/// A Progress Bar renderer for a 'Meter' fill.
				/// See <see href="https://docs.microsoft.com/en-us/windows/win32/uxguide/progress-bars?redirectedfrom=MSDN#meters">
				/// the progress bars guidelines</see>.
				/// </summary>
				/// <remarks>
				/// This is an alias for <see cref="Partial"/>
				/// </remarks>
				public static VSE Meter => Partial;
			}

			public static class TransparentBar
			{
				/// <summary>
				/// A Progress Bar renderer for a 'Partial' transparent bar.
				/// </summary>
				public static VSE Partial { get; } = VSE.CreateElement(
						PB.VSCLASS.PROGRESS,
						PB.PP.TRANSPARENTBAR,
						PB.PBBS.PARTIAL);

				/// <summary>
				/// A Progress Bar renderer for a 'Meter' transparent bar.
				/// See <see href="https://docs.microsoft.com/en-us/windows/win32/uxguide/progress-bars?redirectedfrom=MSDN#meters"/>
				/// the progress bars guidelines</see>.
				/// </summary>
				/// <remarks>
				/// This is an alias for <see cref="Partial"/>
				/// </remarks>
				public static VSE Meter => Partial;
			}
		}

		public static class ExplorerTreeView
		{
			public static class Glyph
			{
				public static class Normal
				{
					/// <summary>
					/// The explorer treeview opened expando glyph, no mouse over.
					/// </summary>
					public static VSE Closed { get; } = VSE.CreateElement(
							TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
							TV.TVP.GLYPH,
							TV.GLPS.CLOSED);

					/// <summary>
					/// The explorer treeview closed expando glyph, no mouse over.
					/// </summary>
					public static VSE Opened { get; } = VSE.CreateElement(
							TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
							TV.TVP.GLYPH,
							TV.GLPS.OPENED);
				}

				public static class Hot
				{
					/// <summary>
					/// The explorer treeview opened expando glyph, with mouse over.
					/// </summary>
					public static VSE Closed { get; } = VSE.CreateElement(
							TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
							TV.TVP.HOTGLYPH,
							TV.HGLPS.CLOSED);

					/// <summary>
					/// The explorer treeview closed expando glyph, with mouse over.
					/// </summary>
					public static VSE Opened { get; } = VSE.CreateElement(
							TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
							TV.TVP.HOTGLYPH,
							TV.HGLPS.OPENED);
				}
			}

			public static class TreeItem
			{
				/// <summary>
				/// A normal tree item.
				/// </summary>
				public static VSE Normal = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.NORMAL
					);


				/// <summary>
				/// A hot tree item.
				/// </summary>
				public static VSE Hot = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.HOT
					);


				/// <summary>
				/// A selected tree item.
				/// </summary>
				public static VSE Selected = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.SELECTED
					);


				/// <summary>
				/// A disabled tree item.
				/// </summary>
				public static VSE Disabled = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.DISABLED
					);

				/// <summary>
				/// A selected & unfocused tree item.
				/// </summary>
				public static VSE SelectedNotFocus = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.SELECTEDNOTFOCUS
					);

				/// <summary>
				/// A hot & selected tree item.
				/// </summary>
				public static VSE HotSelected = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.HOTSELECTED
					);
			}
		}

		public static class ExplorerListView
		{
			public static class ListItem
			{
				/// <summary>
				/// A normal list item.
				/// </summary>
				public static VSE Normal = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.NORMAL
					);

				/// <summary>
				/// A hot list item.
				/// </summary>
				public static VSE Hot = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.HOT
					);

				/// <summary>
				/// A selected list item.
				/// </summary>
				public static VSE Selected = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.SELECTED
					);

				/// <summary>
				/// A disabled list item.
				/// </summary>
				public static VSE Disabled = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.DISABLED
					);

				/// <summary>
				/// A selected & unfocused list item.
				/// </summary>
				public static VSE SelectedNotFocus = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.SELECTEDNOTFOCUS
					);

				/// <summary>
				/// A hot & selected list item.
				/// </summary>
				public static VSE HotSelected = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.HOTSELECTED
					);
			}
		}
	}
}
