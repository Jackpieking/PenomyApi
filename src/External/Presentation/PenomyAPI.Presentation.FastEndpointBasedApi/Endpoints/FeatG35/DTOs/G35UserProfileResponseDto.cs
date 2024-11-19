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

    /// <summary>
    ///     The total users that have followed this user account.
    /// </summary>
    public int TotalFollowers { get; set; }

    /// <summary>
    ///     The total created artworks of the current user as a creator.
    /// </summary>
    public int TotalArtworks { get; set; }

    public DateTime LastActiveAt { get; set; }

    public DateTime RegisteredAt { get; set; }

    public DateTime BecomeCreatorAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public static G35UserProfileResponseDto MapFrom(G35Response response)
    {
        // Map the related information first
        // before checking current user is creator or not.
        var userProfile = response.UserProfile;

        var mapResult = new G35UserProfileResponseDto
        {
            UserId = userProfile.UserId.ToString(),
            AboutMe = userProfile.AboutMe,
            AvatarUrl = userProfile.AvatarUrl,
            LastActiveAt = userProfile.LastActiveAt,
            Nickname = userProfile.NickName,
            IsCreator = userProfile.RegisterAsCreator,
            RegisteredAt = userProfile.RegisteredAt,
            TotalFollowedCreators = userProfile.TotalFollowedCreators,
            UpdatedAt = userProfile.UpdatedAt
        };

        // If the user has registered as creator, then map more information.
        if (userProfile.RegisterAsCreator)
        {
            mapResult.BecomeCreatorAt = userProfile.CreatorProfile.RegisteredAt;
            mapResult.TotalFollowers = userProfile.CreatorProfile.TotalFollowers;
            mapResult.TotalArtworks = userProfile.CreatorProfile.TotalArtworks;
        }

        return mapResult;
    }
}
