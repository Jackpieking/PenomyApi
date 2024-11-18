using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM7;

public class SM7Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public ICollection<SocialGroup> Result { get; set; }
    public SM7ResponseStatusCode StatusCode { get; set; }
}
