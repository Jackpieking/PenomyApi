﻿using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG45;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G45;

public class G45Handler : IFeatureHandler<G45Request, G45Response>
{
    private readonly IG45Repository _G45Repository;

    public G45Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _G45Repository = unitOfWork.Value.G45Repository;
    }

    public async Task<G45Response> ExecuteAsync(G45Request request, CancellationToken ct)
    {
        try
        {
            List<G45FollowedArtworkReadModel> artworks = await _G45Repository
                    .GetFollowedArtworksByTypeAndUserIdWithPaginationAsync(
                        request.UserId,
                        request.ArtworkType,
                        request.PageNum,
                        request.ArtNum,
                        ct
                    );

            return new G45Response
            {
                IsSuccess = true,
                Result = artworks,
                StatusCode = G45ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return new G45Response { IsSuccess = false, StatusCode = G45ResponseStatusCode.INVALID_REQUEST };
        }
    }
}
