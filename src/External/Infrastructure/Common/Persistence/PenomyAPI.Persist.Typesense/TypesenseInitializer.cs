using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Persist.Typesense.AppSchema;
using Typesense;

namespace PenomyAPI.Persist.Typesense;

public static class TypesenseInitializer
{
    public static async Task<List<string>> AreRequiredCollectionsExistAsync(
        FrozenDictionary<string, Schema> schemaInfos,
        ITypesenseClient client,
        CancellationToken ct
    )
    {
        // Init list of missing collection schema names
        var missingCollectionSchemaNames = new List<string>();

        foreach (var schemaInfo in schemaInfos)
        {
            try
            {
                // is manga search collection exists?
                await client.RetrieveCollection(schemaInfo.Key, ct);
            }
            catch (TypesenseApiNotFoundException e)
            {
                missingCollectionSchemaNames.Add(schemaInfo.Key);
            }
        }

        return missingCollectionSchemaNames;
    }

    public static async Task<string> InitAllCollectionsAsync(
        FrozenDictionary<string, Schema> schemaInfos,
        ITypesenseClient client,
        CancellationToken ct
    )
    {
        try
        {
            // Init all collections
            foreach (var schemaInfo in schemaInfos)
            {
                await client.CreateCollection(schemaInfo.Value);
            }

            // Init all collections successfully
            return string.Empty;
        }
        catch (TypesenseApiException e)
        {
            return e.Message;
        }
    }

    public static async Task<List<string>> ImportDataIntoMangaSearchCollectionAsync(
        ITypesenseClient client,
        IEnumerable<MangaSearchSchema> documents,
        CancellationToken ct
    )
    {
        try
        {
            var result = await client.ImportDocuments(
                MangaSearchSchema.Metadata.SchemaName,
                documents,
                40,
                ImportType.Upsert
            );

            if (!result.All(resultItem => resultItem.Success == true))
            {
                return result.Select(resultItem => resultItem.Error).ToList();
            }

            return [];
        }
        catch (TypesenseApiException e)
        {
            return [e.Message];
        }
    }

    public static FrozenDictionary<string, Schema> InitCollectionSchemas()
    {
        // Init list of schema infos
        var schemaInfos = new Dictionary<string, Schema>
        {
            {
                MangaSearchSchema.Metadata.SchemaName,
                new(
                    MangaSearchSchema.Metadata.SchemaName,
                    [
                        new(MangaSearchSchema.Metadata.FieldTitle.MangaId, FieldType.String, false),
                        new(
                            MangaSearchSchema.Metadata.FieldTitle.MangaName,
                            FieldType.String,
                            false
                        ),
                        new(
                            MangaSearchSchema.Metadata.FieldTitle.MangaAvatar,
                            FieldType.String,
                            false
                        ),
                        new(
                            MangaSearchSchema.Metadata.FieldTitle.MangaNumberOfStars,
                            FieldType.Int64,
                            false
                        ),
                        new(
                            MangaSearchSchema.Metadata.FieldTitle.MangaNumberOfFollowers,
                            FieldType.Int64,
                            false
                        )
                    ]
                )
            }
        };

        return schemaInfos.ToFrozenDictionary();
    }

    public static async Task<List<string>> DeleteAllCollectionsAsync(
        FrozenDictionary<string, Schema> schemaInfos,
        ITypesenseClient client,
        CancellationToken ct
    )
    {
        var errors = new List<string>();

        foreach (var schemaInfo in schemaInfos)
        {
            try
            {
                await client.DeleteCollection(schemaInfo.Key);
            }
            catch (TypesenseApiException e)
            {
                errors.Add(e.Message);
            }
        }

        return errors;
    }
}
