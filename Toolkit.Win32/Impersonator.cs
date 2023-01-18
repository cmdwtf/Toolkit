using System;
using System.Runtime.Versioning;
using System.Security.Principal;
using System.Threading.Tasks;

using cmdwtf.Toolkit.Win32.Native;

namespace cmdwtf.Toolkit.Win32
{
	/// <summary>
	/// A small class used to impersonate another windows user.
	/// </summary>
	/// <remarks>
	/// Based on information from, and general design:
	/// https://stackoverflow.com/a/51799484/944605
	/// Worth a look if you need a slightly more robust impersonation framework:
	/// https://github.com/mattjohnsonpint/SimpleImpersonation
	/// </remarks>
#if NET5_0_OR_GREATER
	[SupportedOSPlatform("windows")]
#endif //NET5_0_OR_GREATER
	public class Impersonator : IDisposable
	{
		/// <summary>
		/// Creates a new, logged out instance of the Impersonator class.
		/// </summary>
		public Impersonator()
		{
			CurrentIdentity = WindowsIdentity.GetCurrent();
		}

		/// <summary>
		/// Creates a new instance of the Impersonator class, and logs in immediately.
		/// You can use this constructor in a using statement for easy lifetime management.
		/// </summary>
		/// <param name="userName">The user to login as.</param>
		/// <param name="domainName">The domain of the user.</param>
		/// <param name="password">The user's password.</param>
		public Impersonator(string userName, string domainName, string password)
		{
			Login(userName, domainName, password);
		}

		/// <summary>
		/// Finalizer.
		/// </summary>
		~Impersonator()
		{
			Logout();
		}

		/// <summary>
		/// The identity associated with the Impersonator. If you are not logged
		/// in, then it will represent your current user's identity.
		/// </summary>
		public WindowsIdentity? CurrentIdentity { get; private set; }

		/// <summary>
		/// Gets a value indicating whether we are logged on or not.
		/// </summary>
		/// <value>
		///   <c>true</c> if we are logged on; otherwise, <c>false</c>.
		/// </value>
		public bool LoggedOn => _accessToken != IntPtr.Zero;

		private IntPtr _accessToken = IntPtr.Zero;
		private const int LogonType = AdvApi32.LOGON32_LOGON_INTERACTIVE;
		private const int LogonProvider = AdvApi32.LOGON32_PROVIDER_DEFAULT;

		/// <summary>
		/// Logs in, getting a windows identity representing the account you want
		/// to impersonate if successful.
		/// </summary>
		/// <param name="userName">The user to login as.</param>
		/// <param name="domainName">The domain of the user.</param>
		/// <param name="password">The user's password.</param>
		/// <returns>True, if login was successful.</returns>
		public bool Login(string userName, string domainName, string password)
		{

			if (CurrentIdentity != null)
			{
				CurrentIdentity.Dispose();
				CurrentIdentity = null;
			}

			try
			{
				Logout();

				bool loggedOn = AdvApi32.LogonUser(userName, domainName, password, LogonType, LogonProvider, ref _accessToken);

				if (loggedOn)
				{
					CurrentIdentity = new WindowsIdentity(_accessToken);
					return true;
				}

				Logout();

				return false;
			}
			catch
			{
				// could handle exceptions more gracefully.
				throw;
			}
		}

		/// <summary>
		/// Closes your impersonator login, if it's open.
		/// </summary>
		public void Logout()
		{
			if (LoggedOn)
			{
				_ = Kernel32.CloseHandle(_accessToken);
			}

			_accessToken = IntPtr.Zero;

			if (CurrentIdentity != null)
			{
				CurrentIdentity.Dispose();
				CurrentIdentity = WindowsIdentity.GetCurrent();
			}
		}

		/// <summary>
		/// Runs the specified action as the impersonated Windows identity.
		/// Instead of using an impersonated method call and running your
		/// function as the active <see cref="WindowsIdentity"/>,
		/// you can use <see cref="System.Security.Principal.WindowsIdentity.RunImpersonated(Microsoft.Win32.SafeHandles.SafeAccessTokenHandle,System.Action)"/>
		/// and provide your function directly as a parameter.
		/// </summary>
		/// <typeparam name="T">The type of the object to return.</typeparam>
		/// <param name="func">The function to run.</param>
		/// <returns>The result of the function.</returns>
		/// <exception cref="System.UnauthorizedAccessException">You are not logged on as a user to impersonate!</exception>
		public T RunImpersonated<T>(Func<T> func)
		{
			if (LoggedOn == false || CurrentIdentity == null)
			{
				throw new UnauthorizedAccessException("You are not logged on as a user to impersonate!");
			}

			return WindowsIdentity.RunImpersonated(CurrentIdentity.AccessToken, func);
		}

