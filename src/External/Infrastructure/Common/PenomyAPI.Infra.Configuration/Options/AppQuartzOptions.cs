namespace PenomyAPI.Infra.Configuration.Options;

public class AppQuartzOptions
{
    public string SchedulerId { get; init; }

    public int MaxBatchSize { get; init; }

    public DefaultThreadPoolOption DefaultThreadPool { get; init; } = new();

    public PersistentStoreOption PersistentStore { get; init; } = new();

    public ServerOption Server { get; init; } = new();

    public SchedulingOption Scheduling { get; init; } = new();

    public class DefaultThreadPoolOption
    {
        public int MaxConcurrency { get; init; }
    }

    public class ServerOption
    {
        public bool WaitForJobsToComplete { get; init; }

        public bool AwaitApplicationStarted { get; init; }
    }

    public class PersistentStoreOption
    {
        public string ConnectionString { get; init; }

        public bool UseProperties { get; init; }
    }

    public class SchedulingOption
    {
        public bool IgnoreDuplicates { get; init; }

        public bool OverWriteExistingData { get; init; }
    }
}
