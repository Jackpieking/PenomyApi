using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    IArt12Repository Art12Repository { get; }
}
