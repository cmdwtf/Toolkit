using System.Collections.Concurrent;
using System.Windows.Forms.VisualStyles;

using VSE = System.Windows.Forms.VisualStyles.VisualStyleElement;
using VSR = System.Windows.Forms.VisualStyles.VisualStyleRenderer;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// A small class to cache <see cref="VSR"/>s by the element they
	/// were created by.
	/// </summary>
	public class VisualStyleRendererCache
	{
		private readonly ConcurrentDictionary<VSE, VSR> _cache;

		/// <summary>
		/// Creates a new instance of <see cref="VisualStyleRendererCache"/>.
		/// </summary>
		public VisualStyleRendererCache()
		{
			_cache = new ConcurrentDictionary<VSE, VSR>();
		}

		/// <summary>
		/// Gets (or creates and caches) a <see cref="VSR"/> for the given <see cref="VSE"/>.
		/// </summary>
		/// <param name="element">The <see cref="VSE"/> to get a renderer for.</param>
		/// <returns>The <see cref="VSR"/>.</returns>
		public VSR GetCachedRenderer(VSE element)
		{
			if (!_cache.ContainsKey(element))
			{
				_cache[element] = new VisualStyleRenderer(element);
			}

			return _cache[element];
		}

	}
}
