using System;
using System.Drawing;

using static cmdwtf.Toolkit.WinForms.Native.Icon;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// A collection of tools related to interacting with the Windows cursor.
	/// </summary>
	/// <remarks>See also: https://stackoverflow.com/a/550965/944605 </remarks>
	public static class Cursor
	{
		/// <summary>
		/// Create a cursor from a bitmap without resizing and with the specified
		/// hot spot.
		/// </summary>
		/// <param name="cursorImage">The image to use as a cursor.</param>
		/// <param name="xHotSpot">The X hot spot, in pixels.</param>
		/// <param name="yHotSpot">The Y hot spot, in pixels.</param>
		/// <returns></returns>
		public static System.Windows.Forms.Cursor CreateCursorNoResize(Bitmap cursorImage, int xHotSpot, int yHotSpot)
		{
			IntPtr hIcon = cursorImage.GetHicon();
			var info = new IconInfo();

			GetIconInfo(hIcon, ref info);
			info.xHotspot = xHotSpot;
			info.yHotspot = yHotSpot;
			info.fIcon = false;

			hIcon = CreateIconIndirect(ref info);

			return new System.Windows.Forms.Cursor(hIcon);
		}


		/// <summary>
		/// Create a 32x32 cursor from a bitmap, with the hot spot in the middle.
		/// Will resize the bitmap to 32x32 before cursorizing it.
		/// </summary>
		/// <param name="cursorImage">The image to use as the cursor.</param>
		/// <returns>The created cursor.</returns>
		public static System.Windows.Forms.Cursor CreateCursor(Bitmap cursorImage)
		{
			const int SizeSquare = 32;
			const int HotSpotSquare = SizeSquare / 2;

			IntPtr ptr = cursorImage.Resize(SizeSquare, SizeSquare).GetHicon();
			var tmp = new IconInfo();
			GetIconInfo(ptr, ref tmp);
			tmp.xHotspot = HotSpotSquare;
			tmp.yHotspot = HotSpotSquare;
			tmp.fIcon = false;
			ptr = CreateIconIndirect(ref tmp);
			return new System.Windows.Forms.Cursor(ptr);
		}
	}
}
