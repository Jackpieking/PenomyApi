using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using System.Collections.Concurrent;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.TestingSnowflake
{
    public sealed class Art4TestingHandler : IFeatureHandler<Art4TestingRequest, Art4TestingResponse>
    {
        private static readonly ConcurrentDictionary<long, long> randomIds = new();
        private static readonly IList<long> failToAddedIds = new List<long>();
        private readonly Lazy<ISnowflakeIdGenerator> _snowflakeIdGenerator;

        public Art4TestingHandler(Lazy<ISnowflakeIdGenerator> snowflakeIdGenerator)
        {
            _snowflakeIdGenerator = snowflakeIdGenerator;
        }

        public Task<Art4TestingResponse> ExecuteAsync(Art4TestingRequest request, CancellationToken ct)
        {
            var id = _snowflakeIdGenerator.Value.Get();

            var result = randomIds.TryAdd(id, id);

            if (result)
            {
                return Task.FromResult(new Art4TestingResponse
                {
                    IsSuccess = true,
                });
            }

            failToAddedIds.Add(id);

            return Task.FromResult(new Art4TestingResponse
            {
                IsSuccess = false,
            });
        }
    }
}
