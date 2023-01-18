using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// An awaiter to easily switch back to a different (eg, the UI) synchronization context by just awaiting it.
	/// Via: https://thomaslevesque.com/2015/11/11/explicitly-switch-to-the-ui-thread-in-an-async-method/
	/// </summary>
	public struct SynchronizationContextAwaiter : INotifyCompletion
	{
		private static readonly SendOrPostCallback PostCallback = state => { if (state is Action a) { a(); } };

		private readonly SynchronizationContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="SynchronizationContextAwaiter"/> struct.
		/// </summary>
		/// <param name="context">The context.</param>
		public SynchronizationContextAwaiter(SynchronizationContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is completed.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is completed; otherwise, <c>false</c>.
		/// </value>
		public bool IsCompleted => _context == SynchronizationContext.Current;

		/// <summary>
		/// Schedules the continuation action that's invoked when the instance completes.
		/// </summary>
		/// <param name="continuation">The action to invoke when the operation completes.</param>
		public void OnCompleted(Action continuation) => _context.Post(PostCallback, continuation);

		/// <summary>
		/// Gets the result.
		/// </summary>
		public static void GetResult() { }
	}

	/// <summary>
	/// An extension method to actually get the awaiter.
	/// </summary>
	public static class SynchronizationContextAwaiterExtensions
	{
		/// <summary>
		/// Gets an awaiter for the given <see cref="SynchronizationContext"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>The awaiter.</returns>
		public static SynchronizationContextAwaiter GetAwaiter(this SynchronizationContext context)
			=> new(context);
	}
}
