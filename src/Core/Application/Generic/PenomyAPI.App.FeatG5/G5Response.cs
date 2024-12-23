﻿using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;

namespace PenomyAPI.App.FeatG5;

public class G5Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public bool IsUserFavorite { get; set; }

    public bool HasFollowed { get; set; }

    public G5ComicDetailReadModel Result { get; set; }

    public G5ResponseStatusCode StatusCode { get; set; }

    public static readonly G5Response INVALID_REQUEST = new()
    {
        IsSuccess = false,
        StatusCode = G5ResponseStatusCode.INVALID_REQUEST,
    };

    public static readonly G5Response NOT_FOUND = new()
    {
        IsSuccess = false,
        StatusCode = G5ResponseStatusCode.NOT_FOUND,
    };

    public static readonly G5Response FAILED = new()
    {
        IsSuccess = false,
        StatusCode = G5ResponseStatusCode.FAILED,
    };
}
