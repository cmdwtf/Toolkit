using System.Drawing.Imaging;

using SdiColorMatrix = System.Drawing.Imaging.ColorMatrix;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Functionality related to color matrices.
	/// </summary>
	/// <remarks>See also: https://docs.microsoft.com/en-us/dotnet/framework/winforms/advanced/how-to-use-a-color-matrix-to-transform-a-single-color</remarks>
	public static class ColorMatrix
	{
		/// <summary>
		/// Sets the color matrix in the given attributes to have a tint color matrix.
		/// </summary>
		/// <param name="attribs">The attributes to adjust.</param>
		/// <param name="tintColor">The color of the tint matrix.</param>
		public static void SetTintColorMatrix(this ImageAttributes attribs, System.Drawing.Color tintColor)
			=> attribs.SetColorMatrix(CreateTintColorMatrix(tintColor));


		/// <summary>
		/// Sets the color matrix in the given attributes to have a hue color matrix.
		/// </summary>
		/// <param name="attribs">The attributes to adjust.</param>
		/// <param name="hueColor">The color of the hue matrix.</param>
		/// <param name="intensity">The intensity of the hue matrix.</param>
		public static void SetHueColorMatrix(this ImageAttributes attribs, System.Drawing.Color hueColor, float intensity = 1.0f)
			=> attribs.SetColorMatrix(CreateHueColorMatrix(hueColor, intensity));

		/// <summary>
		/// Creates a hue color shift matrix.
		/// </summary>
		/// <param name="hueColor">The hue color to shift to.</param>
		/// <param name="intensity">How intense the shift should be, normalized and defaulted at 1.0f.</param>
		/// <returns>The hue matrix.</returns>
		public static SdiColorMatrix CreateHueColorMatrix(System.Drawing.Color hueColor, float intensity = 1.0f)
		{
			float r = hueColor.R / 255f * intensity;
			float g = hueColor.G / 255f * intensity;
			float b = hueColor.B / 255f * intensity;

			return new SdiColorMatrix(new float[][]
				{
					new float[] { 1, 0, 0, 0, 0 },
					new float[] { 0, 1, 0, 0, 0 },
					new float[] { 0, 0, 1, 0, 0 },
					new float[] { 0, 0, 0, 1, 0 },
					new float[] { r, g, b, 0, 1 }
				});
		}

		/// <summary>
		/// Creates a tint color shift matrix.
		/// </summary>
		/// <param name="tintBy">The color to tint by.</param>
		/// <returns>The tint matrix.</returns>
		public static SdiColorMatrix CreateTintColorMatrix(System.Drawing.Color tintBy)
		{
			float r = tintBy.R / 255f;
			float g = tintBy.G / 255f;
			float b = tintBy.B / 255f;
			float a = tintBy.A / 255f;

			return new SdiColorMatrix(new float[][]
				{
					new float[] { r, 0, 0, 0, 0 },
					new float[] { 0, g, 0, 0, 0 },
					new float[] { 0, 0, b, 0, 0 },
					new float[] { 0, 0, 0, a, 0 },
					new float[] { 0, 0, 0, 0, 1 },
				});
		}

		/// <summary>
		/// Conversion to grayscale is another common conversion. Grayscale
		/// values are determined by calculating the luminosity of a color,
		/// which is a weighted average of the R, G and B color components.
		/// The average is weighted according to the sensitivity of the human
		/// eye to each color component. The weights used here are as given
		/// by the NTSC (North America Television Standards Committee) and are widely accepted.
		/// </summary>
		/// <remarks>See also: https://www.codeproject.com/Articles/3772/ColorMatrix-Basics-Simple-Image-Color-Adjustment </remarks>
		public static readonly SdiColorMatrix GrayscaleColorMatrix =
			new(new float[][]
				{
					new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
					new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
					new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
					new float[] { 0, 0, 0, 1, 0 },
					new float[] { 0, 0, 0, 0, 1 },
				});
	}
}
