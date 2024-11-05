namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.Common;

public sealed class G35StateBag
{
    /// <summary>
    ///     Check if the request is authorized to resolve.
    /// </summary>
    public bool IsAuthorized { get; set; }

    /// <summary>
    ///     The userId extracted from the access-token
    ///     when the request is authorized.
    /// </summary>
    public long UserId { get; set; }

    public string RefreshTokenId { get; set; }
}
