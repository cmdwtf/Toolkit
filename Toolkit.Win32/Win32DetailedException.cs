using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cmdwtf.Toolkit.Win32
{
	/// <summary>
	/// The exception that is thrown for a Win32 error code, with additional application provided details.
	/// </summary>
	public class Win32DetailedException : Win32Exception
	{
		/// <inheritdoc/>
		public Win32DetailedException(string details)
		{
			Details = details;
		}

		/// <inheritdoc/>
		public Win32DetailedException(int error, string details) : base(error)
		{
			Details = details;
		}

		/// <inheritdoc/>
		public Win32DetailedException(string details, System.Exception? innerException) : base(null, innerException)
		{
			Details = details;
		}

		/// <summary>
		/// Represents additional details associated with this exception. This field is read-only.
		/// </summary>
		public string Details { get; }
	}
}
