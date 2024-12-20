namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;

public class G5CreatorProfileReadModel
{
    public long CreatorId { get; set; }

    public string CreatorName { get; set; }

    public string CreatorAvatarUrl { get; set; }

    public long CreatorTotalFollowers { get; set; }
}
