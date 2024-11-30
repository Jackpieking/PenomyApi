using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM39;

public class SM39Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public IEnumerable<SocialGroupMember> Members { get; set; }
    public SM39ResponseStatusCode StatusCode { get; set; }
}
