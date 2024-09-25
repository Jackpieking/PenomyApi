using System.ComponentModel.DataAnnotations;
using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG3;

public class FeatG3Request : IFeatureRequest<FeatG3Response>
{
    public bool Empty { get; set; } = true;
}
