using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;

public class G25ChapterViewHistoryItemResponseDto
{
    public string Id { get; set; }

    public int UploadOrder { get; set; }

    public DateTime ViewedAt { get; set; }
}
