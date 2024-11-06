using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt5;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt5.HttpResponses;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt5.HttpResponseMappers;

public static class Art5HttpResponseMapper
{
    private static ConcurrentDictionary<Art5ResponseAppCode, Func<Art5Response, Art5HttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: Art5ResponseAppCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = Art5HttpResponse.GetAppCode(response.AppCode),
                HttpCode = StatusCodes.Status200OK,
                Body = new()
                {
                    Id = response.ComicDetail.Id.ToString(),
                    Title = response.ComicDetail.Title,
                    ThumbnailUrl = response.ComicDetail.ThumbnailUrl,
                    Introduction = response.ComicDetail.Introduction,
                    ArtworkStatus = response.ComicDetail.ArtworkStatus,
                    AuthorName = response.ComicDetail.Creator.NickName,
                    Origin = response.ComicDetail.Origin.CountryName,
                    Categories = response.ComicDetail.ArtworkCategories.Select(category => new CategoryDto
                    {
                        Id = category.CategoryId.ToString(),
                        Label = category.Category.Name
                    }),
                    Series = SeriesDto.MapFrom(response.ComicDetail.ArtworkSeries)
                }
            });

        _dictionary.TryAdd(
            key: Art5ResponseAppCode.COMIC_ID_NOT_FOUND,
            value: (response) => new()
            {
                AppCode = Art5HttpResponse.GetAppCode(response.AppCode),
                HttpCode = StatusCodes.Status404NotFound,
            });

        _dictionary.TryAdd(
            key: Art5ResponseAppCode.COMIC_IS_TEMPORARILY_REMOVED,
            value: (response) => new()
            {
                AppCode = Art5HttpResponse.GetAppCode(response.AppCode),
                HttpCode = StatusCodes.Status400BadRequest,
            });

        _dictionary.TryAdd(
            key: Art5ResponseAppCode.COMIC_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR,
            value: (response) => new()
            {
                AppCode = Art5HttpResponse.GetAppCode(response.AppCode),
                HttpCode = StatusCodes.Status401Unauthorized,
            });

        _dictionary.TryAdd(
            key: Art5ResponseAppCode.DATABASE_ERROR,
            value: (response) => new()
            {
                AppCode = Art5HttpResponse.GetAppCode(response.AppCode),
                HttpCode = StatusCodes.Status500InternalServerError,
            });
    }

    internal static Func<Art5Response, Art5HttpResponse> Resolve(Art5ResponseAppCode appCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(appCode, out var response))
        {
            return response;
        }

        return _dictionary[Art5ResponseAppCode.DATABASE_ERROR];
    }
}
