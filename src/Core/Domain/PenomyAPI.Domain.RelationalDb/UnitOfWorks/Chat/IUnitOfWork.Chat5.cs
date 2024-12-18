using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    IChat5Repository Chat5Repository { get; }
}
