using PenomyAPI.App.Common.FileServices;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt12.Infrastructures;

/// <summary>
///     The base interaface file service supports only for feature-art10.
/// </summary>
public interface IArt12FileService
    : IFeatureDistributedFileService<Art12Handler, Art12Request, Art12Response>
{
    /// <summary>
    ///     Delete the list of specified files with input <paramref name="fileIds"/>
    /// </summary>
    /// <param name="fileIds">
    ///     The list of ids of the files to be deleted.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The result (bool) of the deletion.
    /// </returns>
    Task<bool> DeleteMultipleFileByIdsAsync(
        string chapterFolderRelativePath,
        IEnumerable<long> fileIds,
        CancellationToken cancellationToken);
}
