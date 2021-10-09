using System.Drawing;

namespace cmdwtf.Toolkit.WinForms.Extensions
{
	/// <summary>
	/// Tools related to <see cref="Color"/>
	/// </summary>
	public static class ColorExtensions

	{
		/// <summary>
		/// A color based linear interpolation.
		/// </summary>
		/// <param name="color">The color to lerp from.</param>
		/// <param name="to">The color to lerp to.</param>
		/// <param name="amount">The amount of distance to lerp, as a value from 0.0f-1.0f. This value is unclamped.</param>
		/// <returns>The interpolated color.</returns>
		public static Color Lerp(this Color color, Color to, float amount)
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
			return Color.FromArgb(a, r, g, b);
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
		public static Color With(this Color c, int? a = null, int? r = null, int? g = null, int? b = null)
			=> Color.FromArgb(a ?? c.A, r ?? c.R, g ?? c.G, b ?? c.B);

	}
}
