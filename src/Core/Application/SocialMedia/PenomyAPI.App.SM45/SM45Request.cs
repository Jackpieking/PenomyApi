using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM45;

public class SM45Request : IFeatureRequest<SM45Response>
{
    private string _userId;

    public string GetUserId() => _userId;

    public void SetUserId(string userId) => _userId = userId;

    public long GroupId { get; set; }
}
