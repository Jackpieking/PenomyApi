using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal sealed class Art10Repository : IArt10Repository
{
    private readonly DbContext _dbContext;
    private DbSet<ArtworkChapter> _chapterDbSet;
    private DbSet<ArtworkChapterMetaData> _chapterMetaDataDbSet;
    private DbSet<ArtworkChapterMedia> _chapterMediaDbSet;

    public Art10Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Artwork> GetDetailToCreateChapterByComicIdAsync(
        long comicId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<Artwork>()
            .AsNoTracking()
            .Where(comic => comic.Id == comicId)
            .Select(comic => new Artwork
            {
                Title = comic.Title,
                LastChapterUploadOrder = comic.LastChapterUploadOrder,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> CreateComicChapterAsync(
        ArtworkChapter comicChapter,
        IEnumerable<ArtworkChapterMedia> chapterMedias,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalCreateComicChapterAsync(
                    comicChapter,
                    chapterMedias,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalCreateComicChapterAsync(
        ArtworkChapter comicChapter,
        IEnumerable<ArtworkChapterMedia> chapterMedias,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            _chapterDbSet = _dbContext.Set<ArtworkChapter>();
            _chapterMetaDataDbSet = _dbContext.Set<ArtworkChapterMetaData>();
            _chapterMediaDbSet = _dbContext.Set<ArtworkChapterMedia>();

            await _chapterDbSet.AddAsync(comicChapter, cancellationToken);

            var metadata = new ArtworkChapterMetaData
            {
                ChapterId = comicChapter.Id
            };

            await _chapterMetaDataDbSet.AddAsync(metadata, cancellationToken);

            await _chapterMediaDbSet.AddRangeAsync(chapterMedias, cancellationToken);

            // If comic chapter is not upload with drafted mode,
            // then update the upload order of the corresponding comic.
            if (comicChapter.PublishStatus != PublishStatus.Drafted)
            {
                var artworkDbSet = _dbContext.Set<Artwork>();

                await artworkDbSet
                    .Where(artwork => artwork.Id == comicChapter.ArtworkId)
                    .ExecuteUpdateAsync(
                        artwork => artwork.SetProperty(
                            updateDetail => updateDetail.LastChapterUploadOrder,
                            updateDetail => updateDetail.LastChapterUploadOrder + 1),
                        cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch (System.Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
}
