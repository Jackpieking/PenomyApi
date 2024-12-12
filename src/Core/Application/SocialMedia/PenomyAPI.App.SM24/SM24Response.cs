using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM24;

public class SM24Response : IFeatureResponse
{
    public long CommentId { get; set; }

    public bool IsSuccess { get; set; }

    public SM24ResponseStatusCode StatusCode { get; set; }
}
