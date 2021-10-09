using System.Collections.Generic;
using System.Windows.Forms;

namespace cmdwtf.Toolkit.WinForms.Extensions
{
	/// <summary>
	/// Extensions related to toolstrips and other similar controls.
	/// </summary>
	public static class ToolStripExtensions
	{
		/// <summary>
		/// Gets the context menu items from a given tool strip item collection.
		/// Will cascade through drop downs.
		/// </summary>
		/// <param name="items">The item collection to get the menu items from.</param>
		/// <returns>A list of the menu items.</returns>
		public static List<ToolStripMenuItem> GetMenuItems(this ToolStripItemCollection items)
		{
			List<ToolStripMenuItem> result = new();
			foreach (ToolStripItem item in items)
			{
				if (item is ToolStripMenuItem menuItem)
				{
					result.Add(menuItem);
					if (menuItem.HasDropDownItems)
					{
						result.AddRange(GetMenuItems(menuItem.DropDownItems));
					}
				}
			}
			return result;
		}
	}
}
