using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Persist.Postgres.Repositories.Common;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for ArtworkRepository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArtworkRepository _artworkRepository;

    public IArtworkRepository ArtworkRepository
    {
        get
        {
            if (Equals(_artworkRepository, null))
            {
                _artworkRepository = new ArtworkRepository(_dbContext);
            }

            return _artworkRepository;
        }
    }
}