using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.Chat5;

public class Chat5Handler : IFeatureHandler<Chat5Request, Chat5Response>
{
    private readonly IChat5Repository _Chat5Repository;

    public Chat5Handler(
        Lazy<IUnitOfWork> unitOfWork
    )
    {
        _Chat5Repository = unitOfWork.Value.Chat5Repository;
    }

    public async Task<Chat5Response> ExecuteAsync(Chat5Request request, CancellationToken ct)
    {
        var response = new Chat5Response();
        try
        {
            if (!await _Chat5Repository.IsMessageExistsAsync(request.UserId, request.MessageID, ct))
            {
                response.StatusCode = Chat5ResponseStatusCode.NOT_EXIST;
                response.isSuccess = false;
            }
            else
            {
                response.isSuccess = await _Chat5Repository.RemoveMessageAsync(request.UserId, request.MessageID, ct);
                response.StatusCode =
                    response.isSuccess ? Chat5ResponseStatusCode.SUCCESS : Chat5ResponseStatusCode.FAILED;
            }
        }
        catch
        {
            response.isSuccess = false;
            response.StatusCode = Chat5ResponseStatusCode.FAILED;
        }

        return response;
    }
}
