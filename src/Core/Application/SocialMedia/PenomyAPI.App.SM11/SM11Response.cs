using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM11;

public class SM11Response : IFeatureResponse
{
    public List<GroupPost> GroupPosts { get; set; }
    public bool IsSuccess { get; set; }

    public SM11ResponseStatusCode StatusCode { get; set; }
}
