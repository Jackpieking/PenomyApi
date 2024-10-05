using PenomyAPI.App.Common;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;

public sealed class Art7LoadComicDetailHandler
    : IFeatureHandler<Art7LoadComicDetailRequest, Art7LoadComicDetailResponse>
{


    public Task<Art7LoadComicDetailResponse> ExecuteAsync(
        Art7LoadComicDetailRequest request,
        CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}
