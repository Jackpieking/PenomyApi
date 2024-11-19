using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface ISM38Repository
{
    // 
    Task<bool> UpdateGroupDetailAsync(long userId, long groupId, string name, string description, string coverPhotoUrl);
}
