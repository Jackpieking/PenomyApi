using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM24;

public class SM24Request : IFeatureRequest<SM24Response>
{
    public UserPostComment comment { get; set; }
    public long UserId { get; set; }
}
