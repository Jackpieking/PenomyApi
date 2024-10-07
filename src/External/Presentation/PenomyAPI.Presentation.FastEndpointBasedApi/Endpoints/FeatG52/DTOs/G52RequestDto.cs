using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG52.DTOs
{
    public class G52RequestDto
    {
        public long ArtworkId { get; init; }

        public long ChapterId { get; init; } = 0;

        public string CommentContent { get; init; }

        public long ParentCommentId { get; init; }

        public bool IsDirectComment { get; init; }

        public long UserId { get; init; }
    }
}
