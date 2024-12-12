using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM38.GroupProfile
{
    public class SM38ProfileRequest : IFeatureRequest<SM38ProfileResponse>
    {
        public long UserId { get; set; }
        public long GroupId {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public bool RequireApprovedWhenPost {get;set;}
    }
}
