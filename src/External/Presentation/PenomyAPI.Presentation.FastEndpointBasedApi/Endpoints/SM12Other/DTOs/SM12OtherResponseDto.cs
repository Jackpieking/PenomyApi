using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12Other.DTOs;

public class SM12OtherResponseDto
{
    public List<FriendRequestDto> FriendRequests { get; set; } = new();
}

public class FriendRequestDto
{
    public long FriendId { get; set; }

    public long CreatedBy { get; set; }

    public RequestStatus RequestStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
