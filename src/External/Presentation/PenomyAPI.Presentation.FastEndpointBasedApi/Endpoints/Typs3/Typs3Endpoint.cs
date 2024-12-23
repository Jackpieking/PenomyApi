using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Persist.Typesense.AppSchema;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs3.Middlewares.Validation;
using Typesense;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs3;

public class Typs3Endpoint : Endpoint<Typs3HttpRequest, Typs3HttpResponse>
{
    private readonly ITypesenseClient _typesenseClient;

    public Typs3Endpoint(ITypesenseClient typesenseClient)
    {
        _typesenseClient = typesenseClient;
    }

    public override void Configure()
    {
        Get("/typs3");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<Typs3ValidationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for searching manga feature";
            summary.Description = "This endpoint is used for searching manga purpose.";
            summary.ExampleRequest = new() { SearchText = "string" };
            summary.Response<Typs3HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { }
            );
        });
    }

    public override async Task<Typs3HttpResponse> ExecuteAsync(
        Typs3HttpRequest req,
        CancellationToken ct
    )
    {
        var query = new SearchParameters(
            req.SearchText,
            $"{MangaSearchSchema.Metadata.FieldTitle.MangaName},{MangaSearchSchema.Metadata.FieldTitle.MangaDescription},{MangaSearchSchema.Metadata.FieldTitle.Embedding}"
        );

        query.ExcludeFields = MangaSearchSchema.Metadata.FieldTitle.Embedding;
        query.SortBy = "_text_match:desc";
        query.PerPage = 8;

        var searchResult = await _typesenseClient.Search<MangaSearchSchema>(
            MangaSearchSchema.Metadata.SchemaName,
            query,
            ct
        );

        var mangas = new List<Typs3HttpResponse.MangaSearchResult>();

        foreach (var result in searchResult.Hits)
        {
            mangas.Add(
                new()
                {
                    MangaId = result.Document.MangaId,
                    MangaName = result.Document.MangaName,
                    MangaAvatar = result.Document.MangaAvatar,
                    ArtworkType = result.Document.ArtworkType,
                    MangaNumberOfStars = result.Document.MangaNumberOfStars,
                    MangaNumberOfFollowers = result.Document.MangaNumberOfFollowers,
                }
            );
        }

        var response = new Typs3HttpResponse() { Body = mangas };

        await SendAsync(response, StatusCodes.Status200OK, ct);

        return response;
    }
}
