using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G63;

public class G63Handler : IFeatureHandler<G63Request, G63Response>
{
    private readonly IG63Repository _G63Repository;

    public G63Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _G63Repository = unitOfWork.Value.G63Repository;
    }

    public async Task<G63Response> ExecuteAsync(G63Request request, CancellationToken ct)
    {
        try
        {
            ICollection<CreatorProfile> creators = await _G63Repository
                .GetFollowedCreatorsByUserIdWithPaginationAsync(
                    request.UserId,
                    request.PageNum,
                    request.CreatorNum,
                    ct
                    );

            return new G63Response
            {
                IsSuccess = true,
                Result = creators,
                StatusCode = G63ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return new G63Response
            {
                IsSuccess = false,
                StatusCode = G63ResponseStatusCode.FAILED
            };
        }
    }
}
