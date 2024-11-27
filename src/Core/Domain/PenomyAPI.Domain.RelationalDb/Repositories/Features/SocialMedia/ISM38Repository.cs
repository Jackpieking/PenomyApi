using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM38Repository
{
    Task<int> UpdateGroupDetailAsync(
        long userId,
        long groupId,
        string name,
        string description,
        bool RequireApprovedWhenPost
    );

    Task<int> UpdateGroupCoverPhotoAsync(long userId, long groupId, string coverPhotoUrl);
}
