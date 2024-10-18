using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.FileService.CloudinaryService;

public sealed class CloudinaryFileService : IDefaultDistributedFileService
{
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
            var result = await cloudinary.CreateFolderAsync(folderInfo.RelativePath);

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
        throw new System.NotImplementedException();
    }

    public Task<bool> DeleteFileByIdAsync(string fileId, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task<Result<IEnumerable<AppFileInfo>>> UploadMultipleFilesAsync(
        IEnumerable<AppFileInfo> fileInfos,
        bool overwrite,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    ///     Create an instance of cloudinary to make api calls.
    /// </summary>
    private Cloudinary CreateCloudinary()
    {
        var cloudinary = new Cloudinary(cloudinaryUrl: _options.CloudinaryUrl);

        cloudinary.Api.Secure = true;

        return cloudinary;
    }
}
