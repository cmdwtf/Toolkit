using System.Windows.Forms;

namespace cmdwtf.Toolkit.WinForms.DoubleBuffered
{
	/// <summary>
	/// Helpful constants for double buffered forms.
	/// </summary>
	internal static class Constants
	{
		public static ControlStyles UserPaintStyles = ControlStyles.UserPaint |
			ControlStyles.AllPaintingInWmPaint |
			ControlStyles.OptimizedDoubleBuffer;
	}
}
