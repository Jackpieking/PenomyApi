namespace PenomyAPI.App.Common.IdGenerator.Snowflake;

public interface ISnowflakeIdGenerator
{
    /// <summary>
    ///     Generate a random id using Snowflake scheme.
    /// </summary>
    /// <returns>
    ///     The random id.
    /// </returns>
    public long Get();
}
