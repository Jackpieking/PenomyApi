using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt10.Infrastructures;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.Infra.FeatArt10;

public class Art10FileService : IArt10FileService
{
    private readonly CloudinaryOptions _options;

    public Art10FileService(CloudinaryOptions options)
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
                rootDirectory: _options.ComicRootFolder,
                childFolders: folderInfo.RelativePath
            );

            var result = await cloudinary.CreateFolderAsync(folderInfo.AbsolutePath);

            return result.Success;
        }
        catch
        {
            return false;
        }
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
        var cloudinary = new Cloudinary(cloudinaryUrl: _options.CloudinaryUrl);

        cloudinary.Api.Secure = true;

        return cloudinary;
    }
}
