using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    ISM30Repository FeatSM30Repository { get; }
}
