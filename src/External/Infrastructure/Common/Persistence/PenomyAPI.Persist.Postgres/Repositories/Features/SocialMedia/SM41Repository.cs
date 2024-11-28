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

public class SM41Repository : ISM41Repository
{
    private readonly DbSet<SocialGroupMember> _socialGroupMemberDbSet;
    private readonly DbSet<SocialGroup> _socialGroupDbSet;
    private readonly DbContext _dbContext;

    public SM41Repository(DbContext dbContext)
    {
        _socialGroupMemberDbSet = dbContext.Set<SocialGroupMember>();
        _socialGroupDbSet = dbContext.Set<SocialGroup>();
        _dbContext = dbContext;
    }

    public async Task<int> KickMemberAsync(
        long groupId,
        long memberId,
        long userId,
        CancellationToken ct
    )
    {
        Result<int> result = new Result<int>(0);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            async () => await KickMember(groupId, memberId, userId, result, ct)
        );

        return result.Value;
    }

    public async Task KickMember(
        long groupId,
        long memberId,
        long userId,
        Result<int> result,
        CancellationToken ct
    )
    {
        try
        {
            IDbContextTransaction transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                ct
            );

            await _socialGroupMemberDbSet
                .Where(o => o.GroupId == groupId && o.MemberId == memberId)
                .ExecuteDeleteAsync(ct);

            await _socialGroupDbSet
                .Where(g => g.Id == groupId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(o => o.TotalMembers, o => o.TotalMembers - 1)
                );

            await _dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            result.Value = 1;
        }
        catch
        {
            result.Value = 0;
        }
        ;
    }
}
