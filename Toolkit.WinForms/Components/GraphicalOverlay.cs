using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cmdwtf.Toolkit.WinForms.Components
{
	/// <summary>
	/// Provides a component that will allow drawing on top of a form's controls.
	/// </summary>
	/// <remarks>See also: https://www.codeproject.com/Articles/26071/Draw-Over-WinForms-Controls </remarks>
	public partial class GraphicalOverlay : Component
	{
		/// <summary>
		/// Creates a new graphical overlay component.
		/// </summary>
		public GraphicalOverlay()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Creates a new graphical overlay component, adding the provided
		/// container to it's container.
		/// </summary>
		/// <param name="container"></param>
		public GraphicalOverlay(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		/// <summary>
		/// Raised when the component needs to paint.
		/// </summary>
		public event EventHandler<PaintEventArgs> Paint;

		private Form _owner;

		/// <summary>
		/// The owning form of the overlay. The overlay will draw on top of
		/// the controls in this form.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Form Owner
		{
			get => _owner;
			set
			{
				// The owner form can only be set once.
				if (_owner != null)
				{
					throw new InvalidOperationException();
				}

				// Save the form for future reference.
				_owner = value ?? throw new ArgumentNullException(nameof(Owner));

				// Handle the form's Resize event.
				_owner.Resize += new EventHandler(Form_Resize);

				// Handle the Paint event for each of the controls in the form's hierarchy.
				ConnectPaintEventHandlers(_owner);
			}
		}

		/// <summary>
		/// Handles the owner's resize event, to cause an invalidation.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event data.</param>
		private void Form_Resize(object sender, EventArgs e)
			=> _owner.Invalidate(true);

		/// <summary>
		/// Resubscribes the provided control's Paint &amp; ControlAdded events
		/// to this graphical overlays handlers. As well, does the same
		/// for each of the control's children.
		/// </summary>
		/// <param name="control"></param>
		private void ConnectPaintEventHandlers(Control control)
		{
			// Connect the paint event handler for this control.
			// Remove the existing handler first (if one exists) and replace it.
			control.Paint -= Control_Paint;
			control.Paint += Control_Paint;

			control.ControlAdded -= Control_ControlAdded;
			control.ControlAdded += Control_ControlAdded;

			// Recurse the hierarchy.
			foreach (Control child in control.Controls)
			{
				ConnectPaintEventHandlers(child);
			}
		}

		/// <summary>
		/// Connects the paint event handler when a new control is added.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event data.</param>
		private void Control_ControlAdded(object sender, ControlEventArgs e) =>
			ConnectPaintEventHandlers(e.Control);

		/// <summary>
		/// Handle's a control's paint event to allow the user to draw on top
		/// of the control after it paints itself.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event data.</param>
		private void Control_Paint(object sender, PaintEventArgs e)
		{
			// As each control on the form is repainted, this handler is called.
			Control control = sender as Control ?? throw new InvalidOperationException($"{nameof(sender)} must be a ${nameof(Control)}.");
			Point location;

			// Determine the location of the control's client area relative to the form's client area.
			if (control == _owner)
			{
				// The form's client area is already form-relative.
				location = control.Location;
			}
			else
			{
				// The control may be in a hierarchy, so convert to screen coordinates and then back to form coordinates.
				location = _owner.PointToClient(control.Parent.PointToScreen(control.Location));

				// If the control has a border shift the location of the control's client area.
				location += new Size((control.Width - control.ClientSize.Width) / 2, (control.Height - control.ClientSize.Height) / 2);
			}

			// Translate the location so that we can use form-relative coordinates to draw on the control.
			if (control != _owner)
			{
				e.Graphics.TranslateTransform(-location.X, -location.Y);
			}

			// Fire a paint event.
			OnPaint(sender, e);
		}

		/// <summary>
		/// Raises the Paint event, passing along the original event sender.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event data.</param>
		private void OnPaint(object sender, PaintEventArgs e) =>
			Paint?.Invoke(sender, e);
	}
}

