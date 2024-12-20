using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G63.OtherHandlers.CountArtwork
{
    public class G63CountArtworkHandler : IFeatureHandler<G63CountArtworkRequest, G63CountArtworkResponse>
    {
        private readonly IG63Repository _G63Repository;

        public G63CountArtworkHandler(Lazy<IUnitOfWork> unitOfWork)
        {
            _G63Repository = unitOfWork.Value.G63Repository;
        }

        public async Task<G63CountArtworkResponse> ExecuteAsync(G63CountArtworkRequest request, CancellationToken ct)
        {
            int totalArtwork = await _G63Repository.GetTotalOfCreatorByUserIdAsync(
                    request.UserId,
                    cancellationToken: ct);

            return new G63CountArtworkResponse
            {
                TotalArtwork = totalArtwork
            };
        }
    }
}
