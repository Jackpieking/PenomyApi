using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt10;

public class Art10Handler : IFeatureHandler<Art10Request, Art10Response>
{
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly IArt10Repository _art10Repository;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;

    public Art10Handler(Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
    }


    public async Task<Art10Response> ExecuteAsync(Art10Request request, CancellationToken ct)
    {
        return new Art10Response();
    }
}
