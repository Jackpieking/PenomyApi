using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadCategory;

public class Art4LoadCategoryHandler
    : IFeatureHandler<Art4LoadCategoryRequest, Art4LoadCategoryResponse>
{
    private readonly IArt4Repository _art4Repository;

    public Art4LoadCategoryHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _art4Repository = unitOfWork.Value.Art4Repository;
    }

    public async Task<Art4LoadCategoryResponse> ExecuteAsync(
        Art4LoadCategoryRequest request,
        CancellationToken ct
    )
    {
        var categories = await _art4Repository.GetAllCategoriesAsync(ct);

        return new Art4LoadCategoryResponse { Categories = categories, };
    }
}
