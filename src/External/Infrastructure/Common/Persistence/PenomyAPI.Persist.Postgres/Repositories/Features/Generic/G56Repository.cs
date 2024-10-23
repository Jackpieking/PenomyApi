using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G56Repository : IG56Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;
    private readonly DbSet<UserLikeArtworkComment> _userLikeArtworkCommentDbSet;

    public G56Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
        _userLikeArtworkCommentDbSet = dbContext.Set<UserLikeArtworkComment>();
    }

    public async Task<long> ExcecuteLikeCommentAsync(
        long CommentId,
        long UserId,
        CancellationToken cancellationToken
    )
    {
        var UserLikeArtworkComment = new UserLikeArtworkComment
        {
            UserId = UserId,
            CommentId = CommentId,
            LikedAt = DateTime.UtcNow,
        };

        var result = new Result<long>(0);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await LikeCommentAsync(
                    CommentId,
                    UserLikeArtworkComment,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    public async Task LikeCommentAsync(
        long CommentId,
        UserLikeArtworkComment UserLikeArtworkComment,
        CancellationToken cancellationToken,
        Result<long> result
    )
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            await _artworkCommentDbSet
                .Where(c => c.Id == CommentId)
                .ExecuteUpdateAsync(
                    c => c.SetProperty(c => c.TotalLikes, c => c.TotalLikes + 1),
                    cancellationToken: cancellationToken
                );

            await _userLikeArtworkCommentDbSet.AddAsync(UserLikeArtworkComment);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            result.Value = 1;
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                result.Value = 2;
            }
            await transaction.DisposeAsync();

            result.Value = 3;
        }
    }
}