		/// <summary>
		/// Runs the specified action as the impersonated Windows identity.
		/// Instead of using an impersonated method call and running your
		/// function as the active <see cref="WindowsIdentity"/>,
		/// you can use <see cref="System.Security.Principal.WindowsIdentity.RunImpersonated(Microsoft.Win32.SafeHandles.SafeAccessTokenHandle,System.Action)"/>
		/// and provide your function directly as a parameter.
		/// </summary>
		/// <param name="action">The action to run.</param>
		/// <exception cref="System.UnauthorizedAccessException">You are not logged on as a user to impersonate!</exception>
		public void RunImpersonated(Action action)
		{
			if (LoggedOn == false || CurrentIdentity == null)
			{
				throw new UnauthorizedAccessException("You are not logged on as a user to impersonate!");
			}

			WindowsIdentity.RunImpersonated(CurrentIdentity.AccessToken, action);
		}


#if NET5_0_OR_GREATER

		/// <summary>
		/// Runs the specified asynchronous action as the impersonated Windows identity.
		/// </summary>
		/// <typeparam name="T">The type of the object to return.</typeparam>
		/// <param name="func">The function to run.</param>
		/// <returns>A task that represents the asynchronous operation of func.</returns>
		public async Task<T> RunImpersonatedAsync<T>(Func<Task<T>> func)
		{
			if (LoggedOn == false || CurrentIdentity == null)
			{
				throw new UnauthorizedAccessException("You are not logged on as a user to impersonate!");
			}

			return await WindowsIdentity.RunImpersonatedAsync(CurrentIdentity.AccessToken, func).ConfigureAwait(false);
		}

		/// <summary>
		/// Runs the specified asynchronous action as the impersonated Windows identity.
		/// </summary>
		/// <param name="func">The function to run.</param>
		/// <returns> A task that represents the asynchronous operation of the provided <see cref="System.Func{TResult}"/>.</returns>
		public async Task RunImpersonatedAsync(Func<Task> func)
		{
			if (LoggedOn == false || CurrentIdentity == null)
			{
				throw new UnauthorizedAccessException("You are not logged on as a user to impersonate!");
			}

			await WindowsIdentity.RunImpersonatedAsync(CurrentIdentity.AccessToken, func).ConfigureAwait(false);
		}

#else

		// these functions are not supported before .NET 5... And the attribute I have to get compile time errors, is called "Obsolete" 🤷🏻‍♀️

		/// <summary>
		/// A non-function that exists to inform the caller that we need to be on .NET 5 or later.
		/// </summary>
		/// <typeparam name="T">Unused.</typeparam>
		/// <param name="func">Unused.</param>
		/// <returns>Nothing.</returns>
		/// <exception cref="System.NotSupportedException">Always thrown.</exception>
		[Obsolete($"{nameof(RunImpersonatedAsync)} is not supported on .NET versions before .NET 5. Please use the {nameof(RunImpersonated)} instead.", error: true)]
		public Task<T> RunImpersonatedAsync<T>(Func<Task<T>> func) => throw new NotSupportedException($"{nameof(RunImpersonatedAsync)} is not supported!");

		/// <summary>
		/// A non-function that exists to inform the caller that we need to be on .NET 5 or later.
		/// </summary>
		/// <param name="func">Unused.</param>
		/// <returns>Nothing.</returns>
		/// <exception cref="System.NotSupportedException">Always thrown.</exception>
		[Obsolete($"{nameof(RunImpersonatedAsync)} is not supported on .NET versions before .NET 5. Please use the {nameof(RunImpersonated)} instead.", error: true)]
		public Task RunImpersonatedAsync(Func<Task> func) => throw new NotSupportedException($"{nameof(RunImpersonatedAsync)} is not supported!");

#endif // NET5_0_OR_GREATER

		#region IDisposable

		/// <summary>
		/// IDisposable implementation.
		/// </summary>
		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool _) => Logout();

		#endregion IDisposable
	}
}
