namespace PenomyAPI.App.Common.FileServices;

/// <summary>
///     The default distributed file service that will be used in the app.
/// </summary>
public interface IDefaultDistributedFileService
    : IFeatureDistributedFileService<
        IFeatureHandler<IFeatureRequest<IFeatureResponse>, IFeatureResponse>,
        IFeatureRequest<IFeatureResponse>,
        IFeatureResponse
    > { }
