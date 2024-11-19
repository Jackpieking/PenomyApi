using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG21;

public class G21Handler : IFeatureHandler<G21Request, G21Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G21Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G21Response> ExecuteAsync(G21Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G21Repository.GetCommentsAsync(
            request.ChapterId,
            request.UserId
        );

        return new G21Response { Result = result, StatusCode = G21ResponseStatusCode.SUCCESS };
    }
}
