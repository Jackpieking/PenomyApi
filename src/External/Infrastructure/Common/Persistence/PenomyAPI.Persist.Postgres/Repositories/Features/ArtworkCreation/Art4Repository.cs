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

public sealed class Art4Repository : IArt4Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Category> _categoryDbSet;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkMetaData> _artworkMetaDataDbSet;
    private readonly DbSet<ArtworkCategory> _artworkCategoryDbSet;
    private readonly DbSet<ArtworkOrigin> _originDbSet;

    public Art4Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _categoryDbSet = dbContext.Set<Category>();
        _artworkDbSet = dbContext.Set<Artwork>();
        _artworkMetaDataDbSet = dbContext.Set<ArtworkMetaData>();
        _artworkCategoryDbSet = dbContext.Set<ArtworkCategory>();
        _originDbSet = dbContext.Set<ArtworkOrigin>();
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(
        CancellationToken cancellationToken
    )
    {
        return await _categoryDbSet
            .AsNoTracking()
            .Select(category => new Category { Id = category.Id, Name = category.Name, })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ArtworkOrigin>> GetAllOriginsAsync(
        CancellationToken cancellationToken
    )
    {
        return await _originDbSet
            .AsNoTracking()
            .Select(origin => new ArtworkOrigin
            {
                Id = origin.Id,
                CountryName = origin.CountryName,
                ImageUrl = origin.ImageUrl,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> CreateComicAsync(
        Artwork comic,
        ArtworkMetaData comicMetaData,
        IEnumerable<ArtworkCategory> artworkCategories,
        CancellationToken cancellationToken
    )
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalCreateComicAsync(
                    comic: comic,
                    comicMetaData: comicMetaData,
                    artworkCategories: artworkCategories,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalCreateComicAsync(
        Artwork comic,
        ArtworkMetaData comicMetaData,
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

            await _artworkDbSet.AddAsync(comic, cancellationToken);

            await _artworkMetaDataDbSet.AddAsync(comicMetaData, cancellationToken);

            await _artworkCategoryDbSet.AddRangeAsync(artworkCategories, cancellationToken);

            await _dbContext.Set<CreatorProfile>()
                .Where(creator => creator.CreatorId == comic.CreatedBy)
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
