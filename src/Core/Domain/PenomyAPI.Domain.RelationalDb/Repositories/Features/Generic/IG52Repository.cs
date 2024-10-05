using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG52Repository
{
    long CreateCommentAsync(ArtworkComment Comment);
}
