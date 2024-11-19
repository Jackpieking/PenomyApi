using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM13;

public class SM13Request : IFeatureRequest<SM13Response>
{
    public long UserId { get; set; }
    public long UserPostId { get; set; }
    public bool IsUpdateAttachMedia { get; set; }
    public string Content { get; set; }
    public bool AllowComment { get; set; }
    public UserPostPublicLevel PublicLevel { get; set; }
    public IEnumerable<AppFileInfo> AppFileInfos { get; set; }
    public long UpdatedBy { get; set; }
}
