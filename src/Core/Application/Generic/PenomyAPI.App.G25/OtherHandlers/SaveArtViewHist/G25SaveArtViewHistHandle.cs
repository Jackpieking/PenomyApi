using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.G25.OtherHandlers.SaveArtViewHist
{
    public class G25SaveArtViewHistHandle : IFeatureHandler<G25SaveArtViewHistRequest, G25SaveArtViewHistResponse>
    {
        private readonly IG25Repository _g25Repository;

        public G25SaveArtViewHistHandle(
            Lazy<IUnitOfWork> unitOfWork
        )
        {
            _g25Repository = unitOfWork.Value.G25Repository;
        }

        public async Task<G25SaveArtViewHistResponse> ExecuteAsync(G25SaveArtViewHistRequest request, CancellationToken ct)
        {
            if (request.UserId == 0)
            {
                return new G25SaveArtViewHistResponse { StatusCode = G25SaveArtViewHistResponseStatusCode.DATABASE_ERROR };
            }

            await _g25Repository.AddArtworkViewHist(request.UserId, request.ArtworkId, request.ChapterId, request.ArtworkType, ct);

            return new G25SaveArtViewHistResponse
            {
                IsSuccess = true,
                StatusCode = G25SaveArtViewHistResponseStatusCode.SUCCESS
            };
        }
    }
}
