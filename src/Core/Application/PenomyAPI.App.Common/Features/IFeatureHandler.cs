using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.Common;

public interface IFeatureHandler<TRequest, TResponse>
    where TRequest : IFeatureRequest<TResponse>
    where TResponse : IFeatureResponse
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken ct);
}
