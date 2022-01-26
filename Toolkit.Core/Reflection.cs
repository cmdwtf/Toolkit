using System;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// Reflection tools. Mostly just shorthand for regularly done things that aren't
	/// done frequently enough to justify larger boilerplate.
	/// </summary>
	public static class Reflection
	{
		/// <summary>
		/// Gets the named property from the given object. Attempts to recurse through
		/// an object if it is a complex type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The object to get the property value of.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>The property value, if found. Otherwise, default.</returns>
		public static T GetPropertyValue<T>(object obj, string propertyName)
		{
			if (obj == null)
			{
				throw new ArgumentException("Value cannot be null.", nameof(obj));
			}

			if (propertyName == null)
			{
				throw new ArgumentException("Value cannot be null.", nameof(propertyName));
			}

			// check for complex/nested type
			if (propertyName.Contains('.'))
			{
				string[] temp = propertyName.Split(new char[] { '.' }, 2);
				return (T)GetPropertyValue(GetPropertyValue(obj, temp[0]), temp[1]);
			}
			else
			{
				System.Reflection.PropertyInfo prop = obj.GetType().GetProperty(propertyName);
				return (T)prop?.GetValue(obj, null) ?? default;
			}
		}

		/// <inheritdoc cref="GetPropertyValue{T}(object, string)"/>
		public static object GetPropertyValue(object src, string propName)
			=> GetPropertyValue<object>(src, propName);
	}
}
