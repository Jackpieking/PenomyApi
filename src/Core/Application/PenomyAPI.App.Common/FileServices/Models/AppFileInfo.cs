using System.IO;

namespace PenomyAPI.App.Common.FileServices.Models;

/// <summary>
///     The base class to store file information
///     that will be used to work with file service.
/// </summary>
/// <remarks>
///     The implementation of this class are: <see cref="ImageFileInfo"/>.
/// </remarks>
public abstract class AppFileInfo
{
    /// <summary>
    ///     The id of the file that upload to the file service.
    /// </summary>
    /// <remarks>
    ///     This field should be specified explicitly to manage the id
    ///     of the file instead of randomly init by the implementation service.
    /// </remarks>
    public string FileId { get; set; }

    /// <summary>
    ///     The size of the file (in bytes).
    /// </summary>
    public long FileSize { get; set; }

    public string FileName { get; set; }

    /// <summary>
    ///     (Optional) The extension of the file you upload.
    /// </summary>
    public string FileExtension { get; set; }

    /// <summary>
    ///     The path of the folder that will store this file.
    ///     Example: If the group want to store its background image to folder: /groups/group_id,
    ///     then it must specify the FolderPath = /groups/group_id.
    /// </summary>
    public string FolderPath { get; set; }

    /// <summary>
    ///     Upload order of the image file to the storage with base zero.
    /// </summary>
    public int UploadOrder { get; set; }

    /// <summary>
    ///     (No need to init) The url of the file that located on the storage service.
    /// </summary>
    /// <remarks>
    ///     This storage URL will be set when the storage service upload success
    ///     and then be used for database persistence.
    /// </remarks>
    public string StorageUrl { get; set; }

    /// <summary>
    ///     The stream contains the data of the file. This field must
    ///     be specified to include the data of the file to upload.
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
