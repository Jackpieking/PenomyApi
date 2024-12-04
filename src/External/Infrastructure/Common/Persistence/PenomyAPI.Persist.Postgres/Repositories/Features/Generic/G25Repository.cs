using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG25;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G25Repository : IG25Repository
{
    private const int MAX_VIEW_HISTORY_RETURN_RECORDS = 64;

    private readonly DbContext _dbContext;
    private readonly DbSet<UserArtworkViewHistory> _viewHistoryDbSet;
    private readonly DbSet<ArtworkMetaData> _artworkMetaDataDbSet;

    public G25Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _viewHistoryDbSet = dbContext.Set<UserArtworkViewHistory>();
        _artworkMetaDataDbSet = dbContext.Set<ArtworkMetaData>();
    }

    #region For User section.
    public Task<bool> IsUserViewHistoryNotEmptyAsync(
        long userId,
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<UserArtworkViewHistory>()
            .AnyAsync(
                viewHistory => viewHistory.UserId == userId
                && viewHistory.ArtworkType == artworkType,
                cancellationToken);
    }

    public Task<bool> IsUserViewHistoryRecordExistedAsync(
        long userId,
        long artworkId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<UserArtworkViewHistory>()
            .AnyAsync(
                viewHistory => viewHistory.ArtworkId == artworkId
                && viewHistory.UserId == userId,
                cancellationToken);
    }

    public async Task<int> GetTotalOfArtworksByUserIdAndTypeAsync(
        long userId,
        ArtworkType artType,
        CancellationToken ct
    )
    {
        return await _viewHistoryDbSet
            .AsNoTracking()
            .Where(viewHist => viewHist.UserId == userId && viewHist.ArtworkType == artType && !viewHist.Artwork.IsTakenDown && !viewHist.Artwork.IsTemporarilyRemoved)
            .GroupBy(viewHist => viewHist.ArtworkId)
            .CountAsync(ct);
    }

    public Task<List<G25ViewHistoryArtworkReadModel>> GetArtworkViewHistByUserIdAndTypeWithPaginationAsync(
        long userId,
        ArtworkType artType,
        int pageNum,
        int artNum,
        CancellationToken ct
    )
    {
        return _viewHistoryDbSet
            .AsNoTracking()
            .Where(
                viewHistory => viewHistory.UserId == userId
                && viewHistory.ArtworkType == artType)
            .Select(viewHistory => new G25ViewHistoryArtworkReadModel
            {
                Id = viewHistory.ArtworkId,
                ArtworkStatus = viewHistory.Artwork.ArtworkStatus,
                Title = viewHistory.Artwork.Title,
                ThumbnailUrl = viewHistory.Artwork.ThumbnailUrl,
                OriginImageUrl = viewHistory.Artwork.Origin.ImageUrl,
                TotalStarRates = viewHistory.Artwork.ArtworkMetaData.TotalStarRates,
                TotalUsersRated = viewHistory.Artwork.ArtworkMetaData.TotalUsersRated,
                // Creator section.
                CreatorId = viewHistory.Artwork.Creator.UserId,
                CreatorName = viewHistory.Artwork.Creator.NickName,
                CreatorAvatarUrl = viewHistory.Artwork.Creator.AvatarUrl,
                // Recently viewed chapter section.
                Chapter = new G25ViewHistoryChapterReadModel
                {
                    Id = viewHistory.ChapterId,
                    UploadOrder = viewHistory.Chapter.UploadOrder,
                    ViewedAt = viewHistory.ViewedAt,
                }
            })
            .OrderByDescending(viewHistory => viewHistory.Chapter.ViewedAt)
            .Take(MAX_VIEW_HISTORY_RETURN_RECORDS)
            .ToListAsync(ct);
    }

    public async Task<bool> AddUserViewHistoryAsync(
        long userId,
        long artworkId,
        long chapterId,
        ArtworkType type,
        CancellationToken ct
    )
    {
        await _dbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken: ct);

            var viewedAt = DateTime.UtcNow;

            await _viewHistoryDbSet.AddAsync(
                new UserArtworkViewHistory
                {
                    UserId = userId,
                    ArtworkId = artworkId,
                    ChapterId = chapterId,
                    ArtworkType = type,
                    ViewedAt = viewedAt
                },
                ct
            );

            // Update the total views of both artwork and chapter.
            await _artworkMetaDataDbSet
                .Where(o => o.ArtworkId == artworkId)
                .ExecuteUpdateAsync(o => o.SetProperty(o => o.TotalViews, e => e.TotalViews + 1));

            await _dbContext.Set<ArtworkChapterMetaData>()
                .Where(metadata => metadata.ChapterId == chapterId)
                .ExecuteUpdateAsync(
                    metadata => metadata
                        .SetProperty(o => o.TotalViews, e => e.TotalViews + 1),
                    ct);

            await _dbContext.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

        });

        return true;
    }

    public async Task<bool> UpdateUserViewHistoryAsync(
        long userId,
        long artworkId,
        long chapterId,
        CancellationToken ct
    )
    {
        await _dbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken: ct);

            var viewedAt = DateTime.UtcNow;

            await _viewHistoryDbSet
                .Where(
                    viewHistory => viewHistory.UserId == userId
                    && viewHistory.ArtworkId == artworkId)
                .ExecuteUpdateAsync(
                    viewHistory => viewHistory
                        .SetProperty(
                            viewHistory => viewHistory.ChapterId,
                            viewHistory => chapterId)
                        .SetProperty(
                            viewHistory => viewHistory.ViewedAt,
                            viewHistory => viewedAt),
                    ct);

            // Update the total views of both chapter and artwork.
            await _artworkMetaDataDbSet
                .Where(o => o.ArtworkId == artworkId)
                .ExecuteUpdateAsync(o => o.SetProperty(o => o.TotalViews, e => e.TotalViews + 1));

            await _dbContext.Set<ArtworkChapterMetaData>()
                .Where(metadata => metadata.ChapterId == chapterId)
                .ExecuteUpdateAsync(
                    metadata => metadata
                        .SetProperty(o => o.TotalViews, e => e.TotalViews + 1),
                    ct);

            await _dbContext.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

        });

        return true;
    }

    public async Task<bool> RemoveUserViewHistoryItemAsync(
        long userId,
        long artworkId,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () => await InternalRemoveUserViewHistoryItem(
                userId,
                artworkId,
                cancellationToken,
                result));

        return result.Value;
    }

    private async Task InternalRemoveUserViewHistoryItem(
        long userId,
        long artworkId,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            await _dbContext.Set<UserArtworkViewHistory>()
                .Where(
                    viewHistory => viewHistory.UserId == userId
                    && viewHistory.ArtworkId == artworkId)
                .ExecuteDeleteAsync(cancellationToken);


            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
    #endregion

    #region For Guest section.
    public Task<GuestTracking> GetGuestTrackingByIdAsync(
        long guestId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<GuestTracking>()
            .AsNoTracking()
            .Where(guest => guest.GuestId == guestId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> InitGuestViewHistoryAsync(
        long guestId,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () => await InternalInitGuestViewHistory(guestId, cancellationToken, result));

        return result.Value;
    }

    private async Task InternalInitGuestViewHistory(
        long guestId,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            // Create a new guest tracking record in DB with last active is current UTC datetime.
            var newGuestTrackingRecord = new GuestTracking
            {
                GuestId = guestId,
                LastActiveAt = DateTime.UtcNow,
            };

            await _dbContext.Set<GuestTracking>().AddAsync(newGuestTrackingRecord, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }

    public Task<bool> IsGuestIdExistedAsync(
        long guestId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<GuestTracking>()
            .AnyAsync(guest => guest.GuestId == guestId, cancellationToken);
    }

    public Task<bool> IsGuestViewHistoryNotEmptyAsync(
        long guestId,
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<GuestArtworkViewHistory>()
            .AnyAsync(
                viewHistory => viewHistory.GuestId == guestId
                && viewHistory.ArtworkType == artworkType,
                cancellationToken);
    }

    public Task<List<G25ViewHistoryArtworkReadModel>> GetGuestViewHistoryByArtworkTypeAsync(
        long guestId,
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<GuestArtworkViewHistory>()
            .AsNoTracking()
            .Where(
                viewHistory => viewHistory.GuestId == guestId
                && viewHistory.ArtworkType == artworkType)
            .Select(viewHistory => new G25ViewHistoryArtworkReadModel
            {
                Id = viewHistory.ArtworkId,
                ArtworkStatus = viewHistory.Artwork.ArtworkStatus,
                Title = viewHistory.Artwork.Title,
                ThumbnailUrl = viewHistory.Artwork.ThumbnailUrl,
                OriginImageUrl = viewHistory.Artwork.Origin.ImageUrl,
                TotalStarRates = viewHistory.Artwork.ArtworkMetaData.TotalStarRates,
                TotalUsersRated = viewHistory.Artwork.ArtworkMetaData.TotalUsersRated,
                // Creator section.
                CreatorId = viewHistory.Artwork.Creator.UserId,
                CreatorName = viewHistory.Artwork.Creator.NickName,
                CreatorAvatarUrl = viewHistory.Artwork.Creator.AvatarUrl,
                // Recently viewed chapter section.
                Chapter = new G25ViewHistoryChapterReadModel
                {
                    Id = viewHistory.ChapterId,
                    UploadOrder = viewHistory.Chapter.UploadOrder,
                    ViewedAt = viewHistory.ViewedAt,
                }
            })
            .OrderByDescending(viewHistory => viewHistory.Chapter.ViewedAt)
            .Take(MAX_VIEW_HISTORY_RETURN_RECORDS)
            .ToListAsync(cancellationToken);
    }

    public Task<ArtworkType> GetArtworkTypeForAddingViewHistoryRecordAsync(
        long artworkId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<Artwork>()
            .Where(artwork => artwork.Id == artworkId)
            .Select(artwork => artwork.ArtworkType)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsGuestViewHistoryRecordExistedAsync(
        long guestId,
        long artworkId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<GuestArtworkViewHistory>()
            .AnyAsync(
                viewHistory => viewHistory.ArtworkId == artworkId
                && viewHistory.GuestId == guestId,
                cancellationToken);
    }

    public async Task<bool> AddGuestViewHistoryAsync(
        long guestId,
        ArtworkType artworkType,
        long artworkId,
        long chapterId,
        DateTime viewedAt,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () => await InternalAddGuestViewHistory(
                guestId,
                artworkType,
                artworkId,
                chapterId,
                viewedAt,
                cancellationToken,
                result));

        return result.Value;
    }

    private async Task InternalAddGuestViewHistory(
        long guestId,
        ArtworkType artworkType,
        long artworkId,
        long chapterId,
        DateTime viewedAt,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            // Create a new guest tracking record in DB with last active is current UTC datetime.
            var newGuestTrackingRecord = new GuestArtworkViewHistory
            {
                GuestId = guestId,
                ArtworkId = artworkId,
                ChapterId = chapterId,
                ArtworkType = artworkType,
                ViewedAt = viewedAt,
            };

            await _dbContext.Set<GuestArtworkViewHistory>()
                .AddAsync(newGuestTrackingRecord, cancellationToken);

            // Update again the last active at for the guest.
            await _dbContext.Set<GuestTracking>()
                .Where(guest => guest.GuestId == guestId)
                .ExecuteUpdateAsync(
                    guestTracking => guestTracking
                        .SetProperty(guest => guest.LastActiveAt, guest => viewedAt),
                    cancellationToken);

            // Increase the total views of both chapter and artwork.
            await _artworkMetaDataDbSet
                .Where(metadata => metadata.ArtworkId == artworkId)
                .ExecuteUpdateAsync(
                    metadata => metadata
                        .SetProperty(
                            metadata => metadata.TotalViews,
                            metadata => metadata.TotalViews + 1),
                    cancellationToken);

            await _dbContext.Set<ArtworkChapterMetaData>()
                .Where(metadata => metadata.ChapterId == chapterId)
                .ExecuteUpdateAsync(
                    metadata => metadata
                        .SetProperty(o => o.TotalViews, e => e.TotalViews + 1),
                    cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }

    public async Task<bool> UpdateGuestViewHistoryAsync(
        long guestId,
        long artworkId,
        long chapterId,
        DateTime viewedAt,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () => await InternalUpdateGuestViewHistory(
                guestId,
                artworkId,
                chapterId,
                viewedAt,
                cancellationToken,
                result));

        return result.Value;
    }

    private async Task InternalUpdateGuestViewHistory(
        long guestId,
        long artworkId,
        long chapterId,
        DateTime viewedAt,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            // Update again the last active at for the guest.
            await _dbContext.Set<GuestTracking>()
                .Where(guest => guest.GuestId == guestId)
                .ExecuteUpdateAsync(
                    guestTracking => guestTracking
                        .SetProperty(guest => guest.LastActiveAt, guest => viewedAt),
                    cancellationToken);

            await _dbContext.Set<GuestArtworkViewHistory>()
                .Where(
                    viewHistory => viewHistory.GuestId == guestId
                    && viewHistory.ArtworkId == artworkId)
                .ExecuteUpdateAsync(
                    viewHistory => viewHistory
                        .SetProperty(
                            viewHistory => viewHistory.ChapterId,
                            viewHistory => chapterId)
                        .SetProperty(
                            viewHistory => viewHistory.ViewedAt,
                            viewHistory => viewedAt),
                    cancellationToken);

            await _artworkMetaDataDbSet
                .Where(metadata => metadata.ArtworkId == artworkId)
                .ExecuteUpdateAsync(
                    metadata => metadata
                        .SetProperty(o => o.TotalViews, e => e.TotalViews + 1),
                    cancellationToken);

            await _dbContext.Set<ArtworkChapterMetaData>()
                .Where(metadata => metadata.ChapterId == chapterId)
                .ExecuteUpdateAsync(
                    metadata => metadata
                        .SetProperty(o => o.TotalViews, e => e.TotalViews + 1),
                    cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }

    public async Task<bool> RemoveGuestViewHistoryItemAsync(
        long guestId,
        long artworkId,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () => await InternalRemoveGuestViewHistoryItem(
                guestId,
                artworkId,
                cancellationToken,
                result));

        return result.Value;
    }

    private async Task InternalRemoveGuestViewHistoryItem(
        long guestId,
        long artworkId,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            await _dbContext.Set<GuestArtworkViewHistory>()
                .Where(
                    viewHistory => viewHistory.GuestId == guestId
                    && viewHistory.ArtworkId == artworkId)
                .ExecuteDeleteAsync(cancellationToken);


            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
    #endregion
}
