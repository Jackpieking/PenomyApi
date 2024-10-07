using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatG8
{
    public class G8Response : IFeatureResponse
    {
        public bool IsSuccess { get; set; }

        public List<ArtworkChapter> Result { get; set; }

        public G8ResponseStatusCode StatusCode { get; set; }
    }
}
