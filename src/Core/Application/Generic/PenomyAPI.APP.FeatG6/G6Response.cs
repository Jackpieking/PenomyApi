using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.APP.FeatG6;

public class G6Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<Artwork> Result { get; set; }

    public G6ResponseStatusCode StatusCode { get; set; }

    public static readonly G6Response ARTWORK_ID_NOT_FOUND = new()
    {
        IsSuccess = false,
        StatusCode = G6ResponseStatusCode.ARTWORK_ID_NOT_FOUND
    };

    public static readonly G6Response DATABASE_ERROR = new()
    {
        IsSuccess = false,
        StatusCode = G6ResponseStatusCode.DATABASE_ERROR
    };

    public static G6Response SUCCESS(List<Artwork> result) => new()
    {
        IsSuccess = true,
        StatusCode = G6ResponseStatusCode.SUCCESS,
        Result = result
    };
}
