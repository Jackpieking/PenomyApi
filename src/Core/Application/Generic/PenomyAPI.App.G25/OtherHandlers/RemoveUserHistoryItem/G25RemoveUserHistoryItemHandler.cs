using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G25.OtherHandlers.RemoveUserHistoryItem;

public class G25RemoveUserHistoryItemHandler
    : IFeatureHandler<G25RemoveUserHistoryItemRequest, G25RemoveUserHistoryItemReponse>
{
    private readonly IG25Repository _g25Repository;

    public G25RemoveUserHistoryItemHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g25Repository = unitOfWork.Value.G25Repository;
    }

    public async Task<G25RemoveUserHistoryItemReponse> ExecuteAsync(
        G25RemoveUserHistoryItemRequest request,
        CancellationToken ct)
    {
        var removeSuccess = await _g25Repository.RemoveUserViewHistoryItemAsync(
            request.UserId,
            request.ArtworkId,
            ct);

        if (removeSuccess)
        {
            return G25RemoveUserHistoryItemReponse.SUCCESS;
        }

        return G25RemoveUserHistoryItemReponse.DATABASE_ERROR;
    }
}
