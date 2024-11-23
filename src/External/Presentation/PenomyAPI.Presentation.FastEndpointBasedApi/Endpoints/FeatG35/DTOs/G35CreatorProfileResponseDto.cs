using PenomyAPI.App.FeatG35.OtherHandlers.GetCreatorProfile;
using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.DTOs;

public sealed class G35CreatorProfileResponseDto
{
    public string CreatorId { get; set; }

    public string Nickname { get; set; }

    public string AvatarUrl { get; set; }

    public string AboutMe { get; set; }

    public int TotalArtworks { get; set; }

    /// <summary>
    ///     The total users that have followed this user account.
    /// </summary>
    public int TotalFollowers { get; set; }

    public DateTime RegisteredAt { get; set; }

    public DateTime BecomeCreatorAt { get; set; }

    public static G35CreatorProfileResponseDto MapFrom(G35GetCreatorProfileResponse response)
    {
        var creatorProfile = response.CreatorProfile;

        var mapResult = new G35CreatorProfileResponseDto
        {
            CreatorId = creatorProfile.UserId.ToString(),
            Nickname = creatorProfile.NickName,
            AboutMe = creatorProfile.AboutMe,
            AvatarUrl = creatorProfile.AvatarUrl,
            RegisteredAt = creatorProfile.RegisteredAt,
            TotalArtworks = creatorProfile.CreatorProfile.TotalArtworks,
            TotalFollowers = creatorProfile.CreatorProfile.TotalFollowers,
            BecomeCreatorAt = creatorProfile.CreatorProfile.RegisteredAt,
        };

        return mapResult;
    }
}
