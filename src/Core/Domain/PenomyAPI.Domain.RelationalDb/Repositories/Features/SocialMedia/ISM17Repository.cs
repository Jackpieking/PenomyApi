using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM17Repository
{
    Task<string> LikeUnlikePostAsync(
        long userId,
        long postId,
        bool isGroupPost,
        CancellationToken token
    );
}
