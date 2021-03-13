
using SDColor = System.Drawing.Color;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Tools related to <see cref="System.Drawing.Color"/>
	/// </summary>
	public static class Color
	{
		/// <summary>
		/// A color based linear interpolation.
		/// </summary>
		/// <param name="color">The color to lerp from.</param>
		/// <param name="to">The color to lerp to.</param>
		/// <param name="amount">The amount of distance to lerp, as a value from 0.0f-1.0f. This value is unclamped.</param>
		/// <returns>The interpolated color.</returns>
		public static SDColor Lerp(this SDColor color, SDColor to, float amount)
		{
			// start colors as lerp-able floats
			float sr = color.R, sg = color.G, sb = color.B, sa = color.A;

			// end colors as lerp-able floats
			float er = to.R, eg = to.G, eb = to.B, ea = to.A;

			// lerp the colors to get the difference
			byte r = (byte)sr.Lerp(er, amount),
				 g = (byte)sg.Lerp(eg, amount),
				 b = (byte)sb.Lerp(eb, amount),
				 a = (byte)sa.Lerp(ea, amount);

			// return the new color
			return SDColor.FromArgb(a, r, g, b);
		}

		/// <summary>
		/// Creates a color from one color, but replacing the components by the given optional parameters.
		/// </summary>
		/// <param name="c">The base color.</param>
		/// <param name="a">The new alpha component. (Optional)</param>
		/// <param name="r">The new red component. (Optional)</param>
		/// <param name="g">The new green component. (Optional)</param>
		/// <param name="b">The new blue component. (Optional)</param>
		/// <returns>The new color.</returns>
		public static SDColor With(this SDColor c, int? a = null, int? r = null, int? g = null, int? b = null)
			=> SDColor.FromArgb(a ?? c.A, r ?? c.R, g ?? c.G, b ?? c.B);

	}
}
