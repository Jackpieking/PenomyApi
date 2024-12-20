using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM38.GroupProfile;

public class SM38ProfileResponse : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public bool Result { get; set; }
    public SM38ResponseStatusCode StatusCode { get; set; }
    public string[] Message { get; set; }
}
