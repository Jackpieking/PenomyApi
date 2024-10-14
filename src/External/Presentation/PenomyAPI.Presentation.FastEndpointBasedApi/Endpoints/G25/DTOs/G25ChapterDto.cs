using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;

public class G25ChapterDto
{
    public long Id { get; set; }
    public string ChapterTitle { get; set; }
    public int ChapterUploadOrder { get; set; }
    public string ThumbnailUrl { get; set; }
    public DateTime ViewedAt { get; set; }
}
