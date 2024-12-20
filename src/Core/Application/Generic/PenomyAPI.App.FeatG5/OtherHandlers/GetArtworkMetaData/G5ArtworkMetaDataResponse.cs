using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG5.OtherHandlers.GetArtworkMetaData;

public class G5ArtworkMetaDataResponse : IFeatureResponse
{
    public G5ArtworkMetaDataResponseAppCode AppCode { get; set; }

    public ArtworkMetaData ArtworkMetaData { get; set; }

    public static readonly G5ArtworkMetaDataResponse ARTWORK_IS_NOT_FOUND = new()
    {
        AppCode = G5ArtworkMetaDataResponseAppCode.ARTWORK_IS_NOT_FOUND,
    };

    public static G5ArtworkMetaDataResponse SUCCESS(ArtworkMetaData artworkMetaData) => new()
    {
        AppCode = G5ArtworkMetaDataResponseAppCode.SUCCESS,
        ArtworkMetaData = artworkMetaData
    };
}
