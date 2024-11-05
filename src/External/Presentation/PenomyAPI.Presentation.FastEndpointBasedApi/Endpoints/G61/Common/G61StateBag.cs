using PenomyAPI.App.G61;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.Common;

public sealed class G61StateBag
{
    public G61Request AppRequest { get; set; } = new();
}
