using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks
{
    public partial interface IUnitOfWork
    {
        IG43Repository G43Repository { get; }
    }
}
