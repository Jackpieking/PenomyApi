using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G45.OtherHandlers.CountArtwork
{
    public class G45CountArtworkHandler : IFeatureHandler<G45CountArtworkRequest, G45CountArtworkResponse>
    {
        private readonly IG45Repository _G45Repository;

        public G45CountArtworkHandler(Lazy<IUnitOfWork> unitOfWork)
        {
            _G45Repository = unitOfWork.Value.G45Repository;
        }

        public async Task<G45CountArtworkResponse> ExecuteAsync(G45CountArtworkRequest request, CancellationToken ct)
        {
            int totalArtwork = await _G45Repository.GetTotalOfArtworksByTypeAndUserIdAsync(
                    request.UserId,
                    request.ArtworkType, ct);

            return new G45CountArtworkResponse
            {
                TotalArtwork = totalArtwork
            };
        }
    }
}
