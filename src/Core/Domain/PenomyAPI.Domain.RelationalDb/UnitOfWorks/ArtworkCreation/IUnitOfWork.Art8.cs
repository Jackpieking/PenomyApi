using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    IArt8Repository Art8Repository { get; }
}
