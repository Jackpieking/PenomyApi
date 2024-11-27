using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM43;

public class SM43Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public long MemberId { get; set; }
    public SM43ResponseStatusCode StatusCode { get; set; }
}
