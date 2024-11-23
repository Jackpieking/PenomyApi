using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM38.CoverImage;

public class SM38ImageResponse : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public string Result { get; set; }
    public SM38ResponseStatusCode StatusCode { get; set; }
    public string[] Message { get; set; }
}
