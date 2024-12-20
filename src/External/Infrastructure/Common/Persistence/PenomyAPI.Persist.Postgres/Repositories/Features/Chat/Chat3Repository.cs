using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Models.Chat.FeatChat10;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Chat;

public class Chat3Repository : IChat3Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<ChatGroup> _groupContext;
    private readonly DbSet<ChatGroupMember> _groupMemberContext;
    private readonly DbSet<ChatMessageLikeStatistic> _likeStatisticsContext;
    private readonly DbSet<ChatMessageAttachedMedia> _messageAttachedContext;
    private readonly DbSet<ChatMessage> _messageContext;


    public Chat3Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _messageContext = _dbContext.Set<ChatMessage>();
        _messageAttachedContext = _dbContext.Set<ChatMessageAttachedMedia>();
        _likeStatisticsContext = _dbContext.Set<ChatMessageLikeStatistic>();
        _groupContext = dbContext.Set<ChatGroup>();
        _groupMemberContext = dbContext.Set<ChatGroupMember>();
    }

    public async Task<bool> SaveMessageAsync(ChatMessage chat,
        ChatMessageLikeStatistic statistic,
        CancellationToken token)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        await executionStrategy.ExecuteAsync(async () =>
            await InternalSaveMessagePostAsync(
                chat,
                statistic,
                token,
                result
            ));
        return result.Value;
    }

    public async Task<ChatGroup> GetChatGroupAsync(long groupId, CancellationToken token)
    {
        return await _groupContext.FirstOrDefaultAsync(g => g.Id == groupId, token);
    }

    public async Task<bool> IsMemberOfChatGroupAsync(long groupId, long userId, CancellationToken token)
    {
        return await _groupMemberContext.AnyAsync(x => x.MemberId == userId && x.ChatGroupId == groupId, token);
    }

    private async Task InternalSaveMessagePostAsync(ChatMessage chat,
        ChatMessageLikeStatistic statistic,
        CancellationToken token, Result<bool> result)
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                token
            );
            await _messageContext.AddAsync(chat, token);
            await _likeStatisticsContext.AddAsync(statistic, token);
            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            result.Value = true;
        }
        catch (Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(token);
                await transaction.DisposeAsync();
            }

            result.Value = false;
        }
    }

    public async Task<Chat10UserProfileReadModel> GetUserChatInfoAsync(long userId, CancellationToken token)
    {
        return await _groupMemberContext.AsNoTracking()
            .Where(o => o.MemberId == userId)
            .Select(o => new Chat10UserProfileReadModel
            {
                UserId = o.MemberId,
                NickName = o.Member.NickName,
                AvatarUrl = o.Member.AvatarUrl
            })
            .FirstOrDefaultAsync(cancellationToken: token);
    }
}
