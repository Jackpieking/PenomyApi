using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SystemManagement;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SystemManagement;

public class Sys1Repository : ISys1Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<SystemAccount> _systemAccountDbSet;

    public Sys1Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _systemAccountDbSet = dbContext.Set<SystemAccount>();
    }

    public async Task<long> CreateManagerAccountAsync(
        SystemAccount systemAccount,
        CancellationToken token
    )
    {
        try
        {
            await _systemAccountDbSet.AddAsync(systemAccount, token);
            await _dbContext.SaveChangesAsync(token);
            return systemAccount.Id;
        }
        catch
        {
            return 0;
        }
    }
}
