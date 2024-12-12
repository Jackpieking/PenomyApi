using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM5Repository
{
    Task<SocialGroup> GetGroupDetailAsync(long userId, long groupId);
}
