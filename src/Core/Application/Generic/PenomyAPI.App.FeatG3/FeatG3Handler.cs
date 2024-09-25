using PenomyAPI.App.Common;
using PenomyAPI.App.FeatG3.Infrastructures;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG3;

public class FeatG3Handler : IFeatureHandler<FeatG3Request, FeatG3Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IFeatG3MailHandler> _featG3MailHandler;

    public FeatG3Handler(Lazy<IUnitOfWork> unitOfWork, Lazy<IFeatG3MailHandler> featG3MailHandler)
    {
        _unitOfWork = unitOfWork;
        _featG3MailHandler = featG3MailHandler;
    }

    public async Task<FeatG3Response> ExecuteAsync(FeatG3Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.FeatG3Repository.GetRecommendedRecentlyUpdatedComicsAsync();

        return new FeatG3Response
        {
            Result = result,
            StatusCode = FeatG3ResponseStatusCode.SUCCESS
        };
    }
}
