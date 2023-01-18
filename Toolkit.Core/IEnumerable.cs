using System.Collections.Generic;
using System.Linq;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// (I)Enumerable tools. Mostly just shorthand for regularly done things that aren't
	/// done frequently enough to justify larger boilerplate.
	/// </summary>
	public static class IEnumerable
	{
		/// <summary>
		/// An extension that returns an enumerable of non-nullable from one that is nullable.
		/// As well, filters the enumerable to remove null values.
		/// </summary>
		/// <typeparam name="T">The non-nullable type to return.</typeparam>
		/// <param name="enumerable">The collection to enumerate.</param>
		/// <returns>A sequence of non-nullable items.</returns>
		public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class
		{
			return enumerable.Where(e => e != null).Select(e => e!);
		}
	}
}
