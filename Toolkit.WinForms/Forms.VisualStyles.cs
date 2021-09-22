using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace cmdwtf.Toolkit.WinForms
{
	public static partial class Forms
	{
		/// <summary>
		/// Converts a <see cref="CheckState"/> to a corrosponding
		/// <see cref="CheckBoxState"/>, of the 'Normal' variety.
		/// </summary>
		/// <param name="state">The <see cref="CheckState"/> to convert.</param>
		/// <returns>A <see cref="CheckBoxState"/> based on the given value.</returns>
		public static CheckBoxState ToVisualStyleNormal(this CheckState state)
			=> state.ToVisualStyle(false, false, false);

		/// <summary>
		/// Converts a <see cref="CheckState"/> to a corrosponding
		/// <see cref="CheckBoxState"/>, of the 'Hot' variety.
		/// </summary>
		/// <param name="state">The <see cref="CheckState"/> to convert.</param>
		/// <returns>A <see cref="CheckBoxState"/> based on the given value.</returns>
		public static CheckBoxState ToVisualStyleHot(this CheckState state)
			=> state.ToVisualStyle(true, false, false);

		/// <summary>
		/// Converts a <see cref="CheckState"/> to a corrosponding
		/// <see cref="CheckBoxState"/>, of the 'Pressed' variety.
		/// </summary>
		/// <param name="state">The <see cref="CheckState"/> to convert.</param>
		/// <returns>A <see cref="CheckBoxState"/> based on the given value.</returns>
		public static CheckBoxState ToVisualStylePressed(this CheckState state)
			=> state.ToVisualStyle(false, true, false);

		/// <summary>
		/// Converts a <see cref="CheckState"/> to a corrosponding
		/// <see cref="CheckBoxState"/>, of the 'Disabled' variety.
		/// </summary>
		/// <param name="state">The <see cref="CheckState"/> to convert.</param>
		/// <returns>A <see cref="CheckBoxState"/> based on the given value.</returns>
		public static CheckBoxState ToVisualStyleDisabled(this CheckState state)
			=> state.ToVisualStyle(false, false, true);

		/// <summary>
		/// Converts a <see cref="CheckState"/> to a corrosponding
		/// <see cref="CheckBoxState"/> based on the state booleans.
		/// If all the state booleans are false, will return the 'Normal' variant.
		/// </summary>
		/// <param name="state">The <see cref="CheckState"/> to convert.</param>
		/// <param name="hot">true, if the result should be 'Hot'.</param>
		/// <param name="pressed">true, if the result shold be 'Pressed'.</param>
		/// <param name="disabled">true, if the result should be 'Disabled'.</param>
		/// <returns>A <see cref="CheckBoxState"/> based on the given values.</returns>
		public static CheckBoxState ToVisualStyle(this CheckState state, bool hot, bool pressed, bool disabled)
		{
			return state switch
			{
				CheckState.Unchecked when hot => CheckBoxState.UncheckedHot,
				CheckState.Unchecked when pressed => CheckBoxState.UncheckedPressed,
				CheckState.Unchecked when disabled => CheckBoxState.UncheckedDisabled,
				CheckState.Unchecked => CheckBoxState.UncheckedNormal,
				CheckState.Checked when hot => CheckBoxState.CheckedHot,
				CheckState.Checked when pressed => CheckBoxState.CheckedPressed,
				CheckState.Checked when disabled => CheckBoxState.CheckedDisabled,
				CheckState.Checked => CheckBoxState.CheckedNormal,
				CheckState.Indeterminate when hot => CheckBoxState.MixedHot,
				CheckState.Indeterminate when pressed => CheckBoxState.MixedPressed,
				CheckState.Indeterminate when disabled => CheckBoxState.MixedDisabled,
				CheckState.Indeterminate => CheckBoxState.MixedNormal,
				_ => throw new System.ArgumentOutOfRangeException(nameof(state)),
			};
		}
	}
}

