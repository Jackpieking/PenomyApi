using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG5Repository
{
    Task<Artwork> GetArtWorkDetailByIdAsync(long artworkId, CancellationToken ct = default);
}
