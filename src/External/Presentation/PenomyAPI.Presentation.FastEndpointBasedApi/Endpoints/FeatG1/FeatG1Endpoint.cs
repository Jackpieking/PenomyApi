using FastEndpoints;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PenomyAPI.App.FeatG1;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.DTOs;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1;

public class FeatG1Endpoint : Endpoint<G1RequestDto>
{
    private readonly string _registerMailTemplatePath;

    public FeatG1Endpoint(IWebHostEnvironment webHostEnvironment)
    {
         _registerMailTemplatePath = webHostEnvironment.WebRootPath + "\\register_mail_template.html";
    }

    public override void Configure()
    {
        Post("g1/register");

        AllowAnonymous();
    }

    public override async Task<object> ExecuteAsync(G1RequestDto requestDto, CancellationToken ct)
    {
        var mailTemplate = await File.ReadAllTextAsync(_registerMailTemplatePath);

        var request = new G1Request
        {
            RegisterPageLink = "http://localhost:9000/auth/confirm-register",
            Email = requestDto.Email,
            MailTemplate = mailTemplate,
        };

        var featureResponse = await FeatureExtensions.ExecuteAsync<G1Request, G1Response>(request, ct);

        return Results.Ok();
    }
}
