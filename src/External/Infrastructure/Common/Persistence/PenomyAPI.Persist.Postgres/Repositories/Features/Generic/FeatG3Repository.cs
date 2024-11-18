using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class FeatG3Repository : IFeatG3Repository
{
    private const int NUMBER_OF_ARTWORKS_TO_RETURN = 16;
    private const int NUMBER_OF_RECOMMENDED_CHAPTERS_PER_ARTWORK = 2;

    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkMetaData> _statisticDbSet;

    public FeatG3Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _statisticDbSet = dbContext.Set<ArtworkMetaData>();
    }

    public async Task<List<Artwork>> GetRecentlyUpdatedComicsAsync()
    {
        var result = await _artworkDbSet
            .Where(a =>
                a.ArtworkType == ArtworkType.Comic
                && a.IsTemporarilyRemoved == false
                && a.PublicLevel == ArtworkPublicLevel.Everyone
                && a.IsTakenDown == false
            )
            .Select(a => new Artwork()
            {
                Id = a.Id,
                Title = a.Title,
                ThumbnailUrl = a.ThumbnailUrl,
                UpdatedAt = a.UpdatedAt,
                Creator = new UserProfile
                {
                    UserId = a.Creator.UserId,
                    AvatarUrl = a.Creator.AvatarUrl,
                    NickName = a.Creator.NickName,
                },
                Origin = new ArtworkOrigin
                {
                    ImageUrl = a.Origin.ImageUrl
                },
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalStarRates = a.ArtworkMetaData.TotalStarRates,
                    TotalUsersRated = a.ArtworkMetaData.TotalUsersRated,
                },
                Chapters = a.Chapters
                    .Where(chapter
                        => !chapter.IsTemporarilyRemoved
                        && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                        && chapter.PublishStatus == PublishStatus.Published)
                    .Select(chapter => new ArtworkChapter
                    {
                        Id = chapter.Id,
                        UploadOrder = chapter.UploadOrder,
                        PublishedAt = chapter.PublishedAt,
                    })
                    .OrderByDescending(c => c.UploadOrder)
                    .Take(NUMBER_OF_RECOMMENDED_CHAPTERS_PER_ARTWORK),
            })
            .OrderByDescending(a => a.UpdatedAt)
            .Take(NUMBER_OF_ARTWORKS_TO_RETURN)
            .AsNoTracking()
            .ToListAsync();

        return result;
    }
}
