using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

/// <summary>
///     This class stores overview statistics data of a specific artwork.
/// </summary>
public sealed class ArtworkStatistic : IEntity
{
    public long ArtworkId { get; set; }

    public long TotalViews { get; set; }

    public long TotalComments { get; set; }

    public long TotalFollowers { get; set; }

    public long TotalFavorites { get; set; }

    /// <summary>
    ///     Represents the total sum of all star ratings that this artwork has received from users.
    ///     This value reflects the cumulative star ratings provided by all users who have rated
    ///     this artwork.
    /// </summary>
    public long TotalStarRates { get; set; }

    /// <summary>
    ///     The total number of users who have rated the artwork.
    /// </summary>
    public long TotalUsersRated { get; set; }

    /// <summary>
    ///     Represents the average star rating of this artwork.
    ///     This value is calculated as the total sum of all star ratings
    ///     (<see cref="TotalStarRates"/>) divided by the total number of users
    ///     who have rated the artwork (<see cref="TotalUsersRated"/>).
    /// </summary>
    /// <value>
    ///     The result of (TotalStarRates / TotalUsersRated).
    /// </value>
    public double AverageStarRate { get; set; }

    #region Navigation
    public Artwork Artwork { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
