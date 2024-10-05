using IdGen;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Infra.Configuration.Options;

namespace SnowflakeIdGenerator;

public sealed class AppSnowflakeIdGenerator : ISnowflakeIdGenerator
{
    private readonly IdGenerator _idGenerator;

    public AppSnowflakeIdGenerator(SnowflakeIdOptions options)
    {
        // Init the IdGenerator.
        var timeSource = new DefaultTimeSource(epoch: options.GetEpochDateTimeOffset());

        var idGeneratorOptions = new IdGeneratorOptions(
            idStructure: IdStructure.Default,
            timeSource: timeSource);

        _idGenerator = new IdGenerator(options.GeneratorId, idGeneratorOptions);
    }

    public long Get()
    {
        _idGenerator.TryCreateId(out long id);

        return id;
    }
}
