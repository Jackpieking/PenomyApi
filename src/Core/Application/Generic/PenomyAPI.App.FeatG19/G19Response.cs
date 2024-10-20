using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG19
{
    public class G19Response : IFeatureResponse
    {
        public bool IsSuccess { get; set; }

        public List<ArtworkChapter> Chapters { get; set; }
        public int ChapterCount { get; set; }
        public G19ResponseStatusCode StatusCode { get; set; }
    }
}
