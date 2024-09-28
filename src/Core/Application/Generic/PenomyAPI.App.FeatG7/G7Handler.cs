using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG7
{
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
            try
            {
                if (request.Id == 0 || request.StartPage <= 0 || request.PageSize <= 0)
                {
                    return new G7Response { StatusCode = G7ResponseStatusCode.INVALID_REQUEST };
                }
                result = await _g7Repository.GetArkworkBySeriesAsync(
                    request.Id,
                    request.StartPage,
                    request.PageSize,
                    ct
                );
            }
            catch (Exception)
            {
                return new G7Response { StatusCode = G7ResponseStatusCode.FAILED };
            }
            return new G7Response { Result = result, StatusCode = G7ResponseStatusCode.SUCCESS };
        }
    }
}
