using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

// Lưu ý namespace phải giống nhau
namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks;

public partial interface IUnitOfWork
{
    IFeatArt4Repository FeatArt4Repository { get; }
}