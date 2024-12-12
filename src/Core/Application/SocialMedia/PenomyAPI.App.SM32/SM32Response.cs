using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.App.SM32;

public class SM32Response : IFeatureResponse
{
    public IEnumerable<UserProfile> UserProfiles { get; set; }
    public bool IsSuccess { get; set; }

    public SM32ResponseStatusCode StatusCode { get; set; }
}
