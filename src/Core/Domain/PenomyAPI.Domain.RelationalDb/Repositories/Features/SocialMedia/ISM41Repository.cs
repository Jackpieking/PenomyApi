using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM41Repository
{
    Task<int> KickMemberAsync(long groupId, long userId);
}
