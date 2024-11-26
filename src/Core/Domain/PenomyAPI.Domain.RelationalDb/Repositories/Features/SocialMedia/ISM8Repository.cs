using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM8Repository
{
    Task<long> CreateSocialGroupAsync(SocialGroup socialGroup);
}
