using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

public sealed class FeatChat1Repository : IFeatChat1Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ChatGroup> _groupContext;

    public FeatChat1Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _groupContext = _dbContext.Set<ChatGroup>();
    }

    public async Task<bool> CreateGroupAsync(ChatGroup group, ChatGroupMember member, CancellationToken token)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        await executionStrategy.ExecuteAsync(async () =>
            await InternalCreateGroupAsync(
                group,
                member,
                token,
                result
            ));
        return result.Value;
    }

    private async Task InternalCreateGroupAsync(ChatGroup group,
        ChatGroupMember member,
        CancellationToken token, Result<bool> result)
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                token
            );
            await _groupContext.AddAsync(group, token);
            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            result.Value = true;
        }
        catch (Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(token);
                await transaction.DisposeAsync();
            }

            result.Value = false;
        }
    }
}
