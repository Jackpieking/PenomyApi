using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM7.OtherHandlers.CountGroups;

public class SM7CountGroupsHandler : IFeatureHandler<SM7CountGroupsRequest, SM7CountGroupsResponse>
{
    private readonly ISM7Repository _SM7Repository;

    public SM7CountGroupsHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _SM7Repository = unitOfWork.Value.SM7Repository;
    }

    public async Task<SM7CountGroupsResponse> ExecuteAsync(SM7CountGroupsRequest request, CancellationToken ct)
    {
        int totalGroups = await _SM7Repository.GetTotalOfArtworksByTypeAndUserIdAsync(
                request.UserId,
                ct);

        return new SM7CountGroupsResponse
        {
            TotalGroups = totalGroups
        };
    }
}
