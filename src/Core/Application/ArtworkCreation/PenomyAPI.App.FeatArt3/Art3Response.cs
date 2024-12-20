using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt3;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt3;

public class Art3Response : IFeatureResponse
{
    public bool IsEmpty { get; set; }

    public List<Art3DeletedArtworkDetailReadModel> DeletedItems { get; set; }

    public static readonly Art3Response Empty = new()
    {
        IsEmpty = true
    };
}
