using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G48.OtherHandlers.CountArtwork
{
    public class G48CountArtworkHandler : IFeatureHandler<G48CountArtworkRequest, G48CountArtworkResponse>
    {
        private readonly IG48Repository _g48Repository;

        public G48CountArtworkHandler(Lazy<IUnitOfWork> unitOfWork)
        {
            _g48Repository = unitOfWork.Value.G48Repository;
        }

        public async Task<G48CountArtworkResponse> ExecuteAsync(G48CountArtworkRequest request, CancellationToken ct)
        {
            int totalArtwork = await _g48Repository.GetTotalOfArtworksByTypeAndUserIdAsync(
                    request.UserId,
                    request.ArtworkType, ct);

            return new G48CountArtworkResponse
            {
                TotalArtwork = totalArtwork
            };
        }
    }
}
