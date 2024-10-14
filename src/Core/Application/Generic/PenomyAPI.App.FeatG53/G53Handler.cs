using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG53;

public class G53Handler : IFeatureHandler<G53Request, G53Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G53Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G53Response> ExecuteAsync(G53Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G53Repository.EditCommentAsync(
            request.CommentId,
            request.NewComment
        );

        if (result == true)
        {
            return new G53Response { IsSuccess = true, StatusCode = G53ResponseStatusCode.SUCCESS };
        }
        else
        {
            return new G53Response
            {
                IsSuccess = false,
                StatusCode = G53ResponseStatusCode.DATABASE_ERROR
            };
        }
    }
}
