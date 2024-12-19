using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt20.Infrastructures;

public interface IArt20FileService
{
    /// <summary>
    ///     Create a folder on the storage with input <paramref name="folderInfo"/>.
    /// </summary>
    /// <remarks>
    ///     If you don't specify the relative path or absolute path in folder info,
    ///     this method will create the folder at the root path.
    /// </remarks>
    /// <param name="folderInfo">
    ///     Information about the folder to create.
    /// </param>
    /// <returns>
    ///     The result (<see langword="bool"/>) of folder creation.
    /// </returns>
    Task<bool> CreateFolderAsync(AppFolderInfo folderInfo, CancellationToken cancellationToken);

    /// <summary>
    ///     Upload a file on the storage with input <paramref name="fileInfo"/>.
    ///     The <paramref name="overwrite"/> is specified to overwrite
    ///     the file content if the file with same name is already existed.
    /// </summary>
    /// <param name="fileInfo">
    ///     The information of the file to upload.
    /// </param>
    /// <param name="overwrite">
    ///     Indicate to overwrite if the file is already existed. Default is (false).
    /// </param>
    /// <returns>
    ///     The <see cref="AppFileInfo"/> contain the information
    ///     of the file after creating by the service.
    /// </returns>
    Task<Result<AppFileInfo>> UploadImageFileAsync(
        ImageFileInfo fileInfo,
        bool overwrite,
        CancellationToken cancellationToken
    );

    /// <summary>
    ///     Upload a file on the storage with input <paramref name="fileInfo"/>.
    ///     The <paramref name="overwrite"/> is specified to overwrite
    ///     the file content if the file with same name is already existed.
    /// </summary>
    /// <param name="fileInfo">
    ///     The information of the file to upload.
    /// </param>
    /// <param name="overwrite">
    ///     Indicate to overwrite if the file is already existed. Default is (false).
    /// </param>
    /// <returns>
    ///     The <see cref="AppFileInfo"/> contain the information
    ///     of the file after creating by the service.
    /// </returns>
    Task<Result<AppFileInfo>> UploadVideoFileAsync(
        VideoFileInfo fileInfo,
        bool overwrite,
        CancellationToken cancellationToken
    );
}
