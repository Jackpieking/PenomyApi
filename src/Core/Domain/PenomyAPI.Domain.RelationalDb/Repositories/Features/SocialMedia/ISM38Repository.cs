using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface ISM38Repository
{
    Task<int> UpdateGroupDetailAsync(
        long userId,
        long groupId,
        string name,
        string description,
        bool RequireApprovedWhenPost,
        SocialGroupStatus socialGroupStatus
    );

    Task<int> UpdateGroupCoverPhotoAsync(long userId, long groupId, string coverPhotoUrl);
}
