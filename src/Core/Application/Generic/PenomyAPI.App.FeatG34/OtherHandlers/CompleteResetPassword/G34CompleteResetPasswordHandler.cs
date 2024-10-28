using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;

public sealed class G34CompleteResetPasswordHandler
    : IFeatureHandler<G34CompleteResetPasswordRequest, G34CompleteResetPasswordResponse>
{
    public Task<G34CompleteResetPasswordResponse> ExecuteAsync(
        G34CompleteResetPasswordRequest request,
        CancellationToken ct
    )
    {
        throw new System.NotImplementedException();
    }
}
