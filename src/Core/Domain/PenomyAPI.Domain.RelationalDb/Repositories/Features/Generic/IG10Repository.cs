using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG10Repository
{
    Task<List<ArtworkComment>> GetCommentsAsync(long artworkId);
}
