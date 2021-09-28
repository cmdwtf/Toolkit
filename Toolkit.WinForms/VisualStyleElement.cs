
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
		}
	}
}
