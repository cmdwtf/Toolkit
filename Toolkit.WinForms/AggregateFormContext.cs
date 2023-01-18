using System;
using System.Threading;
using System.Windows.Forms;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// An <see cref="ApplicationContext"/> that binds the lifetime
	/// of one or more forms together, so that it doesn't end until
	/// the last form is closed.
	/// </summary>
	/// <remarks>
	/// Based on <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.applicationcontext.exitthreadcore">.NET Documentation</seealso>
	/// and <seealso href="https://stackoverflow.com/a/15301089">this stackoverflow answer</seealso>.
	/// </remarks>
	public class AggregateFormContext : ApplicationContext
	{
		private int _activeForms;

		/// <summary>
		/// Creates a new <see cref="AggregateFormContext"/> that
		/// will stay alive until all the provided forms are closed.
		/// </summary>
		/// <param name="forms">The forms for the context to show and monitor.</param>
		/// <exception cref="ArgumentException">If <paramref name="forms"/> is null or empty.</exception>
		public AggregateFormContext(params Form[] forms)
		{
			_activeForms = forms?.Length ?? 0;

			if (_activeForms == 0 || forms == null)
			{
				throw new ArgumentException($"{nameof(forms)} must provide at least a single {nameof(Form)} in order to create the context.", nameof(forms));
			}

			foreach (Form form in forms)
			{
				form.FormClosed += FormClosedHandler;
				form.Show();
			}
		}

		private void FormClosedHandler(object? sender, FormClosedEventArgs args)
		{
			if (Interlocked.Decrement(ref _activeForms) == 0)
			{
				// our last form closed, our context can end.
				ExitThread();
			}
		}
	}
}
