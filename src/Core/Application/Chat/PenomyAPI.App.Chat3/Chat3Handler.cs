using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.Chat3;

public class Chat3Handler : IFeatureHandler<Chat3Request, Chat3Response>
{
    private readonly IChat3Repository _Chat3Repository;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;

    public Chat3Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService
    )
    {
        _Chat3Repository = unitOfWork.Value.Chat3Repository;
        _fileService = fileService;
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
