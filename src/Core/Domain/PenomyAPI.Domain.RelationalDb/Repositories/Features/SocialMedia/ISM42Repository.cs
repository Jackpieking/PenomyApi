using System.Collections.Generic;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM42Repository
{
    Task<List<SocialGroupJoinRequest>> GetGroupJoinRequestAsync(long groupId);
}
