using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;

namespace PenowmyAPI.APP.Chat2;

public class Chat2Response : IFeatureResponse
{
    public List<ChatGroup> ChatGroups { get; set; }
    private bool IsSuccess { get; set; }

    public Chat2ResponseStatusCode StatusCode { get; set; }
}
