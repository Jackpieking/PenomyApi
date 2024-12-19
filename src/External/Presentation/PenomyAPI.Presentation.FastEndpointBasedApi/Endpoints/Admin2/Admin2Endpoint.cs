using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin2.Middlewares.Authorization;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin2.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin2;

public class Admin2Endpoint : Endpoint<Admin2HttpRequest, Admin2HttpResponse>
{
    private readonly AppDbContext _context;

    public Admin2Endpoint(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/admin2");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<Admin2ValidationPreProcessor>();
        PreProcessor<Admin2AuthorizationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for get category detail in admin page.";
            summary.Description = "This endpoint is used for get category detail in admin page.";
        });
    }

    public override async Task<Admin2HttpResponse> ExecuteAsync(
        Admin2HttpRequest req,
        CancellationToken ct
    )
    {
        var categoryDetail = await GetCategoryByPageAsync(req.CategoryId, ct);

        if (Equals(categoryDetail, null))
        {
            await SendNotFoundAsync(ct);
        }

        var httpResponse = MapToHttpResponse(categoryDetail);

        await SendAsync(httpResponse, StatusCodes.Status200OK, ct);

        return httpResponse;
    }

    private Task<Category> GetCategoryByPageAsync(long categoryId, CancellationToken ct)
    {
        var categories = _context
            .Set<Category>()
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Where(category => category.Id == categoryId)
            .Select(category => new Category
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                UpdatedAt = category.UpdatedAt,
                UpdatedBy = category.UpdatedBy
            })
            .FirstOrDefaultAsync(ct);

        return categories;
    }

    private static Admin2HttpResponse MapToHttpResponse(Category category)
    {
        var localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        return new()
        {
            Body = new()
            {
                CategoryDetail = new()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Description = category.Description,
                    UpdatedBy = category.UpdatedBy.ToString(),
                    UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(category.UpdatedAt, localTimeZone)
                }
            }
        };
    }
}
