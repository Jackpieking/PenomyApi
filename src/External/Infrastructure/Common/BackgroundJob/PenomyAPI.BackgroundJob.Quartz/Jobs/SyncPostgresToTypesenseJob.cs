using System.Threading.Tasks;
using Quartz;

namespace PenomyAPI.BackgroundJob.Quartz.Jobs;

public class SyncPostgresToTypesenseJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        throw new System.NotImplementedException();
    }
}
