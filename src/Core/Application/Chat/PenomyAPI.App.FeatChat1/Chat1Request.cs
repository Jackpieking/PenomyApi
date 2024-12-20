using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;

namespace PenomyAPI.App.FeatChat1;

public class Chat1Request : IFeatureRequest<Chat1Response>
{
    public long UserId { get; set; }

    public AppFileInfo CoverImageFileInfo { get; set; }

    public string GroupName { get; set; }

    public bool IsPublic { get; set; }
    public ChatGroupType GroupType { get; set; }
}
