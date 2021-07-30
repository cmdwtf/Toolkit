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
		private static readonly SendOrPostCallback _postCallback = state => ((Action)state)();

		private readonly SynchronizationContext _context;
		public SynchronizationContextAwaiter(SynchronizationContext context)
		{
			_context = context;
		}

		public bool IsCompleted => _context == SynchronizationContext.Current;

		public void OnCompleted(Action continuation) => _context.Post(_postCallback, continuation);

		public void GetResult() { }
	}

	/// <summary>
	/// An extension method to actually get the awaiter.
	/// </summary>
	public static class SynchronizationContextAwaiterExtensions
	{
		public static SynchronizationContextAwaiter GetAwaiter(this SynchronizationContext context)
			=> new SynchronizationContextAwaiter(context);
	}
}
