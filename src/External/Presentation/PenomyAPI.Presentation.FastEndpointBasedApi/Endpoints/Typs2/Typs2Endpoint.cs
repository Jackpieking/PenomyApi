using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Persist.Typesense.AppSchema;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs2.Middlewares.Validation;
using Typesense;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs2;

public class Typs2Endpoint : Endpoint<Typs2HttpRequest, Typs2HttpResponse>
{
    private readonly ITypesenseClient _typesenseClient;

    public Typs2Endpoint(ITypesenseClient typesenseClient)
    {
        _typesenseClient = typesenseClient;
    }

    public override void Configure()
    {
        Get("/typs2");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<Typs2ValidationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for searching manga with detail feature";
            summary.Description = "This endpoint is used for searching manga with detail purpose.";
            summary.ExampleRequest = new() { SearchText = "string" };
        });
    }

    public override async Task<Typs2HttpResponse> ExecuteAsync(
        Typs2HttpRequest req,
        CancellationToken ct
    )
    {
        var query = new SearchParameters(
            req.SearchText,
            $"{MangaSearchSchema.Metadata.FieldTitle.MangaName},{MangaSearchSchema.Metadata.FieldTitle.MangaDescription},{MangaSearchSchema.Metadata.FieldTitle.Embedding}"
        );

        query.ExcludeFields = MangaSearchSchema.Metadata.FieldTitle.Embedding;
        query.SortBy = "_text_match:desc";
        query.PerPage = 10;

        var searchResult = await _typesenseClient.Search<MangaSearchSchema>(
            MangaSearchSchema.Metadata.SchemaName,
            query,
            ct
        );

        var mangas = new List<Typs2HttpResponse.MangaSearchResult>();

        foreach (var result in searchResult.Hits)
        {
            mangas.Add(
                new()
                {
                    MangaId = result.Document.MangaId,
                    MangaName = result.Document.MangaName,
                    MangaAvatar = result.Document.MangaAvatar,
                    MangaDescription = result.Document.MangaDescription,
                    MangaNumberOfStars = result.Document.MangaNumberOfStars,
                    MangaNumberOfFollowers = result.Document.MangaNumberOfFollowers,
                }
            );
        }

        var response = new Typs2HttpResponse() { Body = mangas };

        await SendAsync(response, StatusCodes.Status200OK, ct);

        return response;
    }
}
