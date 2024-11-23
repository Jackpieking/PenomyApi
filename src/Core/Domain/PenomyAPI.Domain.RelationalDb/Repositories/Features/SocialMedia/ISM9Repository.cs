using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface ISM9Repository
{
    Task<List<SocialGroup>> GetSocialGroupsAsync(long userId);
}
