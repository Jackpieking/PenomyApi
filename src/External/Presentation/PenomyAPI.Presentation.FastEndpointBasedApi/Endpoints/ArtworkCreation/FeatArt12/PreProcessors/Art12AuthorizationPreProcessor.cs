using FastEndpoints;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.PreProcessors;

public sealed class Art12AuthorizationPreProcessor : PreProcessor<Art12RequestDto, object>
{
    public override Task PreProcessAsync(
        IPreProcessorContext<Art12RequestDto> context,
        object state,
        CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}
