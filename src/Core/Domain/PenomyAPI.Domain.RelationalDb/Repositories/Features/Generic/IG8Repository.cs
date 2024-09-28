using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG8Repository
{
    Task<List<ArtworkChapter>> GetArtWorkChapterById(long id, int startPage = 1, int pageSize = 10);
}
