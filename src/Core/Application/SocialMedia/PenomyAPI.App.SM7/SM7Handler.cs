using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM7;

public class SM7Handler : IFeatureHandler<SM7Request, SM7Response>
{
    private readonly ISM7Repository _SM7Repository;

    public SM7Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _SM7Repository = unitOfWork.Value.SM7Repository;
    }

    public async Task<SM7Response> ExecuteAsync(SM7Request request, CancellationToken ct)
    {
        try
        {
            ICollection<SocialGroup> artworks = await _SM7Repository
                .GetJoinedGroupsByUserIdWithPaginationAsync(
                    request.UserId,
                    request.PageNum,
                    request.ArtNum,
                    ct
                    );

            return new SM7Response
            {
                IsSuccess = true,
                Result = artworks,
                StatusCode = SM7ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return new SM7Response
            {
                IsSuccess = false,
                StatusCode = SM7ResponseStatusCode.FAILED
            };
        }
    }
}
