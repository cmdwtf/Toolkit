using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using SRAssembly = System.Reflection.Assembly;

namespace cmdwtf.Toolkit
{
	public static class Assembly
	{
		/// <summary>
		/// Gets a formatted, printable string that includes the assembly name, copyright, and build time.
		/// </summary>
		/// <typeparam name="T">A type that is in the assemblyyou want to get the information from.</typeparam>
		/// <returns>A string in the format: AssemblyName x.x.x.x\nCopyright Information\nBuilt on: M/D/Y H:M:S</returns>
		public static string GetBuildAndCopyrightString<T>()
		{
			var assembly = SRAssembly.GetExecutingAssembly();
			string name = GetAssemblyName<T>();
			string version = assembly.GetName().Version.ToString();
			var configAttrib = assembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true)[0] as AssemblyConfigurationAttribute;
			string configuration = configAttrib.Configuration;

			if (string.IsNullOrWhiteSpace(configuration) == false)
			{
				version = $"{version} {configuration}";
			}

			string copyright = (assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true)[0] as AssemblyCopyrightAttribute).Copyright;
			DateTime linkerTimestamp = assembly.RetrieveLinkerTimestamp();
			string buildTime = linkerTimestamp.ToShortDateString() + " " + linkerTimestamp.ToShortTimeString();

			string buildString = $"{name} {version}{Environment.NewLine}{copyright}{Environment.NewLine}Built on: {buildTime}";

			return buildString;
		}

		/// <summary>
		/// Gets the real assembly name of the class provided.
		/// </summary>
		/// <typeparam name="T">The type to get the assembly name from.</typeparam>
		/// <returns>The name of the assembly title, if available. Otherwise, the executing assembly name.</returns>
		public static string GetAssemblyName<T>()
		{
			// get real assembly name, if possible.
			object[] attribs = typeof(T).Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
			if (attribs.Length > 0)
			{
				var titleAttribute = (AssemblyTitleAttribute)attribs[0];
				if (titleAttribute.Title.Length > 0)
				{
					return titleAttribute.Title;
				}
			}

			// couldn't get the title attribute, use executing assembly name.
			return SRAssembly.GetExecutingAssembly().GetName().Name;
		}

		/// <summary>
		/// Turns a version number into a build date/time.
		/// This only works with version numbers generated via the wildcard auto-version method, which is
		/// non-standard in .NET Core / .NET 5 or later. You must disable deterministic builds to use
		/// the auto-versioning. You could also set your version numbers via command line parameters
		/// (see: https://stackoverflow.com/a/46985624/944605), or stamp the version number in afterwards.
		/// </summary>
		/// <param name="version">The version to decode</param>
		/// <returns>The date time that the version represents</returns>
		/// <remarks>For more information: https://intellitect.com/making-sense-of-assemblyversion-numbers/</remarks>
		public static DateTime ToBuildDateTime(this Version version)
		{
			// Calculate assembly date:
			// Given a version in the format Major.Minor.Build.Revision
			// Date is calculated from 01/01/2000
			// With the build being the number of days after 01/01/2000
			// With the revision being half of the number of seconds into the day
			DateTime date = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)
				.AddDays(version.Build)
				.AddSeconds(version.Revision * 2);

			return date;
		}

		/// <summary>
		/// Turns a version number into a string representing a build date/time.
		/// This only works with version numbers generated via the wildcard auto-version method, which is
		/// non-standard in .NET Core / .NET 5 or later. You must disable deterministic builds to use
		/// the auto-versioning. You could also set your version numbers via command line parameters
		/// (see: https://stackoverflow.com/a/46985624/944605), or stamp the version number in afterwards.
		/// </summary>
		/// <param name="version">The version to decode</param>
		/// <returns>The string that the version represents</returns>
		/// <remarks>For more information: https://intellitect.com/making-sense-of-assemblyversion-numbers/</remarks>
		public static string ToBuildDateTimeString(this Version version)
		{
			DateTime date = version.ToBuildDateTime();
			return date.ToShortDateString() + " " + date.ToLongTimeString();
		}

		/// <summary>
		/// Returns a DateTime based on when the calling assembly was linked.
		/// </summary>
		/// <returns>A DateTime object based on the linker timestamp in the file.</returns>
		/// <remarks>Based on: https://stackoverflow.com/a/1600990/944605</remarks>
		public static DateTime RetrieveLinkerTimestampFromFile(TimeZoneInfo timeZoneInfo = null)
		{
			const int PEHeaderOffset = 60;
			const int LinkerTimestampOffset = 8;
			const int BufferSize = 2048;

			string filePath = SRAssembly.GetCallingAssembly().Location;
			byte[] buffer = new byte[BufferSize];

			try
			{
				using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
				stream.Read(buffer, 0, BufferSize);
			}
			catch
			{
				// failed to read the file.
				return DateTime.MinValue;
			}

			int timestampOffset = BitConverter.ToInt32(buffer, PEHeaderOffset);
			int secondsSince1970 = BitConverter.ToInt32(buffer, timestampOffset + LinkerTimestampOffset);

			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			DateTime linkTimeUtc = epoch.AddSeconds(secondsSince1970);
			DateTime result = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, timeZoneInfo ?? TimeZoneInfo.Local);

			return result;
		}

		/// <summary>
		/// Returns a DateTime based on when the calling assembly was linked. This functions similarly
		/// to <see cref="RetrieveLinkerTimestampFromFile(TimeZoneInfo)"/>, but reads it from memory instead.
		/// </summary>
		/// <returns>A DateTime object based on the linker timestamp in the file.</returns>
		/// <remarks>Based on: https://stackoverflow.com/a/44511677/944605</remarks>
		public static DateTime RetrieveLinkerTimestamp(this System.Reflection.Assembly assembly, TimeZoneInfo timeZoneInfo = null)
		{
			const int PEHeaderOffset = 60;
			const int LinkerTimestampOffset = 8;

			// Discover the base memory address where our assembly is loaded
			Module module = assembly.ManifestModule;
			System.IntPtr hModule = Marshal.GetHINSTANCE(module);

			if (hModule == Pointers.IntPtr.MinusOne)
			{
				throw new ExternalException("Failed to get HINSTANCE.", Marshal.GetLastWin32Error());
			}

			// Read the linker timestamp
			int timestampOffset = Marshal.ReadInt32(hModule, PEHeaderOffset);
			int secondsSince1970 = Marshal.ReadInt32(hModule, timestampOffset + LinkerTimestampOffset);

			// Convert the timestamp to a DateTime
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			DateTime linkTimeUtc = epoch.AddSeconds(secondsSince1970);
			DateTime result = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, timeZoneInfo ?? TimeZoneInfo.Local);

			return result;
		}
	}
}
