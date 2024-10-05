using System;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG54Repository
{
    Task<bool> RemoveCommentAsync(Guid CommentId);
}
