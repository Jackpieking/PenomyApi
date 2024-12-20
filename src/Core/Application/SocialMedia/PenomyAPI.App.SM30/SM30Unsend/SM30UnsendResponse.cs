using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM30.SM30UnsendHandler;

public class SM30UnsendResponse : IFeatureResponse
{
    private bool IsSuccess { get; set; }

    public SM30UnsendResponseStatusCode StatusCode { get; set; }
}
