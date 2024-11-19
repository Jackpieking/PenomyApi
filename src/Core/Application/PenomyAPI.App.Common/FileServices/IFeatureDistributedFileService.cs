using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;

namespace PenomyAPI.App.Common.FileServices;

/// <summary>
///     The base generic interface to implement the
///     basic features of any distributed file service.
/// </summary>
/// <typeparam name="TFeatureHandler">
///     The feature handler that will implement this interface logic.
///     <br/> Implementation version with type <see cref="IFeatureHandler{TRequest, TResponse}"/>
///     will be used as a default file distributed service.
/// </typeparam>
/// <typeparam name="TRequest">
///     The request type that corresponded to the feature handler.
/// </typeparam>
/// <typeparam name="TResponse">
///     The response type that corresponded to the feature handler.
/// </typeparam>
public interface IFeatureDistributedFileService<TFeatureHandler, TRequest, TResponse>
    where TFeatureHandler : IFeatureHandler<TRequest, TResponse>
    where TRequest : IFeatureRequest<TResponse>
    where TResponse : IFeatureResponse
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
    Task<Result<AppFileInfo>> UploadFileAsync(
        AppFileInfo fileInfo,
        bool overwrite,
        CancellationToken cancellationToken
    );

    Task<Result<IEnumerable<AppFileInfo>>> UploadMultipleFilesAsync(
        IEnumerable<AppFileInfo> fileInfos,
        bool overwrite,
        CancellationToken cancellationToken
    );

    /// <summary>
    ///     Delete the specified file with input <paramref name="fileInfo"/>
    /// </summary>
    /// <param name="fileInfo">
    ///     The information of the file to be deleted.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The result (bool) of the deletion.
    /// </returns>
    Task<bool> DeleteFileAsync(AppFileInfo fileInfo, CancellationToken cancellationToken);

    /// <summary>
    ///     Delete the specified file with input <paramref name="fileId"/>
    /// </summary>
    /// <param name="fileId">
    ///     The id of the file to be deleted.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The result (bool) of the deletion.
    /// </returns>
    Task<bool> DeleteFileByIdAsync(string fileId, CancellationToken cancellationToken);
}
