using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin3.Middlewares.Authorization;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin3.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin3;

public class Admin3Endpoint : Endpoint<Admin3HttpRequest, Admin3HttpResponse>
{
    private readonly AppDbContext _context;

    public Admin3Endpoint(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Post("/Admin3");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<Admin3ValidationPreProcessor>();
        PreProcessor<Admin3AuthorizationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for update category detail in admin page.";
            summary.Description = "This endpoint is used for update category detail in admin page.";
        });
    }

    public override async Task<Admin3HttpResponse> ExecuteAsync(
        Admin3HttpRequest req,
        CancellationToken ct
    )
    {
        var foundCategory = await GetCategoryByIdAsync(req.Category.Id, ct);

        await UpdateCategoryDetailAsync(req, foundCategory, ct);

        var httpResponse = new Admin3HttpResponse();

        await SendAsync(httpResponse, StatusCodes.Status200OK, ct);

        return httpResponse;
    }

    private Task<Category> GetCategoryByIdAsync(long categoryId, CancellationToken ct)
    {
        return _context
            .Set<Category>()
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Where(category => category.Id == categoryId)
            .Select(category => new Category
            {
                Name = category.Name,
                Description = category.Description
            })
            .FirstOrDefaultAsync(ct);
    }

    private async Task UpdateCategoryDetailAsync(
        Admin3HttpRequest req,
        Category category,
        CancellationToken ct
    )
    {
        var newCategory = new Category { Id = req.Category.Id };
        var entry = _context.Attach(newCategory);

        if (!req.Category.Name.Equals(category.Name))
        {
            newCategory.Name = req.Category.Name;
            entry.Property(prop => prop.Name).IsModified = true;
        }

        if (!req.Category.Description.Equals(category.Description))
        {
            newCategory.Description = req.Category.Description;
            entry.Property(prop => prop.Description).IsModified = true;
        }

        newCategory.UpdatedAt = DateTime.UtcNow;
        entry.Property(prop => prop.UpdatedAt).IsModified = true;

        await _context.SaveChangesAsync(ct);
    }
}
