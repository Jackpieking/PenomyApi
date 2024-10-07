using System;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG53Repository
{
    Task<bool> EditCommentAsync(long CommentId, string NewComment);
}
