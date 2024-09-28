using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG8
{
    public class G8Handler : IFeatureHandler<G8Request, G8Response>
    {
        private readonly IG8Repository _g8Repository;

        public G8Handler(Lazy<IUnitOfWork> unitOfWork)
        {
            _g8Repository = unitOfWork.Value.G8Repository;
        }

        public async Task<G8Response> ExecuteAsync(G8Request request, CancellationToken ct)
        {
            List<ArtworkChapter> chapters = [];
            try
            {
                if (request.Id == 0 || request.StartPage <= 0)
                {
                    return new G8Response { StatusCode = G8ResponseStatusCode.INVALID_REQUEST };
                }
                chapters = await _g8Repository.GetArtWorkChapterById(
                    request.Id,
                    request.StartPage,
                    request.PageSize
                );
            }
            catch (Exception)
            {
                return new G8Response { StatusCode = G8ResponseStatusCode.FAILED };
            }
            return new G8Response { Result = chapters, StatusCode = G8ResponseStatusCode.SUCCESS };
        }
    }
}
