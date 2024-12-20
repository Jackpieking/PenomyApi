using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    IG6Repository G6Repository { get; }
}
