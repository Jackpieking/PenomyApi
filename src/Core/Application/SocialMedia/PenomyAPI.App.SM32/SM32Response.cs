using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.App.SM32;

public class SM32Response : IFeatureResponse
{
    public IEnumerable<UserProfile> UserProfiles { get; set; }
    public IEnumerable<UserProfile> FriendIds { get; set; }
    public IEnumerable<long> SendToMeFriendRequestIds { get; set; }
    public IEnumerable<long> SendByMeFriendRequestIds { get; set; }
    public bool IsSuccess { get; set; }

    public SM32ResponseStatusCode StatusCode { get; set; }
}
