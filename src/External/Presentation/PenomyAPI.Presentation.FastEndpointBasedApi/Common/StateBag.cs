using PenomyAPI.Presentation.FastEndpointBasedApi.Common.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Common;

public sealed class StateBag
{
    public AuthRequest AppRequest { get; set; } = new();
}
