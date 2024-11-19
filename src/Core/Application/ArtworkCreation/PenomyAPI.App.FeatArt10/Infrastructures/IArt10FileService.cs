using PenomyAPI.App.Common.FileServices;

namespace PenomyAPI.App.FeatArt10.Infrastructures;

/// <summary>
///     The base interaface file service supports only for feature-art10.
/// </summary>
public interface IArt10FileService :
    IFeatureDistributedFileService<Art10Handler, Art10Request, Art10Response>
{
}
