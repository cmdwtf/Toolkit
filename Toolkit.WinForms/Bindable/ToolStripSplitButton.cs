using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace cmdwtf.Toolkit.WinForms.Bindable
{
	/// <summary>
	/// A <see cref="System.Windows.Forms.ToolStripSplitButton"/> that provides
	/// a bindable interface akin to most WinForms controls.
	/// </summary>
	[ToolboxItem(true)]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	public class ToolStripSplitButton : System.Windows.Forms.ToolStripSplitButton, IBindableComponent
	{
		private BindingContext _context = null;
		private ControlBindingsCollection _bindings = null;

		/// <summary>
		/// Gets or sets the <see cref="System.Windows.Forms.BindingContext"/> for
		/// this control.
		/// </summary>
		public BindingContext BindingContext
		{
			get => _context ??= new BindingContext();
			set => _context = value;
		}

		/// <summary>
		/// Gets or sets the <see cref="ControlBindingsCollection"/> for this control.
		/// </summary>
		public ControlBindingsCollection DataBindings
		{
			get => _bindings ??= new ControlBindingsCollection(this);
			set => _bindings = value;
		}

		/// <summary>
		/// Handles the disposing of this control,
		/// disposing of the bindings in addition to
		/// the base class's Dispose.()
		/// </summary>
		/// <param name="disposing">true if disposing, false if finalizing.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			_bindings?.Clear();
			_bindings = null;
			_context = null;
		}
	}
}
