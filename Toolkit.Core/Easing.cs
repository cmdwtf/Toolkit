//Easing
//Animates the value of a double property between two target values using
//Robert Penner's easing equations for interpolation over a specified Duration.
//
//Original Author:  Darren David darren-code@lookorfeel.com
//
//Ported to be easily used in Unity by Marco Mastropaolo
//
//Credit/Thanks:
//Robert Penner - The easing equations we all know and love
//  (http://robertpenner.com/easing/) [See License.txt for license info]
//
//Lee Brimelow - initial port of Penner's equations to WPF
//  (http://thewpfblog.com/?p=12)
//
//Zeh Fernando - additional equations (out/in) from
//  caurina.transitions.Tweener (http://code.google.com/p/tweener/)
//  [See License.txt for license info]


using static System.Math;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// Easing operations based on Robert Penner's easing functions.
	/// For visual examples of most functions, see <see href="https://easings.net"/>.
	/// </summary>
	public static class Easing
	{
		#region Helper

		/// <summary>
		/// The types of easing functions here.
		/// For visual examples of most, see https://easings.net/
		/// </summary>
		public enum Function
		{

			/// <summary>
			/// Easing equation function for a simple linear interpolation, with no easing.
			/// </summary>
			Linear,
			/// <summary>
			/// Easing equation function for an exponential (2^t) easing out:
			/// decelerating from zero velocity.
			/// </summary>
			ExpoEaseOut,
			/// <summary>
			/// Easing equation function for an exponential (2^t) easing in:
			/// accelerating from zero velocity.
			/// </summary>
			ExpoEaseIn,
			/// <summary>
			/// Easing equation function for an exponential (2^t) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			ExpoEaseInOut,
			/// <summary>
			/// Easing equation function for an exponential (2^t) easing out/in:
			/// deceleration until halfway, then acceleration.
			/// </summary>
			ExpoEaseOutIn,
			/// <summary>
			/// Easing equation function for a circular (sqrt(1-t^2)) easing out:
			/// decelerating from zero velocity.
			/// </summary>
			CircEaseOut,
			/// <summary>
			/// Easing equation function for a circular (sqrt(1-t^2)) easing in:
			/// accelerating from zero velocity.
			/// </summary>
			CircEaseIn,
			/// <summary>
			/// Easing equation function for a circular (sqrt(1-t^2)) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			CircEaseInOut,
			/// <summary>
			/// Easing equation function for a circular (sqrt(1-t^2)) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			CircEaseOutIn,
			/// <summary>
			/// Easing equation function for a quadratic (t^2) easing out:
			/// decelerating from zero velocity.
			/// </summary>
			QuadEaseOut,
			/// <summary>
			/// Easing equation function for a quadratic (t^2) easing in:
			/// accelerating from zero velocity.
			/// </summary>
			QuadEaseIn,
			/// <summary>
			/// Easing equation function for a quadratic (t^2) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			QuadEaseInOut,
			/// <summary>
			/// Easing equation function for a quadratic (t^2) easing out/in:
			/// deceleration until halfway, then acceleration.
			/// </summary>
			QuadEaseOutIn,
			/// <summary>
			/// Easing equation function for a sinusoidal (sin(t)) easing out:
			/// decelerating from zero velocity.
			/// </summary>
			SineEaseOut,
			/// <summary>
			/// Easing equation function for a sinusoidal (sin(t)) easing in:
			/// accelerating from zero velocity.
			/// </summary>
			SineEaseIn,
			/// <summary>
			/// Easing equation function for a sinusoidal (sin(t)) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			SineEaseInOut,
			/// <summary>
			/// Easing equation function for a sinusoidal (sin(t)) easing in/out:
			/// deceleration until halfway, then acceleration.
			/// </summary>
			SineEaseOutIn,
			/// <summary>
			/// Easing equation function for a cubic (t^3) easing out:
			/// decelerating from zero velocity.
			/// </summary>
			CubicEaseOut,
			/// <summary>
			/// Easing equation function for a cubic (t^3) easing in:
			/// accelerating from zero velocity.
			/// </summary>
			CubicEaseIn,
			/// <summary>
			/// Easing equation function for a cubic (t^3) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			CubicEaseInOut,
			/// <summary>
			/// Easing equation function for a cubic (t^3) easing out/in:
			/// deceleration until halfway, then acceleration.
			/// </summary>
			CubicEaseOutIn,
			/// <summary>
			/// Easing equation function for a quartic (t^4) easing out:
			/// decelerating from zero velocity.
			/// </summary>
			QuartEaseOut,
			/// <summary>
			/// Easing equation function for a quartic (t^4) easing in:
			/// accelerating from zero velocity.
			/// </summary>
			QuartEaseIn,
			/// <summary>
			/// Easing equation function for a quartic (t^4) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			QuartEaseInOut,
			/// <summary>
			/// Easing equation function for a quartic (t^4) easing out/in:
			/// deceleration until halfway, then acceleration.
			/// </summary>
			QuartEaseOutIn,
			/// <summary>
			/// Easing equation function for a quintic (t^5) easing out:
			/// decelerating from zero velocity.
			/// </summary>
			QuintEaseOut,
			/// <summary>
			/// Easing equation function for a quintic (t^5) easing in:
			/// accelerating from zero velocity.
			/// </summary>
			QuintEaseIn,
			/// <summary>
			/// Easing equation function for a quintic (t^5) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			QuintEaseInOut,
			/// <summary>
			/// Easing equation function for a quintic (t^5) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			QuintEaseOutIn,
			/// <summary>
			/// Easing equation function for an elastic (exponentially decaying sine wave) easing out:
			/// decelerating from zero velocity.
			/// </summary>
			ElasticEaseOut,
			/// <summary>
			/// Easing equation function for an elastic (exponentially decaying sine wave) easing in:
			/// accelerating from zero velocity.
			/// </summary>
			ElasticEaseIn,
			/// <summary>
			/// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			ElasticEaseInOut,
			/// <summary>
			/// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in:
			/// deceleration until halfway, then acceleration.
			/// </summary>
			ElasticEaseOutIn,
			/// <summary>
			/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out:
			/// decelerating from zero velocity.
			/// </summary>
			BounceEaseOut,
			/// <summary>
			/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in:
			/// accelerating from zero velocity.
			/// </summary>
			BounceEaseIn,
			/// <summary>
			/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			BounceEaseInOut,
			/// <summary>
			/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in:
			/// deceleration until halfway, then acceleration.
			/// </summary>
			BounceEaseOutIn,
			/// <summary>
			/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out:
			/// decelerating from zero velocity.
			/// </summary>
			BackEaseOut,
			/// <summary>
			/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in:
			/// accelerating from zero velocity.
			/// </summary>
			BackEaseIn,
			/// <summary>
			/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out:
			/// acceleration until halfway, then deceleration.
			/// </summary>
			BackEaseInOut,
			/// <summary>
			/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in:
			/// deceleration until halfway, then acceleration.
			/// </summary>
			BackEaseOutIn,
			/// <summary>
			/// "Easing" equation function for a snapping to the final value immediately.
			/// </summary>
			SnapOut,
			/// <summary>
			/// "Easing" equation function for a snapping to the start value immediately.
			/// </summary>
			SnapIn,
			/// <summary>
			/// "Easing" equation function for a snapping to the start or final value,
			/// depending the time is before or after the half way point.
			/// </summary>
			SnapInOut,
			/// <summary>
			/// "Easing" equation function for a snapping to the start or final value, depending the time is before or after the half way point.
			/// </summary>
			SnapOutIn,
		};

		/// <summary>
		/// Interpolate provides an easy to use interface to use the easing functions
		/// like you would any other interpolation function. The result is unclamped.
		/// </summary>
		/// <param name="function">The easing function to use.</param>
		/// <param name="value0">The starting value, which guarentees v = v0 when t == 0.0</param>
		/// <param name="value1">The ending value, which guarentees v = v1 when t == 1.0</param>
		/// <param name="t">Where on the function to interpolate to.</param>
		/// <returns>The interpolated value.</returns>
		public static double Interpolate(this Function function, double value0, double value1, double t)
		{
			const double interpolateEaseStart = 0.0;
			const double interpolateEaseEnd = 1.0;

			double easeResult = function.Ease(0, interpolateEaseStart, interpolateEaseEnd, t);
			return Math.Lerp(value0, value1, easeResult);
		}

		/// <summary>
		/// Eases a value from "from" to "to", based on the time and duration. The easing
		/// type is determined by the function.
		/// </summary>
		/// <param name="function">The easing type to use.</param>
		/// <param name="time">The time through the ease.</param>
		/// <param name="from">The initial value to ease from.</param>
		/// <param name="to">The destination value. (Note that the destination will actually be the sum of from + destination, if from is non-zero!)</param>
		/// <param name="duration">The duration of the ease.</param>
		/// <returns>The eased value.</returns>
		public static double Ease(this Function function, double time, double from, double to, double duration)
		{
			return function switch
			{
				Function.Linear => Linear(time, from, to, duration),
				Function.ExpoEaseOut => ExpoEaseOut(time, from, to, duration),
				Function.ExpoEaseIn => ExpoEaseIn(time, from, to, duration),
				Function.ExpoEaseInOut => ExpoEaseInOut(time, from, to, duration),
				Function.ExpoEaseOutIn => ExpoEaseOutIn(time, from, to, duration),
				Function.CircEaseOut => CircEaseOut(time, from, to, duration),
				Function.CircEaseIn => CircEaseIn(time, from, to, duration),
				Function.CircEaseInOut => CircEaseInOut(time, from, to, duration),
				Function.CircEaseOutIn => CircEaseOutIn(time, from, to, duration),
				Function.QuadEaseOut => QuadEaseOut(time, from, to, duration),
				Function.QuadEaseIn => QuadEaseIn(time, from, to, duration),
				Function.QuadEaseInOut => QuadEaseInOut(time, from, to, duration),
				Function.QuadEaseOutIn => QuadEaseOutIn(time, from, to, duration),
				Function.SineEaseOut => SineEaseOut(time, from, to, duration),
				Function.SineEaseIn => SineEaseIn(time, from, to, duration),
				Function.SineEaseInOut => SineEaseInOut(time, from, to, duration),
				Function.SineEaseOutIn => SineEaseOutIn(time, from, to, duration),
				Function.CubicEaseOut => CubicEaseOut(time, from, to, duration),
				Function.CubicEaseIn => CubicEaseIn(time, from, to, duration),
				Function.CubicEaseInOut => CubicEaseInOut(time, from, to, duration),
				Function.CubicEaseOutIn => CubicEaseOutIn(time, from, to, duration),
				Function.QuartEaseOut => QuartEaseOut(time, from, to, duration),
				Function.QuartEaseIn => QuartEaseIn(time, from, to, duration),
				Function.QuartEaseInOut => QuartEaseInOut(time, from, to, duration),
				Function.QuartEaseOutIn => QuartEaseOutIn(time, from, to, duration),
				Function.QuintEaseOut => QuintEaseOut(time, from, to, duration),
				Function.QuintEaseIn => QuintEaseIn(time, from, to, duration),
				Function.QuintEaseInOut => QuintEaseInOut(time, from, to, duration),
				Function.QuintEaseOutIn => QuintEaseOutIn(time, from, to, duration),
				Function.ElasticEaseOut => ElasticEaseOut(time, from, to, duration),
				Function.ElasticEaseIn => ElasticEaseIn(time, from, to, duration),
				Function.ElasticEaseInOut => ElasticEaseInOut(time, from, to, duration),
				Function.ElasticEaseOutIn => ElasticEaseOutIn(time, from, to, duration),
				Function.BounceEaseOut => BounceEaseOut(time, from, to, duration),
				Function.BounceEaseIn => BounceEaseIn(time, from, to, duration),
				Function.BounceEaseInOut => BounceEaseInOut(time, from, to, duration),
				Function.BounceEaseOutIn => BounceEaseOutIn(time, from, to, duration),
				Function.BackEaseOut => BackEaseOut(time, from, to, duration),
				Function.BackEaseIn => BackEaseIn(time, from, to, duration),
				Function.BackEaseInOut => BackEaseInOut(time, from, to, duration),
				Function.BackEaseOutIn => BackEaseOutIn(time, from, to, duration),
				Function.SnapOut => SnapOut(time, from, to, duration),
				Function.SnapIn => SnapIn(time, from, to, duration),
				Function.SnapInOut => SnapInOut(time, from, to, duration),
				Function.SnapOutIn => SnapOutIn(time, from, to, duration),
				_ => throw new System.NotSupportedException($"Easing function {function} is not supported.")
			};
		}

		#endregion

		#region Equations

		#region Linear

		/// <summary>
		/// Easing equation function for a simple linear interpolation, with no easing.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double Linear(double t, double b, double c, double d)
			=> (c * t / d) + b;

		#endregion

		#region Expo

		/// <summary>
		/// Easing equation function for an exponential (2^t) easing out:
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double ExpoEaseOut(double t, double b, double c, double d)
			=> (t == d) ? b + c : (c * (-Pow(2, -10 * t / d) + 1)) + b;

		/// <summary>
		/// Easing equation function for an exponential (2^t) easing in:
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double ExpoEaseIn(double t, double b, double c, double d)
			=> (t == 0) ? b : (c * Pow(2, 10 * ((t / d) - 1))) + b;

		/// <summary>
		/// Easing equation function for an exponential (2^t) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double ExpoEaseInOut(double t, double b, double c, double d)
		{
			if (t == 0)
			{
				return b;
			}

			if (t == d)
			{
				return b + c;
			}

			if ((t /= d / 2) < 1)
			{
				return (c / 2 * Pow(2, 10 * (t - 1))) + b;
			}

			return (c / 2 * (-Pow(2, -10 * --t) + 2)) + b;
		}

		/// <summary>
		/// Easing equation function for an exponential (2^t) easing out/in:
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double ExpoEaseOutIn(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return ExpoEaseOut(t * 2, b, c / 2, d);
			}

			return ExpoEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
		}

		#endregion

		#region Circular

		/// <summary>
		/// Easing equation function for a circular (sqrt(1-t^2)) easing out:
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double CircEaseOut(double t, double b, double c, double d)
			=> (c * Sqrt(1 - ((t = (t / d) - 1) * t))) + b;

		/// <summary>
		/// Easing equation function for a circular (sqrt(1-t^2)) easing in:
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double CircEaseIn(double t, double b, double c, double d)
			=> (-c * (Sqrt(1 - ((t /= d) * t)) - 1)) + b;

		/// <summary>
		/// Easing equation function for a circular (sqrt(1-t^2)) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double CircEaseInOut(double t, double b, double c, double d)
		{
			if ((t /= d / 2) < 1)
			{
				return (-c / 2 * (Sqrt(1 - (t * t)) - 1)) + b;
			}

			return (c / 2 * (Sqrt(1 - ((t -= 2) * t)) + 1)) + b;
		}

		/// <summary>
		/// Easing equation function for a circular (sqrt(1-t^2)) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double CircEaseOutIn(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return CircEaseOut(t * 2, b, c / 2, d);
			}

			return CircEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
		}

		#endregion

		#region Quad

		/// <summary>
		/// Easing equation function for a quadratic (t^2) easing out:
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuadEaseOut(double t, double b, double c, double d)
			=> (-c * (t /= d) * (t - 2)) + b;

		/// <summary>
		/// Easing equation function for a quadratic (t^2) easing in:
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuadEaseIn(double t, double b, double c, double d)
			=> (c * (t /= d) * t) + b;

		/// <summary>
		/// Easing equation function for a quadratic (t^2) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuadEaseInOut(double t, double b, double c, double d)
		{
			if ((t /= d / 2) < 1)
			{
				return (c / 2 * t * t) + b;
			}

			return (-c / 2 * (((--t) * (t - 2)) - 1)) + b;
		}

		/// <summary>
		/// Easing equation function for a quadratic (t^2) easing out/in:
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuadEaseOutIn(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return QuadEaseOut(t * 2, b, c / 2, d);
			}

			return QuadEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
		}

		#endregion

		#region Sine

		/// <summary>
		/// Easing equation function for a sinusoidal (sin(t)) easing out:
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double SineEaseOut(double t, double b, double c, double d)
			=> (c * Sin(t / d * (PI / 2))) + b;

		/// <summary>
		/// Easing equation function for a sinusoidal (sin(t)) easing in:
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double SineEaseIn(double t, double b, double c, double d)
			=> (-c * Cos(t / d * (PI / 2))) + c + b;

		/// <summary>
		/// Easing equation function for a sinusoidal (sin(t)) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double SineEaseInOut(double t, double b, double c, double d)
		{
			if ((t /= d / 2) < 1)
			{
				return (c / 2 * Sin(PI * t / 2)) + b;
			}

			return (-c / 2 * (Cos(PI * --t / 2) - 2)) + b;
		}

		/// <summary>
		/// Easing equation function for a sinusoidal (sin(t)) easing in/out:
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double SineEaseOutIn(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return SineEaseOut(t * 2, b, c / 2, d);
			}

			return SineEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
		}

		#endregion

		#region Cubic

		/// <summary>
		/// Easing equation function for a cubic (t^3) easing out:
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double CubicEaseOut(double t, double b, double c, double d)
			=> (c * (((t = (t / d) - 1) * t * t) + 1)) + b;

		/// <summary>
		/// Easing equation function for a cubic (t^3) easing in:
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double CubicEaseIn(double t, double b, double c, double d)
			=> (c * (t /= d) * t * t) + b;

		/// <summary>
		/// Easing equation function for a cubic (t^3) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double CubicEaseInOut(double t, double b, double c, double d)
		{
			if ((t /= d / 2) < 1)
			{
				return (c / 2 * t * t * t) + b;
			}

			return (c / 2 * (((t -= 2) * t * t) + 2)) + b;
		}

		/// <summary>
		/// Easing equation function for a cubic (t^3) easing out/in:
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double CubicEaseOutIn(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return CubicEaseOut(t * 2, b, c / 2, d);
			}

			return CubicEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
		}

		#endregion

		#region Quartic

		/// <summary>
		/// Easing equation function for a quartic (t^4) easing out:
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuartEaseOut(double t, double b, double c, double d)
			=> (-c * (((t = (t / d) - 1) * t * t * t) - 1)) + b;

		/// <summary>
		/// Easing equation function for a quartic (t^4) easing in:
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuartEaseIn(double t, double b, double c, double d)
			=> (c * (t /= d) * t * t * t) + b;

		/// <summary>
		/// Easing equation function for a quartic (t^4) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuartEaseInOut(double t, double b, double c, double d)
		{
			if ((t /= d / 2) < 1)
			{
				return (c / 2 * t * t * t * t) + b;
			}

			return (-c / 2 * (((t -= 2) * t * t * t) - 2)) + b;
		}

		/// <summary>
		/// Easing equation function for a quartic (t^4) easing out/in:
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuartEaseOutIn(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return QuartEaseOut(t * 2, b, c / 2, d);
			}

			return QuartEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
		}

		#endregion

		#region Quintic

		/// <summary>
		/// Easing equation function for a quintic (t^5) easing out:
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuintEaseOut(double t, double b, double c, double d)
			=> (c * (((t = (t / d) - 1) * t * t * t * t) + 1)) + b;

		/// <summary>
		/// Easing equation function for a quintic (t^5) easing in:
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuintEaseIn(double t, double b, double c, double d)
			=> (c * (t /= d) * t * t * t * t) + b;

		/// <summary>
		/// Easing equation function for a quintic (t^5) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuintEaseInOut(double t, double b, double c, double d)
		{
			if ((t /= d / 2) < 1)
			{
				return (c / 2 * t * t * t * t * t) + b;
			}

			return (c / 2 * (((t -= 2) * t * t * t * t) + 2)) + b;
		}

		/// <summary>
		/// Easing equation function for a quintic (t^5) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double QuintEaseOutIn(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return QuintEaseOut(t * 2, b, c / 2, d);
			}

			return QuintEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
		}

		#endregion

		#region Elastic

		/// <summary>
		/// Easing equation function for an elastic (exponentially decaying sine wave) easing out:
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double ElasticEaseOut(double t, double b, double c, double d)
		{
			if ((t /= d) == 1)
			{
				return b + c;
			}

			double p = d * .3f;
			double s = p / 4;

			return ((c * Pow(2, -10 * t) * Sin(((t * d) - s) * (2 * PI) / p)) + c + b);
		}

		/// <summary>
		/// Easing equation function for an elastic (exponentially decaying sine wave) easing in:
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double ElasticEaseIn(double t, double b, double c, double d)
		{
			if ((t /= d) == 1)
			{
				return b + c;
			}

			double p = d * .3f;
			double s = p / 4;

			return -(c * Pow(2, 10 * (t -= 1)) * Sin(((t * d) - s) * (2 * PI) / p)) + b;
		}

		/// <summary>
		/// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double ElasticEaseInOut(double t, double b, double c, double d)
		{
			if ((t /= d / 2) == 2)
			{
				return b + c;
			}

			double p = d * (.3f * 1.5f);
			double s = p / 4;

			if (t < 1)
			{
				return (-.5f * (c * Pow(2, 10 * (t -= 1)) * Sin(((t * d) - s) * (2 * PI) / p))) + b;
			}

			return (c * Pow(2, -10 * (t -= 1)) * Sin(((t * d) - s) * (2 * PI) / p) * .5f) + c + b;
		}

		/// <summary>
		/// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in:
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double ElasticEaseOutIn(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return ElasticEaseOut(t * 2, b, c / 2, d);
			}

			return ElasticEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
		}

		#endregion

		#region Bounce

		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out:
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double BounceEaseOut(double t, double b, double c, double d)
		{
			if ((t /= d) < (1f / 2.75f))
			{
				return (c * (7.5625f * t * t)) + b;
			}
			else if (t < (2f / 2.75f))
			{
				return (c * ((7.5625f * (t -= (1.5f / 2.75f)) * t) + .75f)) + b;
			}
			else if (t < (2.5f / 2.75f))
			{
				return (c * ((7.5625f * (t -= (2.25f / 2.75f)) * t) + .9375f)) + b;
			}
			else
			{
				return (c * ((7.5625f * (t -= (2.625f / 2.75f)) * t) + .984375f)) + b;
			}
		}

		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in:
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double BounceEaseIn(double t, double b, double c, double d)
			=> c - BounceEaseOut(d - t, 0, c, d) + b;

		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double BounceEaseInOut(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return (BounceEaseIn(t * 2, 0, c, d) * .5f) + b;
			}
			else
			{
				return (BounceEaseOut((t * 2) - d, 0, c, d) * .5f) + (c * .5f) + b;
			}
		}

		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in:
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double BounceEaseOutIn(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return BounceEaseOut(t * 2, b, c / 2, d);
			}

			return BounceEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
		}

		#endregion

		#region Back

		/// <summary>
		/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out:
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double BackEaseOut(double t, double b, double c, double d) => (c * (((t = (t / d) - 1) * t * (((1.70158f + 1) * t) + 1.70158f)) + 1)) + b;

		/// <summary>
		/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in:
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double BackEaseIn(double t, double b, double c, double d) => (c * (t /= d) * t * (((1.70158f + 1) * t) - 1.70158f)) + b;

		/// <summary>
		/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out:
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double BackEaseInOut(double t, double b, double c, double d)
		{
			double s = 1.70158f;
			if ((t /= d / 2) < 1)
			{
				return (c / 2 * (t * t * ((((s *= (1.525f)) + 1) * t) - s))) + b;
			}

			return (c / 2 * (((t -= 2) * t * ((((s *= (1.525f)) + 1) * t) + s)) + 2)) + b;
		}

		/// <summary>
		/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in:
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double BackEaseOutIn(double t, double b, double c, double d)
		{
			if (t < d / 2)
			{
				return BackEaseOut(t * 2, b, c / 2, d);
			}

			return BackEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
		}

		#endregion

		#region Snap

		/// <summary>
		/// "Easing" equation function for a snapping to the final value immediately.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value, c.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Match other ease functions.")]
		public static double SnapOut(double t, double b, double c, double d)
			=> c;

		/// <summary>
		/// "Easing" equation function for a snapping to the start value immediately.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value, b.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Match other ease functions.")]
		public static double SnapIn(double t, double b, double c, double d)
			=> b;

		/// <summary>
		/// "Easing" equation function for a snapping to the start or final value,
		/// depending the time is before or after the half way point.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double SnapInOut(double t, double b, double c, double d)
			=> (t < d / 2) ? b : c;

		/// <summary>
		/// "Easing" equation function for a snapping to the start or final value, depending the time is before or after the half way point.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The eased value.</returns>
		public static double SnapOutIn(double t, double b, double c, double d)
			=> (t < d / 2) ? c : b;

		#endregion

		#endregion

	}
}
