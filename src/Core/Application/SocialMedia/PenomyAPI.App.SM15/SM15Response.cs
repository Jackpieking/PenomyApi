using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM15;

public class SM15Response : IFeatureResponse
{
    public List<UserPost> UserPosts { get; set; }
    private bool IsSuccess { get; set; }

    public SM15ResponseStatusCode StatusCode { get; set; }
}
