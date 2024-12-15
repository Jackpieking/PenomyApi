using PenomyAPI.App.Common;

namespace PenomyAPI.App.Chat5;

public class Chat5Response : IFeatureResponse
{
    public bool isSuccess { get; set; }

    public Chat5ResponseStatusCode StatusCode { get; set; }
}
