using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM38.GroupProfile;

public class SM38ProfileHandler : IFeatureHandler<SM38ProfileRequest, SM38ProfileResponse>
{
    private readonly ISM38Repository _SM38Repository;

    public SM38ProfileHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _SM38Repository = unitOfWork.Value.SM38Repository;
    }

    public async Task<SM38ProfileResponse> ExecuteAsync(
        SM38ProfileRequest request,
        CancellationToken ct
    )
    {
        var result = await _SM38Repository.UpdateGroupDetailAsync(
            userId: request.UserId,
            groupId: request.GroupId,
            name: request.Name,
            description: request.Description,
            RequireApprovedWhenPost: request.RequireApprovedWhenPost
        );
        if (result == 0)
            return new SM38ProfileResponse
            {
                StatusCode = SM38ResponseStatusCode.DATABSE_ERROR,
                Result = false,
                IsSuccess = false,
            };
        return new SM38ProfileResponse
        {
            StatusCode = SM38ResponseStatusCode.SUCCESS,
            Result = true,
            IsSuccess = true,
        };
    }
}
