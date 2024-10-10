using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatArt4.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatArt4;

public class Art4Handler : IFeatureHandler<Art4Request, Art4Response>
{
    private readonly IArt4Repository _art4Repository;
    private readonly Lazy<IFeatArt4FileUploadService> _fileUploadService;
    private readonly Lazy<ISnowflakeIdGenerator> _snowflakeIdGenerator;

    public Art4Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IFeatArt4FileUploadService> fileUploadService,
        Lazy<ISnowflakeIdGenerator> snowflakeIdGenerator
    )
    {
        _art4Repository = unitOfWork.Value.Art4Repository;
        _fileUploadService = fileUploadService;
        _snowflakeIdGenerator = snowflakeIdGenerator;
    }

    public async Task<Art4Response> ExecuteAsync(Art4Request request, CancellationToken ct)
    {
        var comicId = _snowflakeIdGenerator.Value.Get();

        var dateTimeUtcNow = DateTime.UtcNow;
        const int IntialOrder = 0; // Comic is created new so last chapter order is 0.

        var newComic = new Artwork
        {
            Id = request.ComicId,
            Title = request.Title,
            ArtworkType = ArtworkType.Comic,
            ArtworkOriginId = request.OriginId,
            Introduction = request.Introduction,
            AuthorName = request.AuthorName,
            ArtworkStatus = ArtworkStatus.OnGoing,
            PublicLevel = request.PublicLevel,
            AllowComment = request.AllowComment,
            CreatedAt = dateTimeUtcNow,
            CreatedBy = request.CreatedBy,
            UpdatedAt = dateTimeUtcNow,
            UpdatedBy = request.CreatedBy,
            HasSeries = false,
            IsTakenDown = false,
            OtherName = request.Title,
            LastChapterUploadOrder = IntialOrder,
            IsCreatedByAuthorizedUser = false,
            IsTemporarilyRemoved = false,
        };

        var comicCategories = request.ArtworkCategories;

        var result = await _art4Repository.CreateComicAsync(newComic, comicCategories, ct);

        // If result is false.
        if (!result)
        {
            return new Art4Response
            {
                IsSuccess = false,
                StatusCode = Art4ResponseStatusCode.DATABASE_ERROR
            };
        }

        return new Art4Response { IsSuccess = true, StatusCode = Art4ResponseStatusCode.SUCCESS };
    }
}
