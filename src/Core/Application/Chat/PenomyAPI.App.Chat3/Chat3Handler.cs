using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.Realtime;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Models.Chat.FeatChat10;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.Chat3;

public class Chat3Handler : IFeatureHandler<Chat3Request, Chat3Response>
{
    private readonly IChat3Repository _Chat3Repository;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;
    private readonly IChatHub _chatHub;

    public Chat3Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService,
        Lazy<IChatHub> chatHub
    )
    {
        _Chat3Repository = unitOfWork.Value.Chat3Repository;
        _fileService = fileService;
        _chatHub = chatHub.Value;
    }

    public async Task<Chat3Response> ExecuteAsync(Chat3Request request, CancellationToken ct)
    {
        Chat3Response response = new();
        try
        {
            var group = await _Chat3Repository.GetChatGroupAsync(request.ChatGroupId, ct);
            if (request.Content == null || group == null)
                return new Chat3Response
                {
                    IsSuccess = false,
                    ErrorMessages = ["Group not found"],
                    StatusCode = Chat3ResponseStatusCode.FAILED,
                };
            if (
                !await _Chat3Repository.IsMemberOfChatGroupAsync(
                    request.ChatGroupId,
                    request.UserId,
                    ct
                )
            )
                return new Chat3Response
                {
                    IsSuccess = false,
                    ErrorMessages = ["Member not found in group"],
                    StatusCode = Chat3ResponseStatusCode.USER_NOT_MEMBER,
                };
            var dateTimeNow = DateTime.UtcNow;
            var likeStatistic = ChatMessageLikeStatistic.Empty(request.MessageId);
            var chatGroupMessage = new ChatMessage
            {
                Id = request.MessageId,
                ChatGroupId = request.ChatGroupId,
                CreatedAt = dateTimeNow,
                Content = request.Content,
                CreatedBy = request.UserId,
                UpdatedAt = dateTimeNow,
                MessageType = request.MessageType,
                ReplyToAnotherMessage = request.IsReply,
            };
            var result = await _Chat3Repository.SaveMessageAsync(
                chatGroupMessage,
                likeStatistic,
                ct
            );
            var userInfo = await _Chat3Repository.GetUserChatInfoAsync(request.UserId, ct);
            await _chatHub.ReceiveGroupMessange(request.ChatGroupId.ToString(), new Chat10UserProfileReadModel
            {
                UserId = request.UserId,
                AvatarUrl = userInfo.AvatarUrl,
                NickName = userInfo.NickName,
                Messages = [new Chat10ChatMessageReadModel
                {
                    ChatId = chatGroupMessage.Id,
                    Content = chatGroupMessage.Content,
                    Time = chatGroupMessage.CreatedAt,
                    IsReply = chatGroupMessage.ReplyToAnotherMessage,
                    ReplyMessageId = 0
                }]
            });
            if (result)
            {
                response.IsSuccess = true;
                response.StatusCode = Chat3ResponseStatusCode.SUCCESS;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessages = [ex.Message];
            response.StatusCode = Chat3ResponseStatusCode.FAILED;
        }

        return response;
    }
}
