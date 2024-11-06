using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt5;

public class Art5Response : IFeatureResponse
{
    public Artwork ComicDetail { get; set; }

    public Art5ResponseAppCode AppCode { get; set; }

    public static readonly Art5Response COMIC_ID_NOT_FOUND = new()
    {
        AppCode = Art5ResponseAppCode.COMIC_ID_NOT_FOUND,
    };

    public static readonly Art5Response COMIC_IS_TEMPORARILY_REMOVED = new()
    {
        AppCode = Art5ResponseAppCode.COMIC_IS_TEMPORARILY_REMOVED
    };

    public static readonly Art5Response COMIC_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR = new()
    {
        AppCode = Art5ResponseAppCode.COMIC_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR
    };
}
