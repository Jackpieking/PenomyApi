namespace PenomyAPI.App.Common.FileServices.Models;

public sealed class AppFolderInfo
{
    public string FolderName { get; set; }

    /// <summary>
    ///     The relative path of this folder on the storage.
    /// </summary>
    public string RelativePath { get; set; }

    /// <summary>
    ///     The real physical path of this folder on the storage. 
    /// </summary>
    public string AbsolutePath { get; set; }
}
