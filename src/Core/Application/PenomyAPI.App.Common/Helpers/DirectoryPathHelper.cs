using System.Runtime.CompilerServices;

namespace PenomyAPI.App.Common.Helpers;

public static class DirectoryPathHelper
{
    public const string WebPathSeparator = "/";
    public const string UnixDirectoryPathSeparator = "/";
    public const string WindowDirectoryPathSeparator = "\\";

    public static string BuildPath(
        string pathSeparator,
        string rootDirectory,
        params string[] childFolders
    )
    {
        const int includingRootDirectory = 1;

        var handler = new DefaultInterpolatedStringHandler(
            literalLength: childFolders.Length,
            formattedCount: includingRootDirectory + childFolders.Length
        );

        handler.AppendFormatted(rootDirectory);

        if (childFolders.Length > 0)
        {
            foreach (var childFolder in childFolders)
            {
                handler.AppendLiteral(pathSeparator);
                handler.AppendFormatted(childFolder);
            }
        }

        return handler.ToStringAndClear();
    }
}
