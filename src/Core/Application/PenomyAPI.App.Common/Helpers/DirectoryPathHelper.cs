using System.Runtime.CompilerServices;

namespace PenomyAPI.App.Common.Helpers;

public static class DirectoryPathHelper
{
    public const string WebPathSeparator = "/";
    //public const string UnixDirectoryPathSeparator = "/";
    //public const string WindowDirectoryPathSeparator = "\\";

    /// <summary>
    ///     Build the storage path with specified <paramref name="pathSeparator"/>
    ///     and related information including <paramref name="rootDirectory"/> and <paramref name="subFolders"/>
    /// </summary>
    /// <remarks>
    ///     If you want to build a path like: /groups/group_id, then:
    ///     <br/>
    ///     + The root directory will be: groups
    ///     <br/>
    ///     + The sub folder/directory will be: group_id
    ///     <br/>
    ///     + The pathSeparator will be: <see cref="WebPathSeparator"/> (/)
    /// </remarks>
    /// <param name="pathSeparator">
    ///     The path separator that will be used to separate between directory.
    /// </param>
    /// <param name="rootDirectory">
    ///     The root directory of the path.
    /// </param>
    /// <param name="subFolders">
    ///     The sub directories/folders that included in the path (after the root directory).
    /// </param>
    /// <returns></returns>
    public static string BuildPath(
        string pathSeparator,
        string rootDirectory,
        params string[] subFolders
    )
    {
        const int includingRootDirectory = 1;

        var handler = new DefaultInterpolatedStringHandler(
            literalLength: subFolders.Length,
            formattedCount: includingRootDirectory + subFolders.Length
        );

        handler.AppendFormatted(rootDirectory);

        if (subFolders.Length > 0)
        {
            foreach (var childFolder in subFolders)
            {
                handler.AppendLiteral(pathSeparator);
                handler.AppendFormatted(childFolder);
            }
        }

        return handler.ToStringAndClear();
    }
}
