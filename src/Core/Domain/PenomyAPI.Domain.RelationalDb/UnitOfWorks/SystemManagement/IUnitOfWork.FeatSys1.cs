
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    IFeatSys1Repository FeatSys1Repository { get; }
}
