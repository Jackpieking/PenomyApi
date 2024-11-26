using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM41;

public class SM41Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SM41ResponseStatusCode StatusCode { get; set; }
}
