using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Infra.Configuration.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.FileService.CloudinaryService;

public sealed class CloudinaryFileService : IDefaultDistributedFileService
{
    private static Cloudinary _cloudinaryInstance;
    private static readonly object _lock = new object();
    private readonly CloudinaryOptions _options;

    public CloudinaryFileService(CloudinaryOptions options)
    {
        _options = options;
    }

    public async Task<bool> CreateFolderAsync(
        AppFolderInfo folderInfo,
        CancellationToken cancellationToken
    )
    {
        var cloudinary = CreateCloudinary();

        try
        {
            var folderPath = folderInfo.RelativePath ?? folderInfo.AbsolutePath;

            var result = await cloudinary.CreateFolderAsync(folderPath);

            return result.Success;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Result<AppFileInfo>> UploadFileAsync(
        AppFileInfo fileInfo,
        bool overwrite,
        CancellationToken cancellationToken
    )
    {
        var cloudinary = CreateCloudinary();

        try
        {
            fileInfo.ResetFileDataStream();

            // If the overwrite is set true, then invalidate is set (true) too.
            var imageUploadParams = new ImageUploadParams
            {
                Folder = fileInfo.FolderPath,
                AssetFolder = fileInfo.FolderPath,
                PublicId = fileInfo.FileId,
                File = new FileDescription(
                    name: fileInfo.FileName,
                    stream: fileInfo.FileDataStream
                ),
                DisplayName = fileInfo.FileName,
                Overwrite = overwrite,
                Invalidate = overwrite,
            };

            // Get the uploading result.
            var result = await cloudinary.UploadAsync(
                parameters: imageUploadParams,
                cancellationToken: cancellationToken
            );

            fileInfo.CleanFileDataStream();

            fileInfo.StorageUrl = result.SecureUrl.OriginalString;

            return Result<AppFileInfo>.Success(fileInfo);
        }
        catch
        {
            return Result<AppFileInfo>.Failed();
        }
    }

    public Task<bool> DeleteFileAsync(AppFileInfo fileInfo, CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }

    public Task<bool> DeleteFileByIdAsync(string fileId, CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }

    public async Task<Result<IEnumerable<AppFileInfo>>> UploadMultipleFilesAsync(
        IEnumerable<AppFileInfo> fileInfos,
        bool overwrite,
        CancellationToken cancellationToken
    )
    {
        var cloudinary = CreateCloudinary();

        try
        {
            Dictionary<AppFileInfo, Task<ImageUploadResult>> imageUploadTasks = new();

            foreach (var fileInfo in fileInfos)
            {
                var uploadTask = InternalHandleFileUploadAsync(
                    cloudinary,
                    fileInfo,
                    overwrite,
                    cancellationToken);

                imageUploadTasks.Add(fileInfo, uploadTask);
            }

            await Task.WhenAll(imageUploadTasks.Values);

            return Result<IEnumerable<AppFileInfo>>.Success(imageUploadTasks.Keys);
        }
        catch
        {
            return Result<IEnumerable<AppFileInfo>>.Failed();
        }
    }

    #region Private Methods
    private async Task<ImageUploadResult> InternalHandleFileUploadAsync(
        Cloudinary cloudinary,
        AppFileInfo fileInfo,
        bool overwrite,
        CancellationToken cancellationToken)
    {
        fileInfo.ResetFileDataStream();

        // If the overwrite is set true, then invalidate is set (true) too.
        var imageUploadParams = new ImageUploadParams
        {
            Folder = fileInfo.FolderPath,
            AssetFolder = fileInfo.FolderPath,
            PublicId = fileInfo.FileId,
            File = new FileDescription(
            name: fileInfo.FileName,
                stream: fileInfo.FileDataStream
            ),
            DisplayName = fileInfo.FileName,
            Overwrite = overwrite,
            Invalidate = overwrite,
        };

        // Get the uploading result.
        var uploadResult = await cloudinary.UploadAsync(
            parameters: imageUploadParams,
            cancellationToken: cancellationToken
        );

        fileInfo.CleanFileDataStream();

        fileInfo.StorageUrl = uploadResult.SecureUrl.OriginalString;

        return uploadResult;
    }

    /// <summary>
    ///     Create an instance of cloudinary to make api calls.
    /// </summary>
    private Cloudinary CreateCloudinary()
    {
        lock (_lock)
        {
            if (Equals(_cloudinaryInstance, null))
            {
                _cloudinaryInstance = new Cloudinary(cloudinaryUrl: _options.CloudinaryUrl);
                _cloudinaryInstance.Api.Secure = true;
            }
        }

        return _cloudinaryInstance;
    }
    #endregion
}
