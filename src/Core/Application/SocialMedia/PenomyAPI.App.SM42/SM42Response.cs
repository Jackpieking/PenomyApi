using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM42;

public class SM42Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public List<SocialGroupJoinRequest> RequestList { get; set; }
    public SM42ResponseStatusCode StatusCode { get; set; }
}
