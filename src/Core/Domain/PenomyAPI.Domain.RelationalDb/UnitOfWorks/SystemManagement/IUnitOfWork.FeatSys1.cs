using PenomyAPI.Domain.RelationalDb.Repositories.Features.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    ISys1Repository Sys1Repository { get; }
}
