using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM9;

public class SM9Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public List<SocialGroup> Result { get; set; }
    public SM9ResponseStatusCode StatusCode { get; set; }
    public string[] Message { get; set; }
}
