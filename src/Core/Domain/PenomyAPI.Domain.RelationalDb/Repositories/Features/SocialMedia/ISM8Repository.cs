using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM8Repository
{
    Task<long> CreateSocialGroupAsync(SocialGroup socialGroup);
}
