
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
							TV.GLPS.OPENED);

					/// <summary>
					/// The explorer treeview closed expando glyph, no mouse over.
					/// </summary>
					public static VSE Opened { get; } = VSE.CreateElement(
							TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
							TV.TVP.GLYPH,
							TV.GLPS.CLOSED);
				}

				public static class Hover
				{
					/// <summary>
					/// The explorer treeview opened expando glyph, no with mouse over.
					/// </summary>
					public static VSE Closed { get; } = VSE.CreateElement(
							TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
							TV.TVP.GLYPH,
							TV.HGLPS.CLOSED);

					/// <summary>
					/// The explorer treeview closed expando glyph, no with mouse over.
					/// </summary>
					public static VSE Opened { get; } = VSE.CreateElement(
							TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
							TV.TVP.GLYPH,
							TV.HGLPS.CLOSED);
				}
			}

			public static class TreeItem
			{
				public VSE Normal = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.NORMAL
					);

				public VSE Hot = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.HOT
					);

				public VSE Selected = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.SELECTED
					);

				public VSE Disabled = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.DISABLED
					);

				public VSE SelectedNotFocus = VSE.CreateElement(
						TV.VSCLASS.Undocumented.Explorer.ExplorerTreeView,
						TV.TVP.TREEITEM,
						TV.TREIS.SELECTEDNOTFOCUS
					);

				public VSE DoubleSelected = VSE.CreateElement(
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
				public VSE Normal = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.NORMAL
					);

				public VSE Hot = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.HOT
					);

				public VSE Selected = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.SELECTED
					);

				public VSE Disabled = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.DISABLED
					);

				public VSE SelectedNotFocus = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.SELECTEDNOTFOCUS
					);

				public VSE HotSelected = VSE.CreateElement(
						LV.VSCLASS.Undocumented.Explorer.ExplorerListView,
						LV.LVP.LISTITEM,
						LV.LISS.HOTSELECTED
					);
			}
		}
	}
}
