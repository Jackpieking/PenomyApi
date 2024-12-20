using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;

namespace PenomyAPI.App.FeatG5.OtherHandlers.UserPreferences;

public class G5UserPreferenceResponse : IFeatureResponse
{
    public G5UserArtworkPreferenceReadModel UserArtworkPreference { get; set; }
}
