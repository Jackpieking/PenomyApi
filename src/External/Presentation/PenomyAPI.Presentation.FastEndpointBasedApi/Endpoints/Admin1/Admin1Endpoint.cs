using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin1.Middlewares.Authorization;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin1.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin1;

public class Admin1Endpoint : Endpoint<Admin1HttpRequest, Admin1HttpResponse>
{
    private readonly AppDbContext _context;

    public Admin1Endpoint(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/admin1");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<Admin1ValidationPreProcessor>();
        PreProcessor<Admin1AuthorizationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for get all categories in admin page.";
            summary.Description = "This endpoint is used for get all categories in admin page.";
        });
    }

    public override async Task<Admin1HttpResponse> ExecuteAsync(
        Admin1HttpRequest req,
        CancellationToken ct
    )
    {
        var categories = await GetCategoryByPageAsync(
            req.CurrentCategoryId,
            req.NumberOfCategoriesToTake,
            ct
        );

        var httpResponse = MapToHttpResponse(categories);

        await SendAsync(httpResponse, StatusCodes.Status200OK, ct);

        return httpResponse;
    }

    private async Task<List<Category>> GetCategoryByPageAsync(
        long currentCategoryId,
        int numberOfCategoriesToTake,
        CancellationToken ct
    )
    {
        if (currentCategoryId != 0)
        {
            var isCategoryFound = await _context
                .Set<Category>()
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(category => category.Id == currentCategoryId, ct);

            if (!isCategoryFound)
            {
                return [];
            }
        }

        var categories = await _context
            .Set<Category>()
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .OrderBy(category => category.Id)
            .Where(category => category.Id > currentCategoryId)
            .Select(category => new Category
            {
                Id = category.Id,
                Name = category.Name,
                UpdatedAt = category.UpdatedAt,
                UpdatedBy = category.UpdatedBy,
            })
            .Take(numberOfCategoriesToTake)
            .ToListAsync(ct);

        return categories;
    }

    private static Admin1HttpResponse MapToHttpResponse(List<Category> categories)
    {
        var localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        return new()
        {
            Body = new()
            {
                TotalCategories = categories.Count,
                Categories = categories.Select(
                    category => new Admin1HttpResponse.BodyDto.CategoryDto
                    {
                        Id = category.Id.ToString(),
                        Name = category.Name,
                        UpdatedBy = category.UpdatedBy.ToString(),
                        UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(
                            category.UpdatedAt,
                            localTimeZone
                        )
                    }
                )
            }
        };
    }
}
