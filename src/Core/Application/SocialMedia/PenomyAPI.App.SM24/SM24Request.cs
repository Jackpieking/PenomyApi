using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM24;

public class SM24Request : IFeatureRequest<SM24Response>
{
    public string Comment { get; init; }
    public long PostId { get; init; }
    public bool IsGroupPostComment { get; init; }
    private long _userId { get; set; }

    public void SetUserId(long userId) => _userId = userId;

    public long GetUserId() => _userId;
}
