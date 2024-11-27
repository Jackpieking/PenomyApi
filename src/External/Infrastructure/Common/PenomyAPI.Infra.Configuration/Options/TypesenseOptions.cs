using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public class TypesenseOptions : AppOptions
{
    public string ApiKey { get; init; }

    public IEnumerable<NodeOption> Nodes { get; init; } = [];

    public sealed class NodeOption
    {
        public string Host { get; init; }

        public string Port { get; init; }

        public string Protocol { get; init; }
    }

    public override void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection("Typesense").Bind(this);
    }
}
