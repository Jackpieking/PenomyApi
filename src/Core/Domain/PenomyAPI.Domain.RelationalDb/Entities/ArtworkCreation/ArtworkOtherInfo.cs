using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkOtherInfo : IEntity
{
    public long ArtworkId { get; set; }

    public long OtherInfoId { get; set; }

    public string InfoName { get; set; }

    public string Value { get; set; }

    #region Navigation
    public Artwork Artwork { get; set; }

    public OtherInfo OtherInfo { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int ValueLength = 200;
    }
    #endregion
}
