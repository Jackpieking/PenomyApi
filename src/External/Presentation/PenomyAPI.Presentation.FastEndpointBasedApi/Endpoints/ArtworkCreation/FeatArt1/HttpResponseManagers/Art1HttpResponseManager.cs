using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt1;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponse;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponseManagers;

public static class Art1HttpResponseManager
{
    private static ConcurrentDictionary<Art1ResponseAppCode, Func<Art1Response, Art1HttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: Art1ResponseAppCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = Art1HttpResponse.GetAppCode(response.AppCode),
                HttpCode = StatusCodes.Status200OK,
                Body = response.Artworks.Select(artwork => new DTOs.ArtworkDetailResponseDto
                {
                    Id = artwork.Id.ToString(),
                    Title = artwork.Title,
                    ArtworkStatus = artwork.ArtworkStatus,
                    PublicLevel = artwork.PublicLevel,
                    ThumbnailUrl = artwork.ThumbnailUrl,
                    OriginImageUrl = artwork.Origin.ImageUrl,
                    TotalChapters = artwork.LastChapterUploadOrder,
                    AverageStarRate = artwork.ArtworkMetaData.AverageStarRate,
                    TotalUsersRated = artwork.ArtworkMetaData.TotalUsersRated,
                    CreatedAt = artwork.CreatedAt,
                    UpdatedAt = artwork.UpdatedAt,
                })
            });

        _dictionary.TryAdd(
            key: Art1ResponseAppCode.EMPTY_ARTWORK_LIST,
            value: (response) => new()
            {
                AppCode = Art1HttpResponse.GetAppCode(Art1ResponseAppCode.EMPTY_ARTWORK_LIST),
                HttpCode = StatusCodes.Status200OK,
            });
    }

    internal static Func<Art1Response, Art1HttpResponse> Resolve(Art1ResponseAppCode appCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(appCode, out var response))
        {
            return response;
        }

        return _dictionary[Art1ResponseAppCode.EMPTY_ARTWORK_LIST];
    }
}
