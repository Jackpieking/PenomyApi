using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    IArt6Repository Art6Repository { get; }
}
