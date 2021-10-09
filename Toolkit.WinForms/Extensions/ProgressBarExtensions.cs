
using System;
using System.Windows.Forms;

using cmdwtf.Toolkit.WinForms.Controls;

using static cmdwtf.Toolkit.WinForms.Native.ComCtl32.ProgressBar;

namespace cmdwtf.Toolkit.WinForms.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="ProgressBar"/>
	/// </summary>
	public static partial class ProgressBarExtensions

	{
		/// <summary>
		/// Gets the current state of the progress bar.
		/// </summary>
		/// <param name="progressBar">The progress bar to get the state of.</param>
		/// <returns>The current progress bar state.</returns>
		public static ProgressBarState GetStateNative(this ProgressBar progressBar)
		{
			if (progressBar.IsHandleCreated == false)
			{
				return ProgressBarState.Unknown;
			}

			int state = Native.User32.SendMessage(progressBar.Handle, PBM.GETSTATE, UIntPtr.Zero, IntPtr.Zero);
			return (ProgressBarState)state;
		}

		/// <summary>
		/// Sets the progress bar's state.
		/// </summary>
		/// <param name="progressBar">The progress bar to set the state of.</param>
		/// <param name="state">The desired state.</param>
		/// <returns>The result of the native SendMessage call.</returns>
		public static int SetStateNative(this ProgressBar progressBar, ProgressBarState state)
		{
			if (progressBar.IsHandleCreated == false)
			{
				return -1;
			}

			if (state == ProgressBarState.Unknown ||
				Enum.IsDefined(typeof(ProgressBarState), state) == false)
			{
				throw new ArgumentException($"{nameof(state)} must be a known {nameof(ProgressBarState)} value.", nameof(state));
			}

			UIntPtr wParam = new((uint)state);
			return Native.User32.SendMessage(progressBar.Handle, PBM.SETSTATE, wParam, IntPtr.Zero);
		}

		/// <summary>
		/// Gets the progress bar's current orientation state.
		/// </summary>
		/// <param name="progressBar">The progress bar to get the orientation of.</param>
		/// <returns>The orientation of the progress bar.</returns>
		public static Orientation GetOrientationNative(this ProgressBar progressBar)
			=> progressBar.GetNativeStyleFlagSet(PBS.VERTICAL)
				? Orientation.Vertical
				: Orientation.Horizontal;

		/// <summary>
		/// Sets the progress bar's orientation.
		/// </summary>
		/// <param name="progressBar">The progress bar to adjust.</param>
		/// <param name="orientation">The orientation to apply.</param>
		/// <returns>The result of the set window long call, the previous window style.</returns>
		public static int SetOrientationNative(this ProgressBar progressBar, Orientation orientation)
			=> progressBar.SetNativeStyleFlag(PBS.VERTICAL, orientation == Orientation.Vertical);

		/// <summary>
		/// Gets a value indicating if the native style flag for smooth is set.
		/// </summary>
		/// <param name="progressBar">The window to get the style of.</param>
		/// <returns><c>true</c>, if the style is set, otherwise <c>false</c>.</returns>
		public static bool GetSmoothNative(this ProgressBar progressBar)
			=> progressBar.GetNativeStyleFlagSet(PBS.SMOOTH);

		/// <summary>
		/// Sets the native style for smooth to be enabled or disabled.
		/// </summary>
		/// <param name="progressBar">The window to set the style on.</param>
		/// <param name="smooth">The state of the style to set.</param>
		/// <returns>The previous window style value.</returns>
		public static int SetSmoothNative(this ProgressBar progressBar, bool smooth)
			=> progressBar.SetNativeStyleFlag(PBS.SMOOTH, smooth);

		/// <summary>
		/// Gets a value indicating if the native style flag for smooth reverse is set.
		/// </summary>
		/// <param name="progressBar">The window to get the style of.</param>
		/// <returns><c>true</c>, if the style is set, otherwise <c>false</c>.</returns>
		public static bool GetSmoothReverseNative(this ProgressBar progressBar)
			=> progressBar.GetNativeStyleFlagSet(PBS.SMOOTHREVERSE);

		/// <summary>
		/// Sets the native style for smooth reverse to be enabled or disabled.
		/// </summary>
		/// <param name="progressBar">The window to set the style on.</param>
		/// <param name="smooth">The state of the style to set.</param>
		/// <returns>The previous window style value.</returns>
		public static int SetSmoothReverseNative(this ProgressBar progressBar, bool smooth)
			=> progressBar.SetNativeStyleFlag(PBS.SMOOTHREVERSE, smooth);
	}
}
