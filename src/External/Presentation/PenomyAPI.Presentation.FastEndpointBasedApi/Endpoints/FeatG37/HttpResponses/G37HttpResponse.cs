using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG37;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG37.HttpResponses;

public class G37HttpResponse : AppHttpResponse<object>
{
    private static readonly object _lock = new object();
    private static G37HttpResponse _succces = null;
    private static G37HttpResponse _userHasAlreadyBecomeCreator = null;
    private static G37HttpResponse _databaseError = null;

    public static G37HttpResponse MapFrom(G37Response response)
    {
        if (response.AppCode == G37ResponseAppCode.SUCCESS)
        {
            return SUCCESS();
        }

        if (response.AppCode == G37ResponseAppCode.USER_HAS_ALREADY_BECOME_CREATOR)
        {
            return USER_HAS_ALREADY_BECOME_CREATOR();
        }

        return DATABASE_ERROR();
    }

    private static string GetAppCode(G37ResponseAppCode appCode)
    {
        return $"G37.{appCode}.{(int) appCode}";
    }

    private static G37HttpResponse SUCCESS()
    {
        lock (_lock)
        {
            if (_succces == null)
            {
                _succces = new()
                {
                    AppCode = GetAppCode(G37ResponseAppCode.SUCCESS),
                    HttpCode = StatusCodes.Status200OK,
                };
            }
        }

        return _succces;
    }

    private static G37HttpResponse USER_HAS_ALREADY_BECOME_CREATOR()
    {
        lock (_lock)
        {
            if (_userHasAlreadyBecomeCreator == null)
            {
                _userHasAlreadyBecomeCreator = new()
                {
                    AppCode = GetAppCode(G37ResponseAppCode.USER_HAS_ALREADY_BECOME_CREATOR),
                    HttpCode = StatusCodes.Status400BadRequest,
                };
            }
        }

        return _userHasAlreadyBecomeCreator;
    }

    private static G37HttpResponse DATABASE_ERROR()
    {
        lock (_lock)
        {
            if (_databaseError == null)
            {
                _databaseError = new()
                {
                    AppCode = GetAppCode(G37ResponseAppCode.DATABASE_ERROR),
                    HttpCode = StatusCodes.Status200OK,
                };
            }
        }

        return _databaseError;
    }
}
