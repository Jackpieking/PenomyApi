using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Persist.Postgres.Repositories.Common;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for ArtworkChapterRepository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArtworkChapterRepository _chapterRepository;

    public IArtworkChapterRepository ChapterRepository
    {
        get
        {
            if (Equals(_chapterRepository, null))
            {
                _chapterRepository = new ArtworkChapterRepository(_dbContext);
            }

            return _chapterRepository;
        }
    }
}
