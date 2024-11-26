using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM43Repository
{
    Task<long> AcceptGroupJoinRequestAsync(SocialGroupMember member, long userId);
}
