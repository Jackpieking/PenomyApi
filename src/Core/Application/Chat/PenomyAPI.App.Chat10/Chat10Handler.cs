using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Chat.FeatChat10;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.Chat10;

public class Chat10Handler : IFeatureHandler<Chat10Request, Chat10Response>
{
    private readonly IChat10Repository _Chat10Repository;

    public Chat10Handler(
        Lazy<IUnitOfWork> unitOfWork
    )
    {
        _Chat10Repository = unitOfWork.Value.Chat10Repository;
    }

    public async Task<Chat10Response> ExecuteAsync(Chat10Request request, CancellationToken ct)
    {
        Chat10Response response = new();

        try
        {
            bool isExist = _Chat10Repository.CheckGroupExistAsync(request.ChatGroupId, ct).Result;

            if (!isExist)
            {
                response.IsSuccess = false;
                response.UserChatMessages = [];
                response.ErrorMessages = ["Group not found"];
                response.StatusCode = Chat10ResponseStatusCode.INVALID_REQUEST;

                return response;
            };

            var chatMessages = await _Chat10Repository.GetChatGroupByGroupIdAsync(
                request.ChatGroupId,
                request.PageNum,
                request.ChatNum,
                ct);

            Chat10UserProfileReadModel chat10UserProfile = new();
            ICollection<Chat10UserProfileReadModel> chat10UserProfileList = [];

            // Create message list for user
            foreach (var chat in chatMessages)
            {
                // If next user is other user, add old user to list
                // and create new list message for new user
                if (chat10UserProfile.UserId != chat.Sender.UserId)
                {
                    chat10UserProfileList.Add(chat10UserProfile);
                    chat10UserProfile = new();
                }
                // If this is new user message list add info for list
                if (chat10UserProfile.UserId == 0)
                {
                    chat10UserProfile.UserId = chat.Sender.UserId;
                    chat10UserProfile.AvatarUrl = chat.Sender.AvatarUrl;
                    chat10UserProfile.NickName = chat.Sender.NickName;
                    chat10UserProfile.Messages = [];
                }
                // If this message is of current user add to message list
                if (chat10UserProfile.UserId == chat.Sender.UserId)
                {
                    chat10UserProfile.Messages.Add(new Chat10ChatMessageReadModel
                    {
                        ChatId = chat.Id,
                        Content = chat.Content,
                        Time = chat.CreatedAt,
                        IsReply = chat.ReplyToAnotherMessage,
                        // If this is reply message find root message id
                        ReplyMessageId = chat.ReplyToAnotherMessage
                            ? await _Chat10Repository.GetMessageReplyByChatIdAsync(chat.Id, ct)
                            : 0,
                    });
                }
            }

            response.IsSuccess = true;
            response.UserChatMessages = chat10UserProfileList;
            response.StatusCode = Chat10ResponseStatusCode.SUCCESS;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.UserChatMessages = [];
            response.ErrorMessages = [ex.Message];
            response.StatusCode = Chat10ResponseStatusCode.FAILED;
        }

        return response;
    }
}
