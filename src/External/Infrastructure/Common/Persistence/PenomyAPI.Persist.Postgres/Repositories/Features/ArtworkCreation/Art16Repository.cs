using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal class Art16Repository : IArt16Repository
{
    private readonly DbContext _dbContext;

    public Art16Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Artwork> GetAnimeDetailByIdAsync(
        long artworkId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<Artwork>()
            .AsNoTracking()
            .Where(anime => anime.Id == artworkId)
            .Select(anime => new Artwork
            {
                Id = anime.Id,
                Title = anime.Title,
                ThumbnailUrl = anime.ThumbnailUrl,
                Introduction = anime.Introduction,
                Origin = new ArtworkOrigin
                {
                    CountryName = anime.Origin.CountryName
                },
                ArtworkStatus = anime.ArtworkStatus,
                Creator = new UserProfile
                {
                    NickName = anime.Creator.NickName,
                },
                HasSeries = anime.HasSeries,
                ArtworkCategories = anime.ArtworkCategories.Select(artworkCategory => new ArtworkCategory
                {
                    Category = new Category
                    {
                        Id = artworkCategory.CategoryId,
                        Name = artworkCategory.Category.Name,
                    }
                })
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<List<ArtworkChapter>> GetChaptersByPublishStatusAsync(
        long animeId,
        PublishStatus publishStatus,
        CancellationToken cancellationToken)
    {
        Expression<Func<ArtworkChapter, bool>> filterExpression =
            (ArtworkChapter chapter) => chapter.ArtworkId == animeId
            && !chapter.IsTemporarilyRemoved
            && chapter.PublishStatus == PublishStatus.Drafted;

        // If publish status is not drafted, then get both published and scheduled.
        if (publishStatus != PublishStatus.Drafted)
        {
            filterExpression =
                (ArtworkChapter chapter) => chapter.ArtworkId == animeId
                && !chapter.IsTemporarilyRemoved
                && chapter.PublishStatus != PublishStatus.Drafted;
        }

        return _dbContext.Set<ArtworkChapter>()
            .AsNoTracking()
            .Where(filterExpression)
            .Select(chapter => new ArtworkChapter
            {
                Id = chapter.Id,
                Title = chapter.Title,
                UploadOrder = chapter.UploadOrder,
                PublishStatus = chapter.PublishStatus,
                ThumbnailUrl = chapter.ThumbnailUrl,
                AllowComment = chapter.AllowComment,
                CreatedAt = chapter.CreatedAt,
                PublishedAt = chapter.PublishedAt,
                PublicLevel = chapter.PublicLevel,
            })
            .OrderBy(chapter => chapter.UploadOrder)
            .ToListAsync(cancellationToken);
    }
}
