using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal sealed class Art10Repository : IArt10Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkChapter> _chapterDbSet;
    private readonly DbSet<ArtworkChapterMedia> _chapterMediaDbSet;

    public Art10Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _chapterDbSet = dbContext.Set<ArtworkChapter>();
        _chapterMediaDbSet = dbContext.Set<ArtworkChapterMedia>();
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

            await _chapterDbSet.AddAsync(comicChapter, cancellationToken);

            await _chapterMediaDbSet.AddRangeAsync(chapterMedias, cancellationToken);

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
