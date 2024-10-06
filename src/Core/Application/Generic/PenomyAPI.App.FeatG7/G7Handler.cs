using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG7;

public class G7Handler : IFeatureHandler<G7Request, G7Response>
{
    private readonly IG7Repository _g7Repository;

    public G7Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g7Repository = unitOfWork.Value.G7Repository;
    }

    public async Task<G7Response> ExecuteAsync(G7Request request, CancellationToken ct)
    {
        List<Artwork> result = [];
        if (request.Id == 0 || request.StartPage <= 0 || request.PageSize <= 0)
        {
            return new() { StatusCode = G7ResponseStatusCode.INVALID_REQUEST, IsSuccess = false };
        }
        if (!await _g7Repository.IsArtworkExistAsync(request.Id, ct))
        {
            return new() { StatusCode = G7ResponseStatusCode.NOT_FOUND, IsSuccess = false };
        }
        result = await _g7Repository.GetArkworkBySeriesAsync(
            request.Id,
            request.StartPage,
            request.PageSize,
            ct
        );

        return new()
        {
            Result = result,
            StatusCode = G7ResponseStatusCode.SUCCESS,
            IsSuccess = true
        };
    }
}
