using System;
using System.IO;
using System.Runtime.InteropServices;

using static cmdwtf.Toolkit.WinForms.Native.ComCtl32;
using static cmdwtf.Toolkit.WinForms.Native.Shell32;
using static cmdwtf.Toolkit.WinForms.Native.User32;

using SDIcon = System.Drawing.Icon;

namespace cmdwtf.Toolkit.WinForms.Extensions
{
	/// <summary>
	/// Extensions methods for <see cref="SDIcon"/>.
	/// </summary>
	public static class IconExtensions
	{
		/// <summary>
		/// An enumeration representing the states of a folder icon.
		/// </summary>
		public enum FolderType
		{
			/// <summary>
			/// The closed folder type.
			/// </summary>
			Closed,
			/// <summary>
			/// The open folder type.
			/// </summary>
			Open,
		}

		/// <summary>
		/// An enumeration representing the standard sizes of a Win32 icon.
		/// </summary>
		public enum Size
		{
			/// <summary>
			/// Large is a 32x32 icon.
			/// </summary>
			Large,
			/// <summary>
			/// Small is a 16x16 icon.
			/// </summary>
			Small
		}

		/// <summary>
		/// Gets a icon for the given path, for the given size.
		/// </summary>
		/// <param name="filePath">The file to get the icon for.</param>
		/// <param name="size">The size of icon to get. (Defaults to Large.)</param>
		/// <returns>The file's icon.</returns>
		public static SDIcon GetIcon(string filePath, Size size = Size.Large)
			=> size == Size.Large ? GetLargeIcon(filePath) : GetSmallIcon(filePath);

		/// <summary>
		/// Gets a small icon for the given path.
		/// </summary>
		/// <param name="filePath">The file to get the icon for.</param>
		/// <returns>The file's icon.</returns>
		public static SDIcon GetSmallIcon(string filePath)
			=> GetIcon(filePath, SHGFI_ICON | SHGFI_SMALLICON);

		/// <summary>
		/// Gets a large icon for the given path.
		/// </summary>
		/// <param name="filePath">The file to get the icon for.</param>
		/// <returns>The file's icon.</returns>
		public static SDIcon GetLargeIcon(string filePath)
			=> GetIcon(filePath, SHGFI_ICON | SHGFI_LARGEICON);

		/// <summary>
		/// Gets a shell sized icon for the given path, for the given size.
		/// </summary>
		/// <param name="filePath">The file to get the icon for.</param>
		/// <param name="size">The size of icon to get. (Defaults to Large.)</param>
		/// <returns>The file's icon.</returns>
		public static SDIcon GetShellIcon(string filePath, Size size = Size.Large)
			=> size == Size.Large ? GetShellLargeIcon(filePath) : GetShellSmallIcon(filePath);

		/// <summary>
		/// Gets a small shell sized icon for the given path.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns>The file's icon.</returns>
		/// <remarks>
		/// I'm not sure of the difference between this and the non-shell variant.
		/// The documentation doesn't explain much:
		/// https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shgetfileinfoa#shgfi_shelliconsize-0x000000004
		/// I'm really not sure what "shell sized" even means.
		/// </remarks>
		public static SDIcon GetShellSmallIcon(string filePath)
			=> GetIcon(filePath, SHGFI_ICON | SHGFI_SHELLICONSIZE | SHGFI_SMALLICON);

		/// <summary>
		/// Gets a large shell sized icon for the given path.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns>The file's icon.</returns>
		/// <remarks>
		/// I'm not sure of the difference between this and the non-shell variant.
		/// The documentation doesn't explain much:
		/// https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shgetfileinfoa#shgfi_shelliconsize-0x000000004
		/// I'm really not sure what "shell sized" even means.
		/// </remarks>
		public static SDIcon GetShellLargeIcon(string filePath)
			=> GetIcon(filePath, SHGFI_ICON | SHGFI_SHELLICONSIZE | SHGFI_LARGEICON);

		/// <summary>
		/// Gets an icon in the specified size representing a folder of the specified type.
		/// </summary>
		/// <param name="size">The size of icon to get.</param>
		/// <param name="folderType">The type of folder icon to get.</param>
		/// <returns>The requested folder icon.</returns>
		public static SDIcon GetFolderIcon(Size size, FolderType folderType)
		{
			// Need to add size check, although errors generated at present!
			uint flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES |
				(size == Size.Small ? SHGFI_SMALLICON : SHGFI_LARGEICON) |
				(folderType == FolderType.Open ? SHGFI_OPENICON : 0);

			return GetIcon(Directory.GetCurrentDirectory(), flags, FileAttributes.Directory);
		}

		/// <summary>
		/// The meat and potatoes of the get icon functions above. Gets an icon based on the
		/// given path and flags. Will attempt to fetch icons from Image Lists if they use
		/// those (eg: .lnk/.url files).
		/// </summary>
		/// <param name="path">The path to get the icon for.</param>
		/// <param name="flags">A bitfield representing flags to pass to
		/// <see cref="Trampolines.ShellGetFileInfo(string, FileAttributes, uint)"/></param>
		/// <param name="attribs">FileAttributes of the given path.</param>
		/// <returns>The requested icon.</returns>
		private static SDIcon GetIcon(string path, uint flags, FileAttributes attribs = 0)
		{
			(SHFILEINFO info, IntPtr list) = Trampolines.ShellGetFileInfo(path, attribs, flags);

			SDIcon icon = null;

			if (info.hIcon != IntPtr.Zero)
			{
				icon = SDIcon.FromHandle(info.hIcon).Clone() as SDIcon;
				DestroyIcon(info.hIcon);
				return icon;
			}
			else if (list != IntPtr.Zero && info.iIcon != IntPtr.Zero)
			{
				icon = Trampolines.GetIconFromList(list, info.iIcon.ToInt32());
			}

			return icon;
		}

		/// <summary>
		/// Small functions to jump into the native ones a bit easier.
		/// </summary>
		private static class Trampolines
		{
			public static (SHFILEINFO finfo, IntPtr imageList) ShellGetFileInfo(string path, FileAttributes attributes, uint flags)
			{
				var info = new SHFILEINFO();
				IntPtr list = SHGetFileInfo(path, (uint)attributes, ref info, (uint)Marshal.SizeOf(info), flags);
				return (info, list);
			}

			public static SDIcon GetIconFromList(IntPtr iconList, int index, int flags = ILD_TRANSPARENT)
			{
				IntPtr hIcon = ImageList_GetIcon(iconList, index, flags);

				if (hIcon != IntPtr.Zero)
				{
					var managedIcon = SDIcon.FromHandle(hIcon).Clone() as SDIcon;
					DestroyIcon(hIcon);
					return managedIcon;
				}

				return null;
			}
		}

	}
}
