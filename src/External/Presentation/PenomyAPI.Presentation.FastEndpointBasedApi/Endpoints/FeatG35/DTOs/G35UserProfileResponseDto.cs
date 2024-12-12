using PenomyAPI.App.FeatG35;
using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.DTOs;

public sealed class G35UserProfileResponseDto
{
    public string UserId { get; set; }

    public string Nickname { get; set; }

    public string AvatarUrl { get; set; }

    public string AboutMe { get; set; }

    public bool IsCreator { get; set; }

    public int TotalFollowedCreators { get; set; }

    public DateTime RegisteredAt { get; set; }

    public DateTime LastActiveAt { get; set; }

    // Creator profile section.
    public int TotalFollowers { get; set; }

    public int TotalArtworks { get; set; }

    public DateTime BecomeCreatorAt { get; set; }

    public static G35UserProfileResponseDto MapFrom(G35Response response)
    {
        var userProfile = response.UserProfile;

        var mapResult = new G35UserProfileResponseDto
        {
            UserId = userProfile.UserId.ToString(),
            Nickname = userProfile.NickName,
            AboutMe = userProfile.AboutMe,
            AvatarUrl = userProfile.AvatarUrl,
            IsCreator = userProfile.RegisterAsCreator,
            RegisteredAt = userProfile.RegisteredAt,
        };

        // If the request is sent by the profile owner, then map more information.
        if (response.IsProfileOwner)
        {
            mapResult.LastActiveAt = userProfile.LastActiveAt;
            mapResult.TotalFollowedCreators = userProfile.TotalFollowedCreators;
        }

        // If the user has registered as creator, then map more information.
        if (mapResult.IsCreator)
        {
            mapResult.TotalFollowers = userProfile.CreatorProfile.TotalFollowers;
            mapResult.TotalArtworks = userProfile.CreatorProfile.TotalArtworks;
            mapResult.BecomeCreatorAt = userProfile.CreatorProfile.RegisteredAt;
        }

        return mapResult;
    }
}
