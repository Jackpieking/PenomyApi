using Microsoft.EntityFrameworkCore;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

public sealed class FeatArt4Repository : IFeatArt4Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Category> _categoryDbSet;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkCategory> _artworkCategoryDbSet;

    public FeatArt4Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _categoryDbSet = dbContext.Set<Category>();
        _artworkDbSet = dbContext.Set<Artwork>();
        _artworkCategoryDbSet = dbContext.Set<ArtworkCategory>();
    }

    public async Task<bool> CreateComicAsync(
        Artwork comic,
        IEnumerable<ArtworkCategory> artworkCategories,
        CancellationToken ct)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(async () => await InternalCreateComicAsync(comic, artworkCategories, ct, result));

        return result.Value;
    }

    private async Task InternalCreateComicAsync(
        Artwork comic,
        IEnumerable<ArtworkCategory> artworkCategories,
        CancellationToken ct,
        Result<bool> result)
    {
        await using var transaction = await RepositoryHelper.CreateTransactionAsync(_dbContext, ct);

        try
        {
            await _artworkDbSet.AddAsync(comic);

            await _artworkCategoryDbSet.AddRangeAsync(artworkCategories);

            await transaction.CommitAsync(ct);

            result.Value = true;
        }
        catch (System.Exception)
        {
            await transaction.RollbackAsync(ct);
        }
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken ct)
    {
        return await _categoryDbSet.Select(category => new Category
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
        })
        .ToListAsync(ct);
    }
}
