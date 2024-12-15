using PenomyAPI.App.Common;

namespace PenomyAPI.App.Chat5;

public class Chat5Request : IFeatureRequest<Chat5Response>
{
    public long UserId { get; set; }
    public long MessageID { get; set; }
}
