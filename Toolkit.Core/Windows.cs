using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// Windows related tools. Currently mostly related to executing files with the openas verb.
	/// </summary>
	internal static class Windows
	{
		public static bool IsWindows =>
#if NET471_OR_GREATER || NETSTANDARD1_0 || NET5_0_OR_GREATER
			RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#elif NET47_OR_GREATER
			true;
#else
			throw new System.NotSupportedException($"{nameof(IsWindows)} depends on RuntimeInformation.IsOSPlatform, which isn't available until .NET 4.7.1");
#endif // NET471_OR_GREATER


		/// <summary>
		/// Optional settings to be passed to <see cref="Execute(string, string, ExecuteOptions)"/>.
		/// </summary>
		public record ExecuteOptions(
			string DefaultVerb = ExecuteOptions.EmptyVerb,
			bool AllowTryOpenAs = false,
			bool UseShellExecute = false
			)
		{
			private const string EmptyVerb = "";
		}

		/// <summary>
		/// Executes the given file with given optional options. Will try with
		/// the given verb, and optionally the addition of "openas", which works for
		/// non-executable files, like ".txt" or ".zip" files.
		/// </summary>
		/// <param name="fileName">The file to execute or open.</param>
		/// <param name="arguments">Arguments to pass as the process is started.</param>
		/// <param name="options">Optional settings to modify execute behavior.</param>
		/// <returns>The process object, if it was successfully started.</returns>
		/// <remarks>
		/// If there is an exception thrown during an attempt, and no attempt successfully
		/// starts the process, the most recent exception thrown will be thrown after the functions
		/// attempts all verbs it intends to try.
		/// </remarks>
		public static Process? Execute(string fileName, string arguments = "", ExecuteOptions? options = null)
		{
			const string noVerb = "";
			const string verbOpenAs = "openas";

			// try no verb to start if they didn't give us a verb
			var verbsToAttempt = new List<string>
			{
				options?.DefaultVerb ?? noVerb
			};

			if (options?.AllowTryOpenAs == true)
			{
				verbsToAttempt.Add(verbOpenAs);
			}

			Exception? startException = null;

			for (int scan = 0; scan < verbsToAttempt.Count; ++scan)
			{
				try
				{
					var psi = new ProcessStartInfo
					{
						FileName = fileName,
						UseShellExecute = options?.UseShellExecute ?? false,
						Verb = verbsToAttempt[scan],
						Arguments = arguments,
					};

					var result = Process.Start(psi);

					return result;
				}
				catch (Exception ex)
				{
					// store the exception to use later if other verbs fail.
					if (startException == null)
					{
						startException = ex;
					}
				}
			}

			if (startException != null)
			{
				throw startException;
			}

			return null;
		}

		/// <summary>
		/// Shorthand for using Execute with trying the "openas" verb, specifying shell execute.
		/// </summary>
		/// <param name="fi">The file info to execute.</param>
		/// <param name="allowTryOpenAs">If true, will attempt the openas verb if the default verb fails.</param>
		/// <returns>The process object, if it was successfully started.</returns>
		/// <remarks>
		/// If there is an exception thrown during an attempt, and no attempt successfully
		/// starts the process, the most recent exception thrown will be thrown after the functions
		/// attempts all verbs it intends to try.
		/// </remarks>
		public static void ShellExecute(FileInfo fi, bool allowTryOpenAs = true)
			=> ShellExecute(fi.FullName, allowTryOpenAs);

		/// <summary>
		/// Shorthand for using Execute with shell execute.
		/// </summary>
		/// <param name="fileName">The file to execute.</param>
		/// <param name="allowTryOpenAs">If true, will attempt the "openas" verb if the default verb fails.</param>
		/// <returns>The process object, if it was successfully started.</returns>
		/// <remarks>
		/// If there is an exception thrown during an attempt, and no attempt successfully
		/// starts the process, the most recent exception thrown will be thrown after the functions
		/// attempts all verbs it intends to try.
		/// </remarks>
		public static void ShellExecute(string fileName, bool allowTryOpenAs)
			=> Execute(fileName, string.Empty, new ExecuteOptions { AllowTryOpenAs = allowTryOpenAs, UseShellExecute = true });
	}
}
