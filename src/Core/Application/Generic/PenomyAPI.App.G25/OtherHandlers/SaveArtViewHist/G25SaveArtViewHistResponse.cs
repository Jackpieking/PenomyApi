using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.SaveArtViewHist;

public class G25SaveArtViewHistResponse : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public G25ResponseStatusCode StatusCode { get; set; }

    public static readonly G25SaveArtViewHistResponse FAILED = new()
    {
        IsSuccess = false,
        StatusCode = G25ResponseStatusCode.FAILED,
    };

    public static readonly G25SaveArtViewHistResponse SUCCESS = new()
    {
        IsSuccess = true,
        StatusCode = G25ResponseStatusCode.SUCCESS,
    };
}
