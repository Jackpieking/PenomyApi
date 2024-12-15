using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM23;

public class SM23Response : IFeatureResponse
{
    public List<UserPostComment> UserPostComments { get; set; }
    public List<GroupPostComment> GroupPostComments { get; set; }

    public bool IsSuccess { get; set; }

    public SM23ResponseStatusCode StatusCode { get; set; }
}
