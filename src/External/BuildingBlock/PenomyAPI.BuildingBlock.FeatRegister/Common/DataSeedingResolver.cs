using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.BuildingBlock.FeatRegister.Common;

public static class DataSeedingResolver
{
    public static async void Resolve(IServiceProvider value)
    {
        if (Equals(value, null))
        {
            throw new ArgumentNullException("value");
        }

        var serviceProvider = value.CreateScope().ServiceProvider;

        await SeedDataAsync(serviceProvider);
    }

    private static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        IUnitOfWork unitOfWork = serviceProvider.GetService<IUnitOfWork>();

        IDataSeedingRepository seedingRepository = unitOfWork.DataSeedingRepository;

        var hasSeedData = await seedingRepository.HasSeedDataAsync(CancellationToken.None);

        if (hasSeedData)
        {
            return;
        }

        var seedingResult = await seedingRepository.SeedDataAsync(CancellationToken.None);

        if (!seedingResult)
        {
            throw new InvalidOperationException("Something wrong is happened when seeding data");
        }
    }
}
