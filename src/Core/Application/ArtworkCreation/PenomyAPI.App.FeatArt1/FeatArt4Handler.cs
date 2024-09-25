using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatArt4.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatArt4;

public class FeatArt4Handler : IFeatureHandler<FeatArt4Request, FeatArt4Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IFeatArt4FileUploadService> _fileUploadService;

    public FeatArt4Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IFeatArt4FileUploadService> fileUploadService)
    {
        _unitOfWork = unitOfWork;
        _fileUploadService = fileUploadService;
    }

    public async Task<FeatArt4Response> ExecuteAsync(FeatArt4Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value; // Load from the Lazy.
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

        var result = await unitOfWork.FeatArt4Repository.CreateComicAsync(newComic, comicCategories, ct);

        // If result is false.
        if (!result)
        {
            return new FeatArt4Response
            {
                IsSuccess = false,
                StatusCode = FeatArt4ResponseStatusCode.DATABASE_ERROR
            };
        }

        return new FeatArt4Response
        {
            IsSuccess = true,
            StatusCode = FeatArt4ResponseStatusCode.SUCCESS
        };
    }
}
