using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Chat;

public class Chat10Repository : IChat10Repository
{
    private readonly DbSet<ChatMessage> _chatMessage;
    private readonly DbSet<ChatGroup> _chatGroup;
    private readonly DbSet<ChatMessageReply> _chatMessageReplies;

    public Chat10Repository(DbContext dbContext)
    {
        _chatMessage = dbContext.Set<ChatMessage>();
        _chatGroup = dbContext.Set<ChatGroup>();
        _chatMessageReplies = dbContext.Set<ChatMessageReply>();
    }

    public async Task<bool> CheckGroupExistAsync(long chatGroupId, CancellationToken ct = default)
    {
        return await _chatGroup.AsNoTracking()
            .AnyAsync(o => o.Id == chatGroupId, cancellationToken: ct);
    }

    public async Task<ICollection<ChatMessage>> GetChatGroupByGroupIdAsync(long chatGroupId, int pageNum, int chatNum, CancellationToken ct = default)
    {
        return await _chatMessage.AsNoTracking()
            .Where(o => o.ChatGroupId == chatGroupId)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNum - 1) * chatNum)
            .Take(chatNum)
            .Select(o => new ChatMessage
            {
                Id = o.Id,
                Sender = new UserProfile
                {
                    UserId = o.Sender.UserId,
                    AvatarUrl = o.Sender.AvatarUrl,
                    NickName = o.Sender.NickName
                },
                Content = o.Content,
                CreatedAt = o.CreatedAt,
                ReplyToAnotherMessage = o.ReplyToAnotherMessage
            })
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<long> GetMessageReplyByChatIdAsync(long chatId, CancellationToken ct = default)
    {
        return await _chatMessageReplies.AsNoTracking()
            .Where(o => o.RepliedMessageId == chatId)
            .Select(o => o.RootChatMessageId)
            .FirstOrDefaultAsync(cancellationToken: ct);
    }
}
