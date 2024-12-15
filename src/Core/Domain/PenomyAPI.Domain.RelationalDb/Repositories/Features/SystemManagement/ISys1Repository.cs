using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SystemManagement;

public interface ISys1Repository
{
    public Task<long> CreateManagerAccountAsync(
        SystemAccount systemAccount,
        CancellationToken token
    );
}
