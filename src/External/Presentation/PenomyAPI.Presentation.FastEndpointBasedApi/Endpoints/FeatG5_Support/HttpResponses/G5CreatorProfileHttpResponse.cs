using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5.OtherHandlers.CreatorProfileDetail;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support.HttpResponses;

public class G5CreatorProfileHttpResponse : AppHttpResponse<G5CreatorProfileReadModel>
{
    public static string GetAppCode(G5CreatorProfileResponseAppCode appCode)
    {
        return  $"G5.GET_CREATOR_PROFILE.{appCode}.{(int) appCode}";
    }

    public static readonly G5CreatorProfileHttpResponse CREATOR_NOT_FOUND = new()
    {
        HttpCode = StatusCodes.Status404NotFound,
        AppCode = GetAppCode(G5CreatorProfileResponseAppCode.CREATOR_NOT_FOUND)
    };

    public static G5CreatorProfileHttpResponse SUCCESS(G5CreatorProfileDetailResponse response)
    {
        return new()
        {
            HttpCode = StatusCodes.Status200OK,
            AppCode = GetAppCode(G5CreatorProfileResponseAppCode.SUCCESS),
            Body = response.CreatorProfile,
        };
    }

    public static G5CreatorProfileHttpResponse MapFrom(G5CreatorProfileDetailResponse response)
    {
        if (response.AppCode == G5CreatorProfileResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        return CREATOR_NOT_FOUND;
    }
}
