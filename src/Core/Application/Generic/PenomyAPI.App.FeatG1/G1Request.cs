using PenomyAPI.App.Common;
using System.ComponentModel.DataAnnotations;

namespace PenomyAPI.App.FeatG1;

public sealed class G1Request : IFeatureRequest<G1Response>
{
    public string MailTemplate { get; set; }

    public string RegisterPageLink { get; set; }

    [Required]
    public string Email { get; init; }
}
