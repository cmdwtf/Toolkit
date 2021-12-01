using System;
using System.Threading;
using System.Threading.Tasks;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// A set of functions that easily run async methods as sync.
	/// Reproduced from AspNetIdentity, MIT licensed © Microsoft Corporation, Inc.
	/// https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/AsyncHelper.cs
	/// </summary>
	public static class Async
	{
		private static readonly TaskFactory AsyncHelperTaskFactory = new
			 (CancellationToken.None,
			  TaskCreationOptions.None,
			  TaskContinuationOptions.None,
			  TaskScheduler.Default);

		/// <summary>
		/// Runs the given operation synchronously.
		/// </summary>
		/// <typeparam name="TResult">The result type the function's task will return.</typeparam>
		/// <param name="func">The operation to run.</param>
		/// <returns>The unwrapped result from the operation.</returns>
		public static TResult RunSync<TResult>(Func<Task<TResult>> func)
		{
			return AsyncHelperTaskFactory
			  .StartNew<Task<TResult>>(func)
			  .Unwrap<TResult>()
			  .GetAwaiter()
			  .GetResult();
		}

		/// <summary>
		/// Runs the given operation synchronously.
		/// </summary>
		/// <param name="func">The operation to run.</param>
		public static void RunSync(Func<Task> func)
		{
			AsyncHelperTaskFactory
			  .StartNew<Task>(func)
			  .Unwrap()
			  .GetAwaiter()
			  .GetResult();
		}
	}
}
