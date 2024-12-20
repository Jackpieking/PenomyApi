using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.App.SM1;

public class SM1Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public UserProfile Result { get; set; }
    public SM1ResponseStatusCode StatusCode { get; set; }
}
