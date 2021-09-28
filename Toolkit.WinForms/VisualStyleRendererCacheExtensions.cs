
using VSE = System.Windows.Forms.VisualStyles.VisualStyleElement;
using VSR = System.Windows.Forms.VisualStyles.VisualStyleRenderer;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// Extension methods for <see cref="VisualStyleRendererCache"/>.
	/// </summary>
	public static class VisualStyleRendererCacheExtensions
	{
		/// <summary>
		/// Gets (or creates and caches) a <see cref="VSR"/> for the given <see cref="VSE"/>.
		/// </summary>
		/// <param name="element">The <see cref="VSE"/> to get a renderer for.</param>
		/// <param name="cache">The cache to get the renderer from.</param>
		/// <returns>The <see cref="VSR"/>.</returns>
		public static VSR GetRenderer(this VSE element, VisualStyleRendererCache cache) => cache.GetCachedRenderer(element);
	}
}
