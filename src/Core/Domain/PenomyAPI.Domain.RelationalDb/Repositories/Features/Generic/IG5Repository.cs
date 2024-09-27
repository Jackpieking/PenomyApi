using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG5Repository
{
    Task<Artwork> GetArtWorkDetailByIdAsync(long artworkId);
}
