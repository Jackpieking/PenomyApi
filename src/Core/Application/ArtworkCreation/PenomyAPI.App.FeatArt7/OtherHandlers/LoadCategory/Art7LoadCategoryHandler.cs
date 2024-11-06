using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadCategory;

public class Art7LoadCategoryHandler
    : IFeatureHandler<Art7LoadCategoryRequest, Art7LoadCategoryResponse>
{
    private readonly IArt4Repository _art4Repository;

    public Art7LoadCategoryHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _art4Repository = unitOfWork.Value.Art4Repository;
    }

    public async Task<Art7LoadCategoryResponse> ExecuteAsync(
        Art7LoadCategoryRequest request,
        CancellationToken ct
    )
    {
        var categories = await _art4Repository.GetAllCategoriesAsync(ct);

        return new Art7LoadCategoryResponse { Categories = categories, };
    }
}
