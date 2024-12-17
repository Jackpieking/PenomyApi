using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM12.OtherHandler;

public class SM12OtherResponse : IFeatureResponse
{
    public List<UserFriendRequest> FriendRequest { get; set; }
    public Sm12OtherResponseStatusCode StatusCode { get; set; }
}
