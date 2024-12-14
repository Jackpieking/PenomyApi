using PenomyAPI.App.Common;

namespace PenowmyAPI.APP.Chat2;

public class Chat2Request : IFeatureRequest<Chat2Response>
{
    public long UserId { get; set; }
}
