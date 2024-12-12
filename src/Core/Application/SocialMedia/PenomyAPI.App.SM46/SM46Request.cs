using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM46;

public class SM46Request : IFeatureRequest<SM46Response>
{
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    public string MemberId { get; set; }
    public string GroupId { get; set; }
}
