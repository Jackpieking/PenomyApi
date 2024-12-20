using System;
using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class SnowflakeIdOptions : AppOptions
{
    public int GeneratorId { get; init; }

    public int EpochYear { get; init; }

    public int EpochMonth { get; init; }

    public int EpochDay { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection(key: "SnowflakeId").Bind(this);
    }

    public DateTimeOffset GetEpochDateTimeOffset()
    {
        return new DateTimeOffset(EpochYear, EpochMonth, EpochDay, 0, 0, 0, TimeSpan.Zero);
    }
}
