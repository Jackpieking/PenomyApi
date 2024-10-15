using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks
{
    public partial interface IUnitOfWork
    {
        IG44Repository G44Repository { get; }
    }
}
