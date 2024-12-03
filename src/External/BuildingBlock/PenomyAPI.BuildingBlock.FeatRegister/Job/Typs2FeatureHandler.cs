using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Typesense.AppSchema;
using Quartz;
using Typesense;

namespace PenomyAPI.BuildingBlock.FeatRegister.Job;

public sealed class Typs2FeatureHandler : IJob
{
    public const string JobKey = nameof(Typs2FeatureHandler);
    public const string TriggerKey = nameof(Typs2FeatureHandler);
    public const int RepeatAsIntervalInSeconds = 30;
    private readonly ITypesenseClient _typesenseClient;
    private readonly AppDbContext _context;

    public Typs2FeatureHandler(ITypesenseClient typesenseClient, AppDbContext context)
    {
        _typesenseClient = typesenseClient;
        _context = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var mangaDocuments = _context
            .Set<Artwork>()
            .AsNoTracking()
            .AsSplitQuery()
            .Select(artWork => new MangaSearchSchema
            {
                MangaId = artWork.Id.ToString(),
                MangaName = artWork.Title,
                MangaAvatar = artWork.ThumbnailUrl,
                MangaNumberOfFollowers = artWork.ArtworkMetaData.TotalFollowers,
                MangaNumberOfStars = artWork.ArtworkMetaData.TotalStarRates
            });

        await _typesenseClient.ImportDocuments(
            MangaSearchSchema.Metadata.SchemaName,
            mangaDocuments,
            40,
            ImportType.Upsert
        );
    }
}
