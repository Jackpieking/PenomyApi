using PenomyAPI.App.Common;

namespace PenomyAPI.App.Sys1;

public class Sys1Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public long AccountId { get; set; }
    public Sys1ResponseStatusCode StatusCode { get; set; }
    public string[] Message { get; set; }
}
