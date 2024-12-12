using PenomyAPI.Domain.RelationalDb.Repositories.Common;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

/// <summary>
///     This IUnitOfWork file only contains the Common Repositories.
///     Please do not add any Feature Repositories in this UnitOfWork file.
/// </summary>
public partial interface IUnitOfWork
{
    IDataSeedingRepository DataSeedingRepository { get; }

    IArtworkRepository ArtworkRepository { get; }

    IArtworkChapterRepository ChapterRepository { get; }

    ICreatorRepository CreatorRepository { get; }
}
