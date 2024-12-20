using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM50Repository
{
    Task<bool> LeaveSocialGroupAsync(long userId, long groupId, CancellationToken token);
}
