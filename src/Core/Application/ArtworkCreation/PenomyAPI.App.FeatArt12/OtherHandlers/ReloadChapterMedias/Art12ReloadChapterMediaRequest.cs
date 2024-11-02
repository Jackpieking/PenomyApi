using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt12.OtherHandlers.ReloadChapterMedias;

public sealed class Art12ReloadChapterMediaRequest
    : IFeatureRequest<Art12ReloadChapterMediaResponse>
{
    public long ComicId { get; set; }

    public long ChapterId { get; set; }
}
