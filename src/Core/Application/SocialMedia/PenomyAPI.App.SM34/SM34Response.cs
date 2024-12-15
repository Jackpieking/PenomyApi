using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM34;

public class SM34Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public long UserPostId { get; set; }

    public string[] ErrorMessages { get; set; }

    public SM34ResponseStatusCode StatusCode { get; set; }
}
