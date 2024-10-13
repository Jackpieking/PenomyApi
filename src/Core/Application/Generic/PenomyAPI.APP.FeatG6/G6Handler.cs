using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.APP.FeatG6
{
    public class G6Handler : IFeatureHandler<G6Request, G6Response>
    {
        private readonly IG6Repository _IG6Repository;

        public G6Handler(Lazy<IUnitOfWork> unitOfWork)
        {
            _IG6Repository = unitOfWork.Value.G6Repository;
        }

        public async Task<G6Response> ExecuteAsync(G6Request request, CancellationToken ct)
        {
            List<Artwork> result = [];
            try
            {
                if (request.Top == 0)
                {
                    return new()
                    {
                        StatusCode = G6ResponseStatusCode.INVALID_REQUEST,
                        IsSuccess = false
                    };
                }
                result = await _IG6Repository.GetRecommendedArtworksAsync(request.Top);
            }
            catch
            {
                return new() { StatusCode = G6ResponseStatusCode.FAILED, IsSuccess = false };
            }
            return new()
            {
                Result = result,
                IsSuccess = true,
                StatusCode = G6ResponseStatusCode.SUCCESS
            };
        }
    }
}
