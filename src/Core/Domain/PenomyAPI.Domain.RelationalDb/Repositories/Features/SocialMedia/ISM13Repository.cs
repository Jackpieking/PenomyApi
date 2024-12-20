using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM13Repository
{
    Task<bool> IsUserPostExistedAsync(long id, CancellationToken cancellationToken);
    Task<UserPost> GetUserPostByIdAsync(long id, CancellationToken cancellationToken);

    Task<bool> UpdateUserPostAsync(UserPost updatePost,
        bool isImageUpdate,
        IEnumerable<UserPostAttachedMedia> attachedMediae,
        CancellationToken token = default);
}
