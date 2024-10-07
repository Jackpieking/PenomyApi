using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG3;

public class FeatG3Handler : IFeatureHandler<FeatG3Request, FeatG3Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public FeatG3Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<FeatG3Response> ExecuteAsync(FeatG3Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.FeatG3Repository.GetRecentlyUpdatedComicsAsync();

        return new FeatG3Response
        {
            
            ArtworkList = result,
            StatusCode = FeatG3ResponseStatusCode.SUCCESS
        };
    }
}