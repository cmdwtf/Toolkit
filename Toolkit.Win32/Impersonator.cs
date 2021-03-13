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
	/// </remarks>
	[SupportedOSPlatform("windows")]
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
		public WindowsIdentity CurrentIdentity { get; private set; }

		public bool LoggedOn => _accessToken != IntPtr.Zero;

		private IntPtr _accessToken = IntPtr.Zero;
		private const int _logonType = AdvancedApi.LOGON32_LOGON_INTERACTIVE;
		private const int _logonProvider = AdvancedApi.LOGON32_PROVIDER_DEFAULT;

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

				bool loggedOn = AdvancedApi.LogonUser(userName, domainName, password, _logonType, _logonProvider, ref _accessToken);

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
				_ = Kernel.CloseHandle(_accessToken);
			}

			_accessToken = IntPtr.Zero;

			if (CurrentIdentity != null)
			{
				CurrentIdentity.Dispose();
				CurrentIdentity = WindowsIdentity.GetCurrent();
			}
		}

		//
		// Summary:
		//     Runs the specified function as the impersonated Windows identity. Instead of
		//     using an impersonated method call and running your function in System.Security.Principal.WindowsImpersonationContext,
		//     you can use System.Security.Principal.WindowsIdentity.RunImpersonated(Microsoft.Win32.SafeHandles.SafeAccessTokenHandle,System.Action)
		//     and provide your function directly as a parameter.
		//
		// Parameters:
		//   func:
		//     The System.Func to run.
		//
		// Type parameters:
		//   T:
		//     The type of object used by and returned by the function.
		//
		// Returns:
		//     The result of the function.
		public T RunImpersonated<T>(Func<T> func)
		{
			if (LoggedOn == false)
			{
				throw new UnauthorizedAccessException("You are not logged on as a user to impersonate!");
			}

			return WindowsIdentity.RunImpersonated(CurrentIdentity.AccessToken, func);
		}

		//
		// Summary:
		//     Runs the specified action as the impersonated Windows identity. Instead of using
		//     an impersonated method call and running your function in System.Security.Principal.WindowsImpersonationContext,
		//     you can use System.Security.Principal.WindowsIdentity.RunImpersonated(Microsoft.Win32.SafeHandles.SafeAccessTokenHandle,System.Action)
		//     and provide your function directly as a parameter.
		//
		// Parameters:
		//   action:
		//     The System.Action to run.
		public void RunImpersonated(Action action)
		{
			if (LoggedOn == false)
			{
				throw new UnauthorizedAccessException("You are not logged on as a user to impersonate!");
			}

			WindowsIdentity.RunImpersonated(CurrentIdentity.AccessToken, action);
		}
		//
		// Summary:
		//     Runs the specified asynchronous action as the impersonated Windows identity.
		//
		// Parameters:
		//   func:
		//     The function to run.
		//
		// Type parameters:
		//   T:
		//     The type of the object to return.
		//
		// Returns:
		//     A task that represents the asynchronous operation of func.
		public async Task<T> RunImpersonatedAsync<T>(Func<Task<T>> func)
		{
			if (LoggedOn == false)
			{
				throw new UnauthorizedAccessException("You are not logged on as a user to impersonate!");
			}

			return await WindowsIdentity.RunImpersonatedAsync(CurrentIdentity.AccessToken, func).ConfigureAwait(false);
		}
		//
		// Summary:
		//     Runs the specified asynchronous action as the impersonated Windows identity.
		//
		// Parameters:
		//   func:
		//     The function to run.
		//
		// Returns:
		//     A task that represents the asynchronous operation of the provided System.Func`1.
		public async Task RunImpersonatedAsync(Func<Task> func)
		{
			if (LoggedOn == false)
			{
				throw new UnauthorizedAccessException("You are not logged on as a user to impersonate!");
			}

			await WindowsIdentity.RunImpersonatedAsync(CurrentIdentity.AccessToken, func).ConfigureAwait(false);
		}

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
