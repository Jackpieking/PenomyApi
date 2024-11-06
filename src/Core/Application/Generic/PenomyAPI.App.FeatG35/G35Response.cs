using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.App.FeatG35;

public sealed class G35Response : IFeatureResponse
{
    public UserProfile UserProfile { get; set; }
}
