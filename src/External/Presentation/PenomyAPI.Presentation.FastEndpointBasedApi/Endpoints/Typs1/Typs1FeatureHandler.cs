using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Infra.Configuration.Options;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Typesense;
using PenomyAPI.Persist.Typesense.AppSchema;
using Typesense;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs1;

/// <summary>
///     Non functional feat checking collection
/// </summary>
public static class Typs1FeatureHandler
{
    private static AppDbContext _context;
    private static ITypesenseClient _typesenseClient;

    public static void LoadRequiredDependencies(
        AppDbContext context,
        ITypesenseClient typesenseClient
    )
    {
        _context = context;
        _typesenseClient = typesenseClient;
    }

    public static async Task ExecuteAsync(IConfiguration configuration, CancellationToken ct)
    {
        // Init all required collection schemas
        var schemaInfos = TypesenseInitializer.InitCollectionSchemas(
            configuration.GetRequiredSection("Typesense").Get<TypesenseOptions>()
        );

        // Uncomment this incase you want to re init all collections.
        // var errors = await TypesenseInitializer.DeleteAllCollectionsAsync(
        //     schemaInfos,
        //     _typesenseClient,
        //     ct
        // );

        // Check if all required collections exist or not.
        var missingCollectionSchemaNames =
            await TypesenseInitializer.AreRequiredCollectionsExistAsync(
                schemaInfos,
                _typesenseClient,
                ct
            );

        // fail if
        //      - error is exist (not empty)
        //      - there are missing collections (just some)
        if (
            missingCollectionSchemaNames.Count < schemaInfos.Count
            && missingCollectionSchemaNames.Count > 0
        )
        {
            throw new ApplicationException(
                @$"Cannot verify typesense.
                Because of missing collections
                Please contact admin or group leader to fix this issue."
            );
        }

        // If
        //      - there are no missing collections
        //      - there is no error
        //  ===> All collections have been exist
        if (missingCollectionSchemaNames.Count == 0)
        {
            return;
        }

        // Init all collections if all collections are missing
        var initCollectionError = await TypesenseInitializer.InitAllCollectionsAsync(
            schemaInfos,
            _typesenseClient,
            ct
        );

        // Something wrong is happened since there is error
        if (!string.IsNullOrWhiteSpace(initCollectionError))
        {
            throw new ApplicationException(initCollectionError);
        }

        // Start loading data into each collection
        #region Manga Search Collection
        // Get all mangas from postgres and generating manga search documents
        var mangaDocuments = _context
            .Set<Artwork>()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(artwork =>
                // Check if current artwork is public for everyone and not being removed or taken down.
                artwork.PublicLevel == ArtworkPublicLevel.Everyone
                && !artwork.IsTemporarilyRemoved
                && !artwork.IsTakenDown
            )
            .Select(artWork => new MangaSearchSchema
            {
                MangaId = artWork.Id.ToString(),
                MangaName = artWork.Title,
                MangaDescription = artWork.Introduction,
                MangaCategories = artWork.ArtworkCategories.Select(artworkCat =>
                    artworkCat.Category.Name
                ),
                MangaAvatar = artWork.ThumbnailUrl,
                MangaNumberOfFollowers = artWork.ArtworkMetaData.TotalFollowers,
                MangaNumberOfStars = artWork.ArtworkMetaData.TotalStarRates
            });

        var importErrors = await TypesenseInitializer.ImportDataIntoMangaSearchCollectionAsync(
            _typesenseClient,
            mangaDocuments,
            ct
        );

        if (importErrors.Count != 0)
        {
            throw new ApplicationException(
                @$"Cannot import data into manga search collection because of these errors:
                {importErrors}"
            );
        }

        #endregion
    }
}
