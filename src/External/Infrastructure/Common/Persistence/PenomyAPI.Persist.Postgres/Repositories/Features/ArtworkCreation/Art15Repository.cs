using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal sealed class Art15Repository : IArt15Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Category> _categoryDbSet;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkMetaData> _artworkMetaDataDbSet;
    private readonly DbSet<ArtworkCategory> _artworkCategoryDbSet;
    private readonly DbSet<ArtworkOrigin> _originDbSet;

    public Art15Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _categoryDbSet = dbContext.Set<Category>();
        _artworkDbSet = dbContext.Set<Artwork>();
        _artworkMetaDataDbSet = dbContext.Set<ArtworkMetaData>();
        _artworkCategoryDbSet = dbContext.Set<ArtworkCategory>();
        _originDbSet = dbContext.Set<ArtworkOrigin>();
    }

    public async Task<bool> CreateAnimeAsync(
        Artwork anime,
        ArtworkMetaData metaData,
        IEnumerable<ArtworkCategory> artworkCategories,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalCreateAnimeAsync(
                    anime: anime,
                    metaData: metaData,
                    artworkCategories: artworkCategories,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalCreateAnimeAsync(
        Artwork anime,
        ArtworkMetaData metaData,
        IEnumerable<ArtworkCategory> artworkCategories,
        CancellationToken cancellationToken,
        Result<bool> result
    )
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            await _artworkDbSet.AddAsync(anime, cancellationToken);

            await _artworkMetaDataDbSet.AddAsync(metaData, cancellationToken);

            await _artworkCategoryDbSet.AddRangeAsync(artworkCategories, cancellationToken);

            await _dbContext.Set<CreatorProfile>()
                .Where(creator => creator.CreatorId == anime.CreatedBy)
                .ExecuteUpdateAsync(
                    creatorProfile => creatorProfile
                        .SetProperty(
                            creatorProfile => creatorProfile.TotalArtworks,
                            creatorProfile => creatorProfile.TotalArtworks + 1),
                    cancellationToken);

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
