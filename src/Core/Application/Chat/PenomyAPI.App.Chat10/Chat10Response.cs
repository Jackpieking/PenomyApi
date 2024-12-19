using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Chat.FeatChat10;

namespace PenomyAPI.App.Chat10;

public class Chat10Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public ICollection<Chat10UserProfileReadModel> UserChatMessages { get; set; }

    public string[] ErrorMessages { get; set; }

    public Chat10ResponseStatusCode StatusCode { get; set; }
}
