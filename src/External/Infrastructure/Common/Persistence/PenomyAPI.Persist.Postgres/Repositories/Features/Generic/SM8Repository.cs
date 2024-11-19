using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class SM8Repository : ISM8Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<SocialGroup> _socialGroupDbSet;
    private readonly DbSet<SocialGroupMember> _socialGroupMemberDbSet;
    private readonly DbSet<PgRole> _pgRoleDbSet;

    public SM8Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _socialGroupDbSet = dbContext.Set<SocialGroup>();
        _socialGroupMemberDbSet = dbContext.Set<SocialGroupMember>();
        _pgRoleDbSet = dbContext.Set<PgRole>();
    }

    public async Task<long> CreateSocialGroupAsync(SocialGroup socialGroup)
    {
        var result = new Result<long>(0);
        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () => await CreateGroupAndOwnerAsync(socialGroup, result)
        );

        return result.Value;
    }

    public async Task CreateGroupAndOwnerAsync(SocialGroup socialGroup, Result<long> result)
    {
        IDbContextTransaction transaction = null;
        var cancellationToken = new CancellationToken();
        try
        {
            var roles = await _pgRoleDbSet.FirstOrDefaultAsync(
                r => r.Name == "GroupAdmin",
                new CancellationToken()
            );
            
            SocialGroupMember socialGroupMember =
                new()
                {
                    GroupId = socialGroup.Id,
                    MemberId = socialGroup.CreatedBy,
                    RoleId = roles.Id,
                    JoinedAt = socialGroup.CreatedAt,
                };

            transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            _socialGroupDbSet.Add(socialGroup);
            _socialGroupMemberDbSet.Add(socialGroupMember);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync(cancellationToken);

            result.Value = socialGroup.Id;
        }
        catch
        {
            result.Value = 0;
        }
    }
}
