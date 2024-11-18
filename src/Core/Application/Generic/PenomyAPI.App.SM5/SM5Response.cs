using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM5;

public class SM5Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SocialGroup Group { get; set; }
    public SM5ResponseStatusCode StatusCode { get; set; }
    public string[] Message { get; set; }
}
