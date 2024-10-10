using System.IO;

namespace PenomyAPI.App.Common.FileServices.Models;

/// <summary>
///     The base class to store file information
///     that will be used to work with file service.
/// </summary>
public abstract class AppFileInfo
{
    /// <summary>
    ///     The id of the file that upload to the file service.
    /// </summary>
    public string FileId { get; set; }

    /// <summary>
    ///     The size of the file (in bytes).
    /// </summary>
    public long FileSize { get; set; }

    public string FileName { get; set; }

    /// <summary>
    ///     The path of the folder that will store this file.
    /// </summary>
    public string FolderPath { get; set; }

    public int UploadOrder { get; set; }

    /// <summary>
    ///     The url of the file that received from the storage service.
    /// </summary>
    public string StorageUrl { get; set; }

    /// <summary>
    ///     The stream contains the data of the file.
    /// </summary>
    public Stream FileDataStream { get; set; }

    /// <summary>
    ///     Reset the file data stream to read the bytes.
    /// </summary>
    public void ResetFileDataStream()
    {
        // Reset the position to 0 to read the data bytes.
        FileDataStream.Position = 0;
    }

    public void CleanFileDataStream()
    {
        try
        {
            FileDataStream.Flush();
            FileDataStream.Close();
            FileDataStream.Dispose();
        }
        catch { }
    }
}
