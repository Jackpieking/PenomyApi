using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG52Repository
{
    Task<long> CreateCommentAsync(ArtworkComment comment);
}
