using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    IFeatChat1Repository FeatChat1Repository { get; }
}
