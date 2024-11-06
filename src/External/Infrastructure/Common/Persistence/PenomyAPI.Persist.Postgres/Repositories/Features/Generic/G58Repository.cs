using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G58Repository : IG58Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;
    private readonly DbSet<ArtworkCommentParentChild> _commentParentChildDbSet;

    public G58Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _commentParentChildDbSet = dbContext.Set<ArtworkCommentParentChild>();
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
    }

    public async Task<long> ExcecuteReplyCommentAsync(
        ArtworkComment Comment,
        long ParentCommentId,
        CancellationToken cancellationToken
    )
    {
        var result = new Result<long>(0);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await CreateReplyCommentAsync(
                    Comment,
                    ParentCommentId,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    public async Task CreateReplyCommentAsync(
        ArtworkComment Comment,
        long ParentCommentId,
        CancellationToken cancellationToken,
        Result<long> result
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            var parentComment = await _artworkCommentDbSet.FindAsync(ParentCommentId);
            if (parentComment == null)
                result.Value = 3;

            transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            _artworkCommentDbSet.Add(Comment);

            await _commentParentChildDbSet.AddAsync(
                new ArtworkCommentParentChild
                {
                    ChildCommentId = Comment.Id,
                    ParentCommentId = ParentCommentId,
                }
            );

            parentComment.TotalChildComments = parentComment.TotalChildComments + 1;

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync(cancellationToken);

            result.Value = Comment.Id;
        }
        catch
        {
            result.Value = 0;
        }
    }
}
