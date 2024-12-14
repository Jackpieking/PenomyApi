using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenowmyAPI.APP.Chat2;

public class Chat2Handler : IFeatureHandler<Chat2Request, Chat2Response>
{
    private readonly IChat2Repository _Chat2Repository;

    public Chat2Handler(
        Lazy<IUnitOfWork> unitOfWork
    )
    {
        _Chat2Repository = unitOfWork.Value.Chat2Repository;
    }

    public async Task<Chat2Response> ExecuteAsync(Chat2Request request, CancellationToken ct)
    {
        var response = new Chat2Response();
        try
        {
            List<ChatGroup> groups = [];
            if (request.UserId != 0)
            {
                groups = await _Chat2Repository.GetChatGroupsAsync(request.UserId, ct);
                response.ChatGroups = groups;
            }

            response.StatusCode = Chat2ResponseStatusCode.SUCCESS;
        }
        catch
        {
            response.StatusCode = Chat2ResponseStatusCode.FAILED;
        }

        return response;
    }
}
