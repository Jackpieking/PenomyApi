using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadCategory;

public class Art4LoadCategoryHandler :
    IFeatureHandler<Art4LoadCategoryRequest, Art4LoadCategoryResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public Art4LoadCategoryHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art4LoadCategoryResponse> ExecuteAsync(Art4LoadCategoryRequest request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;

        try
        {
            var categories = await unitOfWork.FeatArt4Repository.GetAllCategoriesAsync(ct);

            return new Art4LoadCategoryResponse
            {
                IsSuccess = true,
                StatusCode = Art4LoadCategoryResponseStatusCode.Success,
                Categories = categories,
            };
        }
        catch (Exception)
        {
            return new Art4LoadCategoryResponse
            {
                IsSuccess = false,
                StatusCode = Art4LoadCategoryResponseStatusCode.DATABASE_ERROR,
            };
        }
    }
}
