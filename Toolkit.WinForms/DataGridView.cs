using System.Drawing;
using System.Windows.Forms;

using SwfDataGridView = System.Windows.Forms.DataGridView;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Extension methods for dealing with <see cref="System.Windows.Forms.DataGridView"/>
	/// </summary>
	static class DataGridView
	{
		/// <summary>
		/// Converts the requested column into a checkbox column.
		/// </summary>
		/// <param name="dataGridView">The DataGridView to operate on.</param>
		/// <param name="column">The zero-based column index to convert to a checkbox column.</param>
		/// <returns>The checkbox control created for the column.</returns>
		public static CheckBox ColumnCheckbox(this SwfDataGridView dataGridView, int column, Size? size = null)
		{
			var checkBox = new CheckBox
			{
				Size = size ?? new Size(15, 15),
				BackColor = System.Drawing.Color.Transparent,

				// Reset properties
				Padding = new Padding(0),
				Margin = new Padding(0),
				Text = ""
			};

			// Add checkbox to datagrid cell
			dataGridView.Controls.Add(checkBox);
			DataGridViewHeaderCell header = dataGridView.Columns[column].HeaderCell;
			checkBox.Location = new Point(
				header.ContentBounds.Left +
				 ((header.ContentBounds.Right - header.ContentBounds.Left + checkBox.Size.Width)
				 / 2) + 3,
				header.ContentBounds.Top +
				 ((header.ContentBounds.Bottom - header.ContentBounds.Top + checkBox.Size.Height)
				 / 2) - 2);
			return checkBox;
		}
	}
}
