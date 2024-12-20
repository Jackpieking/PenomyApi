using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Persist.Postgres.Data.DbContexts;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Chat;

public class Chat5Repository : IChat5Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<ChatGroup> _groupContext;
    private readonly DbSet<ChatGroupMember> _groupMemberContext;
    private readonly DbSet<ChatMessageLikeStatistic> _likeStatisticsContext;
    private readonly DbSet<ChatMessageAttachedMedia> _messageAttachedContext;
    private readonly DbSet<ChatMessage> _messageContext;


    public Chat5Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _messageContext = _dbContext.Set<ChatMessage>();
        _messageAttachedContext = _dbContext.Set<ChatMessageAttachedMedia>();
        _likeStatisticsContext = _dbContext.Set<ChatMessageLikeStatistic>();
        _groupContext = dbContext.Set<ChatGroup>();
        _groupMemberContext = dbContext.Set<ChatGroupMember>();
    }

    public async Task<bool> IsMessageExistsAsync(long userId, long messageId, CancellationToken token = default)
    {
        return await _messageContext.AnyAsync(x => x.Id == messageId && x.CreatedBy == userId, token);
    }

    public async Task<bool> RemoveMessageAsync(long userId, long messageId, CancellationToken ct = default)
    {
        var dbResult = false;

        await _dbContext
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(async () =>
            {
                await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(ct);

                try
                {
                    await _likeStatisticsContext
                        .Where(k => k.ChatMessageId == messageId && k.ChatMessage.CreatedBy == userId)
                        .ExecuteDeleteAsync(ct);
                    await _messageContext
                        .Where(m => m.Id == messageId && m.CreatedBy == userId)
                        .ExecuteDeleteAsync(ct);


                    await dbTransaction.CommitAsync(ct);
                    await _dbContext.SaveChangesAsync(ct);
                    dbResult = true;
                }
                catch
                {
                    await dbTransaction.RollbackAsync(ct);
                }
            });

        return dbResult;
    }
}
