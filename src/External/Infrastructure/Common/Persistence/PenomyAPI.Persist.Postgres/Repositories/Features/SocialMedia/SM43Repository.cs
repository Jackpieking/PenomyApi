﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM43Repository : ISM43Repository
{
    private readonly DbSet<SocialGroupJoinRequest> _socialGroupJoinRequestDbSet;
    private readonly DbSet<SocialGroupMember> _socialGroupMemberDbSet;
    private readonly DbContext _dbContext;

    public SM43Repository(DbContext dbContext)
    {
        _socialGroupJoinRequestDbSet = dbContext.Set<SocialGroupJoinRequest>();
        _socialGroupMemberDbSet = dbContext.Set<SocialGroupMember>();
        _dbContext = dbContext;
    }

    public async Task<long> AcceptGroupJoinRequestAsync(SocialGroupMember member, long userId)
    {
        Result<long> result = new Result<long>(0);
        CancellationToken cancellationToken = new CancellationToken();

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            async () => await AcceptGroupJoinRequest(member, userId, result, cancellationToken)
        );

        return result.Value;
    }

    public async Task AcceptGroupJoinRequest(
        SocialGroupMember member,
        long userId,
        Result<long> result,
        CancellationToken cancellationToken
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            await _socialGroupMemberDbSet.AddAsync(member);
            await _socialGroupJoinRequestDbSet
                .Where(req => req.CreatedBy == member.MemberId && req.GroupId == member.GroupId)
                .ExecuteDeleteAsync(cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            result.Value = member.MemberId;
        }
        catch
        {
            result.Value = -1;
        }
    }
}