using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM44Repository
{
    Task<bool> CreateGroupJoinRequestAsync(long groupId, long userId);
}
