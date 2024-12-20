namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.DTOs;

/// <summary>
///     The detail of the comic to support for displaying
///     when creating a new comic chapter.
/// </summary>
public class ComicDetailToCreateChapterResponseDto
{
    public string Title { get; set; }

    public int LastChapterUploadOrder { get; set; }
}
