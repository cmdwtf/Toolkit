using System;

using SPath = System.IO.Path;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// A set of tools used to manipulate, calculate, and query things about path strings.
	/// </summary>
	public static class Path
	{
		// Path.GetRelativePath wasn't there til .NET Standard 2.1... (.NET 5/.NET Core 3.0)
#if !NETCOREAPP3_0_OR_GREATER
		/// <summary>
		/// Creates a relative path from a given working directory and absolute path.
		/// </summary>
		/// <param name="workingDirectory">The directory to assume is the base of the relative path.</param>
		/// <param name="fullPath">The absolute path to be relative to.</param>
		/// <param name="ignoreCase">
		/// If true, case will be ignored when building the path.
		/// Otherwise, files or folders of varying cases will be treated as different.
		/// Defaults to true on Windows platforms, otherwise false.
		/// </param>
		/// <returns>The relative path if one could be calculated, otherwise the full path.</returns>
		public static string GetRelativePath(string workingDirectory, string fullPath, bool? ignoreCase = null)
		{
			string result = string.Empty;
			int offset;

			if (ignoreCase.HasValue == false)
			{
				ignoreCase = Windows.IsWindows || MacOS.IsMacOS;
			}

			// this is the easy case.  The file is inside of the working directory.
			if (fullPath.StartsWith(workingDirectory, ignoreCase.Value, CultureInfo.CurrentCulture))
			{
				return fullPath.Substring(workingDirectory.Length + 1);
			}

			// the hard case has to back out of the working directory
			string[] baseDirs = workingDirectory.Split(new char[] { ':', '\\', '/' });
			string[] fileDirs = fullPath.Split(new char[] { ':', '\\', '/' });

			// if we failed to split (empty strings?) or the drive letter does not match
			if (baseDirs.Length <= 0 || fileDirs.Length <= 0 || baseDirs[0] != fileDirs[0])
			{
				// can't create a relative path between separate harddrives/partitions.
				return fullPath;
			}

			// skip all leading directories that don't match
			for (offset = 1; offset < baseDirs.Length; offset++)
			{
				if (string.Compare(baseDirs[offset], fileDirs[offset], ignoreCase.Value) != 0)
				{
					break;
				}
			}

			// back out of the working directory
			for (int i = 0; i < (baseDirs.Length - offset); i++)
			{
				result += "..\\";
			}

			// step into the file path
			for (int i = offset; i < fileDirs.Length - 1; i++)
			{
				result += fileDirs[i] + "\\";
			}

			// append the file
			result += fileDirs[^1];

			return result;
		}
#endif // !NETCOREAPP3_0_OR_GREATER

		/// <summary>
		/// Will determine if a path is a rooted, local path by attempting
		/// to create a Uri from it, and ensuring it is a loopback.
		/// </summary>
		/// <param name="path">The path to test.</param>
		/// <returns>true, if the path is a valid rooted local path, otherwise false.</returns>
		public static bool IsPathValidRootedLocal(string path)
		{
			bool isValidUri = Uri.TryCreate(path, UriKind.Absolute, out Uri pathUri);
			return isValidUri && pathUri != null && pathUri.IsLoopback;
		}

		/// <summary>
		/// Determines if a path is a full, absolute path by checking against a set of
		/// rules to weed out any non-full paths.
		/// </summary>
		/// <param name="path">The path to test.</param>
		/// <returns>True, if it is a full path.</returns>
		/// <remarks>Has specific checking to look for both Windows and Unix-like paths.</remarks>
		public static bool IsFullPath(string path)
		{
			// if the string is empty, has invalid characters, or isn't rooted, it's not a full path.
			if (string.IsNullOrWhiteSpace(path) || path.IndexOfAny(SPath.GetInvalidPathChars()) != -1 || !SPath.IsPathRooted(path))
			{
				return false;
			}

			// grab the root
			string pathRoot = SPath.GetPathRoot(path);

			// If the root is less than 2 characters, it can't be:
			// - A windows drive (e.g.: "C:\")
			// - A UNC path (e.g.: "\\name")
			// If it *is* one character, but that character is a forward slash,
			// it could be a Unix-like path.
			if (pathRoot.Length <= 2 && pathRoot.StartsWith("/") == false)
			{
				return false;
			}

			// A UNC path without a share name (e.g "\\server") is invalid, and not a full path.
			return !(pathRoot == path && pathRoot.StartsWith("\\\\") && pathRoot.IndexOf('\\', 2) == -1);
		}

		/// <summary>
		/// Gets an argument string to be passed to explorer.exe to highlight the given path.
		/// </summary>
		/// <param name="path">The file to select in explorer.exe</param>
		/// <returns>The generated argument string</returns>
		public static string GenerateExplorerSelectArgument(string path) => "/select, \"" + path + "\"";
	}
}
