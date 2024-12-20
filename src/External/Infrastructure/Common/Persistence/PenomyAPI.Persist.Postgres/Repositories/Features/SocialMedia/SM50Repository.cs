using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM50Repository : ISM50Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<SocialGroupMember> _socialGroupMemberContext;
    private readonly DbSet<SocialGroup> _socialGroupContext;

    public SM50Repository(AppDbContext context)
    {
        _dbContext = context;
        _socialGroupContext = context.Set<SocialGroup>();
        _socialGroupMemberContext = context.Set<SocialGroupMember>();
    }

    public async Task<bool> LeaveSocialGroupAsync(
        long userId,
        long groupId,
        CancellationToken token
    )
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        await executionStrategy.ExecuteAsync(
            async () => await InternalLeaveGroupAsync(userId, groupId, result, token)
        );
        return result.Value;
    }

    private async Task InternalLeaveGroupAsync(
        long userId,
        long groupId,
        Result<bool> result,
        CancellationToken token
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            // Start the transaction
            transaction = await RepositoryHelper.CreateTransactionAsync(_dbContext, token);

            await _socialGroupMemberContext
                .Where(x => x.MemberId == userId && x.GroupId == groupId)
                .ExecuteDeleteAsync(token);

            await _socialGroupContext
                .Where(x => x.Id == groupId)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(o => o.TotalMembers, o => o.TotalMembers - 1),
                    token
                );

            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            result.Value = true;
        }
        catch (Exception)
        {
            result.Value = false;
        }
    }
}
