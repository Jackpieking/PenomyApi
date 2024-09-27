﻿using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.App.FeatG5;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.HttpResponse
{
    public class G5ResponseManager
    {
        private static ConcurrentDictionary<
            G5ResponseStatusCode,
            Func<G5Request, G5Response, G5HttpResponse>> _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G5ResponseStatusCode.SUCCESS,
                value: (_, response) => new()
                {
                    AppCode = $"G5.{G5ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                });

            _dictionary.TryAdd(
                key: G5ResponseStatusCode.FAILED,
                value: (_, response) => new()
                {
                    AppCode = $"G5.{G5ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                });
            _dictionary.TryAdd(
                key: G5ResponseStatusCode.ID_NOT_FOUND,
                value: (_, response) => new()
                {
                    AppCode = $"G5.{G5ResponseStatusCode.ID_NOT_FOUND}",
                    HttpCode = StatusCodes.Status400BadRequest,
                });
        }

        internal static Func<G5Request, G5Response, G5HttpResponse> Resolve(
            G5ResponseStatusCode statusCode)
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
