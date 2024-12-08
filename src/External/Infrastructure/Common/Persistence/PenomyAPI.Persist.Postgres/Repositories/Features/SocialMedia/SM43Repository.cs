using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM43Repository : ISM43Repository
{
    private readonly DbSet<SocialGroupJoinRequest> _socialGroupJoinRequestDbSet;
    private readonly DbSet<SocialGroup> _socialGroupDbSet;
    private readonly DbSet<SocialGroupMember> _socialGroupMemberDbSet;
    private readonly DbContext _dbContext;

    public SM43Repository(DbContext dbContext)
    {
        _socialGroupJoinRequestDbSet = dbContext.Set<SocialGroupJoinRequest>();
        _socialGroupMemberDbSet = dbContext.Set<SocialGroupMember>();
        _socialGroupDbSet = dbContext.Set<SocialGroup>();
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

            await _socialGroupDbSet
                .Where(g => g.Id == member.GroupId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(o => o.TotalMembers, o => o.TotalMembers + 1)
                );

            await _socialGroupJoinRequestDbSet
                .Where(req => req.CreatedBy == member.MemberId && req.GroupId == member.GroupId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(o => o.RequestStatus, o => RequestStatus.Accepted));

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
