using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG1;

public sealed class G1Response : IFeatureResponse
{
    public string PreRegistrationToken { get; init; }

    public string ConfirmedEmail { get; init; }

    public string PreGenNickName { get; init; }

    public G1ResponseStatusCode StatusCode { get; init; }
}
