using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    IFeatArt4Repository FeatArt4Repository { get; }
}