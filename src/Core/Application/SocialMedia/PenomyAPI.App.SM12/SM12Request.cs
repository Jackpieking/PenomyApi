using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM12;

public class SM12Request : IFeatureRequest<SM12Response>
{
    public long UserId { get; set; }
    public long UserPostId { get; set; }
    public string Content { get; set; }
    public bool AllowComment { get; set; }
    public UserPostPublicLevel PublicLevel { get; set; }
    public IEnumerable<AppFileInfo> AppFileInfos { get; set; }
}
