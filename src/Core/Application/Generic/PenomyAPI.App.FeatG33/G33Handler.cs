using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG33;

public sealed class G33Handler : IFeatureHandler<G33Request, G33Response>
{
    private readonly IG33Repository _repository;

    public G33Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _repository = unitOfWork.Value.G33Repository;
    }

    public async Task<G33Response> ExecuteAsync(G33Request request, CancellationToken ct)
    {
        // Attempt to remove refresh token by its id.
        var dbResult = await _repository.RemoveRefreshTokenAsync(request.GetRefreshTokenId(), ct);

        if (!dbResult)
        {
            return new() { StatusCode = G33ResponseStatusCode.DATABASE_ERROR };
        }

        return new() { StatusCode = G33ResponseStatusCode.SUCCESS };
    }
}
