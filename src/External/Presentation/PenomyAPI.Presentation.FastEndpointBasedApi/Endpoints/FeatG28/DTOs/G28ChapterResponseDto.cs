using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.DTOs;

public class G28ChapterResponseDto
{
    public string Id { get; set; }

    public int UploadOrder { get; set; }

    public DateTime PublishedAt { get; set; }
}
