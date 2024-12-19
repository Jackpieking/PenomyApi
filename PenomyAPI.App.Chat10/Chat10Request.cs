using PenomyAPI.App.Common;

namespace PenomyAPI.App.Chat10;

public class Chat10Request : IFeatureRequest<Chat10Response>
{
    public long ChatGroupId { get; set; }
    public int PageNum { get; set; }
    public int ChatNum { get; set; }
}
