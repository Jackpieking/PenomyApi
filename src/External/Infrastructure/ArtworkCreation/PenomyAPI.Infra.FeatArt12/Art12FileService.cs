using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt12.Infrastructures;
using PenomyAPI.Infra.Configuration.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Infra.FeatArt12;

public sealed class Art12FileService : IArt12FileService
{
    private readonly CloudinaryOptions _options;

    public Art12FileService(CloudinaryOptions options)
    {
        _options = options;
    }

    public Task<bool> CreateFolderAsync(
        AppFolderInfo folderInfo,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(false);
    }

    public Task<Result<AppFileInfo>> UploadFileAsync(
        AppFileInfo fileInfo,
        bool overwrite,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(Result<AppFileInfo>.Failed());
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

            var folderPath = DirectoryPathHelper.BuildPath(
                pathSeparator: DirectoryPathHelper.WebPathSeparator,
                rootDirectory: _options.ComicRootFolder,
                subFolders: fileInfos.First().FolderPath);

            foreach (var fileInfo in fileInfos)
            {
                var uploadTask = InternalHandleFileUploadAsync(
                    cloudinary,
                    fileInfo,
                    folderPath,
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

    private async Task<ImageUploadResult> InternalHandleFileUploadAsync(
        Cloudinary cloudinary,
        AppFileInfo fileInfo,
        string folderPath,
        bool overwrite,
        CancellationToken cancellationToken)
    {
        fileInfo.ResetFileDataStream();

        // If the overwrite is set true, then invalidate is set (true) too.
        var imageUploadParams = new ImageUploadParams
        {
            Folder = folderPath,
            AssetFolder = folderPath,
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

    public async Task<bool> DeleteMultipleFileByIdsAsync(
        string chapterFolderRelativePath,
        IEnumerable<long> fileIds,
        CancellationToken cancellationToken)
    {
        var cloudinary = CreateCloudinary();

        try
        {
            chapterFolderRelativePath = DirectoryPathHelper.BuildPath(
                pathSeparator: DirectoryPathHelper.WebPathSeparator,
                rootDirectory: _options.ComicRootFolder,
                subFolders: chapterFolderRelativePath);

            var deletedResult = await cloudinary.DeleteResourcesAsync(
                ResourceType.Image,
                fileIds.Select(
                    id => chapterFolderRelativePath + DirectoryPathHelper.WebPathSeparator + id).ToArray());

            if (deletedResult.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
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
