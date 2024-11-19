using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    IArt5Repository Art5Repository { get; }
}
