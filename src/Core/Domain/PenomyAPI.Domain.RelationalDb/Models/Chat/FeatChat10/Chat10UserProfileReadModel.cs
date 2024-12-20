using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Models.Chat.FeatChat10;

public class Chat10UserProfileReadModel
{
    public long UserId { get; set; }

    public string AvatarUrl { get; set; }

    public string NickName { get; set; }

    public ICollection<Chat10ChatMessageReadModel> Messages { get; set; }
}
