using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;
using System;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class SnowflakeIdOptions : AppOptions
{
    /// <summary>
    ///     The root section is the top-level section the appsettings
    ///     which mean is does not have parent.
    /// </summary>
    public const string RootSectionName = "SnowflakeId";

    public int GeneratorId { get; init; }

    public int EpochYear { get; init; }

    public int EpochMonth { get; init; }

    public int EpochDay { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration
            .GetRequiredSection(key: RootSectionName)
            .Bind(this);
    }

    public DateTimeOffset GetEpochDateTimeOffset()
    {
        return new DateTimeOffset(EpochYear, EpochMonth, EpochDay, 0, 0, 0, TimeSpan.Zero);
    }
}