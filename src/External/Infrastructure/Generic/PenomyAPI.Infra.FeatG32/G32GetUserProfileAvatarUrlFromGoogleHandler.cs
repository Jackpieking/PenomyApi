using System.Linq;
using System.Runtime.CompilerServices;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.FeatG32.Infrastructures;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.Infra.FeatG32;

public sealed class G32GetUserProfileAvatarUrlFromGoogleHandler
    : IG32GetUserProfileAvatarUrlFromGoogleHandler
{
    private readonly GoogleAuthenticationOption _option;
    private static PeopleServiceService _peopleService;

    public G32GetUserProfileAvatarUrlFromGoogleHandler(GoogleAuthenticationOption option)
    {
        _option = option;
        _peopleService = InitPeopleService(_option.ApiKey);
    }

    public string Execute(string userGoogleId)
    {
        const string GooglePeopleApiBaseRoute = "people/";
        const string GooglePeopleApiPersonFields = "photos";

        var stringHandler = new DefaultInterpolatedStringHandler();

        stringHandler.AppendLiteral(GooglePeopleApiBaseRoute);
        stringHandler.AppendFormatted(userGoogleId);

        var userUrlPart = stringHandler.ToStringAndClear();

        var peopleRequest = _peopleService.People.Get(userUrlPart);
        peopleRequest.PersonFields = GooglePeopleApiPersonFields;

        var profile = peopleRequest.Execute();

        return profile.Photos.FirstOrDefault()?.Url ?? CommonValues.Others.DefaultUserAvaterUrl;
    }

    private static PeopleServiceService InitPeopleService(string apiKey)
    {
        if (!Equals(_peopleService, null))
        {
            return _peopleService;
        }

        return new(new() { ApiKey = apiKey });
    }
}
