using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatChat1;

public class Chat1Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public long Result { get; set; }
    public Chat1ResponseStatusCode StatusCode { get; set; }
    public string[] Message { get; set; }
}
