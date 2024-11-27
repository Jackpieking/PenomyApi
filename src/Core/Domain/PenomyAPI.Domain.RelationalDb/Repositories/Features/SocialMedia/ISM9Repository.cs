using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM9Repository
{
    Task<List<SocialGroup>> GetSocialGroupsAsync(long userId);
}
