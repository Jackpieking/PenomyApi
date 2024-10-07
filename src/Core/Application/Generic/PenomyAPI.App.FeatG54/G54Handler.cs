using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG54;

public class G54Handler : IFeatureHandler<G54Request, G54Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G54Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G54Response> ExecuteAsync(G54Request request, CancellationToken ct)
    {

        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G54Repository.RemoveCommentAsync(request.ArtworkCommentId);

        if (result == true)
        {
            return new G54Response
            {
                IsSuccess = result,
                StatusCode = G54ResponseStatusCode.SUCCESS
            };
        }
        else
        {
            return new G54Response
            {
                IsSuccess = result,
                StatusCode = G54ResponseStatusCode.DATABASE_ERROR
            };
        }
    }
}
