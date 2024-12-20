using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System.Collections.Generic;

namespace PenomyAPI.App.G63;

public class G63Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public ICollection<CreatorProfile> Result { get; set; }
    public G63ResponseStatusCode StatusCode { get; set; }
}
