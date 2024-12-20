using PenomyAPI.App.Common;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt15;

public class Art15Handler : IFeatureHandler<Art15Request, Art15Response>
{
    private const string AnimeFolder = "animations";
    private readonly IArt15Repository _art15Repository;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;

    public Art15Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService)
    {
        _art15Repository = unitOfWork.Value.Art15Repository;
        _fileService = fileService;
    }

    public async Task<Art15Response> ExecuteAsync(Art15Request request, CancellationToken ct)
    {
        // The name of the artwork folder will be similar to the id of that artwork.
        var artworkId = request.ArtworkId;

        var artworkFolderName = artworkId.ToString();

        // The info of folder to store the thumbnail of this anime.
        var folderInfo = new AppFolderInfo
        {
            RelativePath = DirectoryPathHelper.BuildPath(
                pathSeparator: DirectoryPathHelper.WebPathSeparator,
                rootDirectory: AnimeFolder,
                subFolders: artworkFolderName
            )
        };

        var fileService = _fileService.Value;

        // Create the folder with provided info.
        var folderCreateResult = await fileService.CreateFolderAsync(
            folderInfo: folderInfo,
            cancellationToken: ct
        );

        if (!folderCreateResult)
        {
            return Art15Response.FILE_SERVICE_ERROR;
        }

        // Upload the thumbnail of the anime.
        var thumnailFileInfo = request.ThumbnailFileInfo;
        thumnailFileInfo.FolderPath = folderInfo.RelativePath;

        var uploadFileResult = await fileService.UploadFileAsync(
            fileInfo: thumnailFileInfo,
            overwrite: true,
            cancellationToken: ct
        );

        // Get the storage url after upload the success.
        thumnailFileInfo.StorageUrl = uploadFileResult.Value.StorageUrl;

        // Last chapter order must be 0 because anime is created new.
        const int IntialOrder = 0;
        var dateTimeUtcNow = DateTime.UtcNow;
        var dateTimeMinUtc = CommonValues.DateTimes.MinUtc;

        var newAnime = new Artwork
        {
            Id = artworkId,
            Title = request.Title,
            ArtworkType = ArtworkType.Animation,
            ArtworkOriginId = request.OriginId,
            ThumbnailUrl = thumnailFileInfo.StorageUrl,
            Introduction = request.Introduction,
            AuthorName = request.AuthorName,
            ArtworkStatus = ArtworkStatus.OnGoing,
            PublicLevel = request.PublicLevel,
            AllowComment = request.AllowComment,
            CreatedAt = dateTimeUtcNow,
            CreatedBy = request.CreatedBy,
            UpdatedAt = dateTimeUtcNow,
            UpdatedBy = request.CreatedBy,
            TemporarilyRemovedBy = request.CreatedBy,
            TemporarilyRemovedAt = dateTimeMinUtc,
            HasSeries = false,
            IsTakenDown = false,
            LastChapterUploadOrder = IntialOrder,
            IsCreatedByAuthorizedUser = false,
            IsTemporarilyRemoved = false,
        };

        var metaData = ArtworkMetaData.Empty(artworkId);

        var comicCategories = request.ArtworkCategories;

        var result = await _art15Repository.CreateAnimeAsync(
            anime: newAnime,
            metaData: metaData,
            artworkCategories: comicCategories,
            cancellationToken: ct
        );

        // If result is false.
        if (!result)
        {
            return Art15Response.DATABASE_ERROR;
        }

        return Art15Response.SUCCESS(artworkId);
    }
}
