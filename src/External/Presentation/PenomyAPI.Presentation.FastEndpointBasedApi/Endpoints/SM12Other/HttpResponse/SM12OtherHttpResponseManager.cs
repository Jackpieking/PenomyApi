using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM12.OtherHandler;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12Other.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12Other.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12OtherOther.HttpResponse;

public class SM12OtherHttpResponseManager
{
    private static ConcurrentDictionary<Sm12OtherResponseStatusCode, Func<SM12OtherResponse, SM12OtherHttpResponse>>
        _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<Sm12OtherResponseStatusCode, Func<SM12OtherResponse, SM12OtherHttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            Sm12OtherResponseStatusCode.SUCCESS,
            response => new SM12OtherHttpResponse
            {
                AppCode = Sm12OtherResponseStatusCode.SUCCESS.ToString(),
                HttpCode = StatusCodes.Status200OK,
                Body = new SM12OtherResponseDto
                {
                    FriendRequests = response.FriendRequest.Select(x => new FriendRequestDto
                    {
                        CreatedAt = x.CreatedAt,
                        CreatedBy = x.CreatedBy,
                        FriendId = x.FriendId,
                        RequestStatus = x.RequestStatus,
                        UpdatedAt = x.UpdatedAt
                    }).ToList()
                }
            });

        _dictionary.TryAdd(
            Sm12OtherResponseStatusCode.FAILED,
            response => new SM12OtherHttpResponse
            {
                AppCode = Sm12OtherResponseStatusCode.FAILED.ToString(),
                HttpCode = StatusCodes.Status400BadRequest
            });
    }

    internal static Func<SM12OtherResponse, SM12OtherHttpResponse> Resolve(Sm12OtherResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default)) Init();

        if (_dictionary.TryGetValue(statusCode, out var response)) return response;

        return _dictionary[Sm12OtherResponseStatusCode.FAILED];
    }
}
