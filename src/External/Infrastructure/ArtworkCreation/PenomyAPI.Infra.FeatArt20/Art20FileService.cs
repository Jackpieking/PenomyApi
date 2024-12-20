using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt20.Infrastructures;
using PenomyAPI.App.FeatArt22.Infrastructures;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.Infra.FeatArt20;

public class Art20FileService : IArt20FileService, IArt22FileService
{
    private static Cloudinary _cloudinaryInstance;
    private static readonly object _lock = new object();
    private readonly CloudinaryOptions _options;

    public Art20FileService(CloudinaryOptions options)
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
            folderInfo.AbsolutePath = DirectoryPathHelper.BuildPath(
                pathSeparator: DirectoryPathHelper.WebPathSeparator,
                rootDirectory: _options.AnimationRootFolder,
                subFolders: folderInfo.RelativePath
            );

            var result = await cloudinary.CreateFolderAsync(folderInfo.AbsolutePath);

            return result.Success;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Result<AppFileInfo>> UploadImageFileAsync(
        ImageFileInfo fileInfo,
        bool overwrite,
        CancellationToken cancellationToken
    )
    {
        var cloudinary = CreateCloudinary();

        try
        {
            fileInfo.ResetFileDataStream();

            // If the overwrite is set true, then invalidate is set (true).
            var imageUploadParams = new ImageUploadParams
            {
                PublicId = fileInfo.FileId,
                Folder = fileInfo.FolderPath,
                AssetFolder = fileInfo.FolderPath,
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

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return Result<AppFileInfo>.Failed();
            }

            fileInfo.CleanFileDataStream();

            fileInfo.StorageUrl = result.SecureUrl.OriginalString;

            return Result<AppFileInfo>.Success(fileInfo);
        }
        catch
        {
            return Result<AppFileInfo>.Failed();
        }
    }

    public async Task<Result<AppFileInfo>> UploadVideoFileAsync(
        VideoFileInfo fileInfo,
        bool overwrite,
        CancellationToken cancellationToken
    )
    {
        var videoUploadParams = new VideoUploadParams
        {
            PublicId = fileInfo.FileId,
            Folder = fileInfo.FolderPath,
            AssetFolder = fileInfo.FolderPath,
            File = new FileDescription(name: fileInfo.FileName, stream: fileInfo.FileDataStream),
            DisplayName = fileInfo.FileName,
            Overwrite = overwrite,
            Invalidate = overwrite,
        };

        // Based on the file size to upload different.
        const long NORMAL_FILE_SIZE = 50 * 1024 * 1024;

        var isNormalFileSize = fileInfo.FileSize <= NORMAL_FILE_SIZE;
        var cloudinary = CreateCloudinary();
        VideoUploadResult uploadResult;

        try
        {
            if (isNormalFileSize)
            {
                uploadResult = await cloudinary.UploadAsync(videoUploadParams, cancellationToken);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return Result<AppFileInfo>.Failed();
                }
            }
            else
            {
                // const int BUFFER_SIZE = 5 * 1024 * 1024;

                uploadResult = cloudinary.UploadLarge<VideoUploadResult>(
                    parameters: videoUploadParams
                );

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return Result<AppFileInfo>.Failed();
                }
            }

            fileInfo.CleanFileDataStream();

            fileInfo.StorageUrl = uploadResult.SecureUrl.OriginalString;

            return Result<AppFileInfo>.Success(fileInfo);
        }
        catch
        {
            return Result<AppFileInfo>.Failed();
        }
    }

    #region Private Methods
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
