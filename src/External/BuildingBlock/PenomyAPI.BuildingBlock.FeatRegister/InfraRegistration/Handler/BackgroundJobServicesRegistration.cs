using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BackgroundJob.Quartz;
using PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Common;
using PenomyAPI.Infra.Configuration.Options;
using Quartz;
using Quartz.AspNetCore;

namespace PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Handler;

public class BackgroundJobServicesRegistration : IServiceRegistration
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        var appQuartzOptions = configuration.GetRequiredSection("Quartz").Get<AppQuartzOptions>();
        var dbOptions = configuration
            .GetRequiredSection("Database")
            .GetRequiredSection("PenomyDev")
            .Get<AppDbContextOptions>();
        ;

        // if you are using persistent job store, you might want to alter some options
        services.Configure<QuartzOptions>(options =>
        {
            options.Scheduling.IgnoreDuplicates = appQuartzOptions.Scheduling.IgnoreDuplicates; // default: false

            options.Scheduling.OverWriteExistingData = appQuartzOptions
                .Scheduling
                .OverWriteExistingData; // default: true
        });

        services
            .AddDbContextPool<AppQuartzContext>(config =>
            {
                config
                    .UseNpgsql(
                        dbOptions.ConnectionString,
                        dbOption =>
                        {
                            dbOption
                                .EnableRetryOnFailure(dbOptions.EnableRetryOnFailure)
                                .CommandTimeout(dbOptions.CommandTimeOutInSecond);
                        }
                    )
                    .EnableSensitiveDataLogging(dbOptions.EnableSensitiveDataLogging)
                    .EnableDetailedErrors(dbOptions.EnableDetailedErrors)
                    .EnableThreadSafetyChecks(dbOptions.EnableThreadSafetyChecks)
                    .EnableServiceProviderCaching(dbOptions.EnableServiceProviderCaching);
            })
            .AddQuartz(config =>
            {
                // Quartz instance Id
                config.SchedulerId = appQuartzOptions.SchedulerId;

                // Change from the default of 1
                config.MaxBatchSize = appQuartzOptions.MaxBatchSize;

                config.UseSimpleTypeLoader();

                config.UseDefaultThreadPool(threadPool =>
                    threadPool.MaxConcurrency = appQuartzOptions.DefaultThreadPool.MaxConcurrency
                );

                config.UsePersistentStore(persistenceStoreConfig =>
                {
                    persistenceStoreConfig.RetryInterval = TimeSpan.FromSeconds(
                        dbOptions.CommandTimeOutInSecond
                    );

                    persistenceStoreConfig.UseProperties = appQuartzOptions
                        .PersistentStore
                        .UseProperties;

                    persistenceStoreConfig.UseSystemTextJsonSerializer();

                    persistenceStoreConfig.UsePostgres(
                        appQuartzOptions.PersistentStore.ConnectionString
                    );
                });

                #region Jobs

                #region HelloWorldJob
                var helloWorldJobKey = new JobKey("HelloWorldJob");

                config
                    .AddJob<HelloWorldJob>(
                        helloWorldJobKey,
                        config =>
                        {
                            config.DisallowConcurrentExecution().StoreDurably().RequestRecovery();
                        }
                    )
                    .AddTrigger(triggerConfig =>
                        triggerConfig
                            .ForJob(helloWorldJobKey)
                            .WithIdentity("HelloWorldJobTrigger")
                            .WithSimpleSchedule(scheduleConfig =>
                            {
                                scheduleConfig
                                    .WithIntervalInSeconds(10)
                                    .RepeatForever()
                                    .WithMisfireHandlingInstructionIgnoreMisfires();
                            })
                    );
                #endregion

                #endregion
            })
            .AddQuartzServer(config =>
            {
                // when shutting down we want jobs to complete gracefully
                config.WaitForJobsToComplete = appQuartzOptions.Server.WaitForJobsToComplete;

                // wait for the application to start appropriately
                config.AwaitApplicationStarted = appQuartzOptions.Server.AwaitApplicationStarted;
            });
    }
}
