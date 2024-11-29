using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public partial class UnitOfWork
{
    private ISM6Repository _SM6Repository;

    public ISM6Repository SM6Repository
    {
        get
        {
            _SM6Repository ??= new SM6Repository(_dbContext);

            return _SM6Repository;
        }
    }
}
