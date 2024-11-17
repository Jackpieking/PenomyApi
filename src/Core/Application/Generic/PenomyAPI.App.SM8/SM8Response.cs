using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.App.SM8;

public class SM8Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public long Result { get; set; }
    public SM8ResponseStatusCode StatusCode { get; set; }
    public string[] Message { get; set; }
}
