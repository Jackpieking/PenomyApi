using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM8;

public class SM8Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public long Result { get; set; }
    public SM8ResponseStatusCode StatusCode { get; set; }
    public string[] Message { get; set; }
}
