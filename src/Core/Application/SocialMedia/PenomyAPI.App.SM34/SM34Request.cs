using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;

namespace PenomyAPI.App.SM34;

public class SM34Request : IFeatureRequest<SM34Response>
{
    public long UserId { get; set; }
    public long GroupPostId { get; set; }
    public long GroupId { get; set; }
    public string Content { get; set; }
    public bool AllowComment { get; set; }
    public IEnumerable<AppFileInfo> AppFileInfos { get; set; }
}
