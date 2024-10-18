using PenomyAPI.Domain.RelationalDb.UnitOfWorks.Common.Common;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

/// <summary>
///     This IUnitOfWork file only contains the Common Repositories.
///     Please do not add any Feature Repositories in this UnitOfWork file.
/// </summary>
public partial interface IUnitOfWork
{
    IArtworkRepository ArtworkRepository { get; }
}
