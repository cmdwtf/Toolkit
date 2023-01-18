using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

// Bindable 

namespace cmdwtf.Toolkit.WinForms.Controls.Bindable
{
	/// <summary>
	/// A <see cref="System.Windows.Forms.ToolStripLabel"/> that provides
	/// a bindable interface akin to most WinForms controls.
	/// </summary>
	/// <remarks>In .NET 7.0 or later, is just a regular
	/// <see cref="System.Windows.Forms.ToolStripLabel"/>, since binding was added.</remarks>
	[ToolboxItem(true)]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
#if NET7_0_OR_GREATER
	public class ToolStripLabel : System.Windows.Forms.ToolStripLabel { /* nothing to add */ }
#else
	public class ToolStripLabel : System.Windows.Forms.ToolStripLabel, IBindableComponent
	{
		private BindingContext? _context = null;
		private ControlBindingsCollection? _bindings = null;

		/// <summary>
		/// Gets or sets the <see cref="System.Windows.Forms.BindingContext"/> for
		/// this control.
		/// </summary>
		public BindingContext? BindingContext
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
#endif // NET7_0_OR_GREATER
}
