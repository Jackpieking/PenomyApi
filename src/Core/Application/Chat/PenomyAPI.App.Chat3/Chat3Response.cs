using PenomyAPI.App.Common;

namespace PenomyAPI.App.Chat3;

public class Chat3Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public long MessageId { get; set; }

    public string[] ErrorMessages { get; set; }

    public Chat3ResponseStatusCode StatusCode { get; set; }
}
