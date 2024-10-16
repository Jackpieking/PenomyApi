using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.G13;

public class G13Handler : IFeatureHandler<G13Request, G13Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G13Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G13Response> ExecuteAsync(G13Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G13Repository.GetRecentlyUpdatedAnimesAsync();
        return new G13Response { ArtworkList = result, StatusCode = G13ResponseStatusCode.SUCCESS };
    }
}
