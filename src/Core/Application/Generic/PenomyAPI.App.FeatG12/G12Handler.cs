using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG12;

public class G12Handler : IFeatureHandler<G12Request, G12Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G12Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G12Response> ExecuteAsync(G12Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G12Repository.GetAnimesByCategoryAsync(request.CategoryId);

        return new G12Response { Result = result, StatusCode = G12ResponseStatusCode.SUCCESS };
    }
}
