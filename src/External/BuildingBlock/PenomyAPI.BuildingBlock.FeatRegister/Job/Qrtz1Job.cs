using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using Quartz;

namespace PenomyAPI.BuildingBlock.FeatRegister.Job;

public class Qrtz1Job : IJob
{
    public const string JobKey = nameof(Qrtz1Job);
    public const string TriggerKey = nameof(Qrtz1Job);
    public const int RepeatAsIntervalInHours = 6;
    private const int MaxRetentionDayOfGuestHistory = 14;
    private readonly AppDbContext _context;

    public Qrtz1Job(AppDbContext context)
    {
        _context = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var guestActives = _context.Set<GuestTracking>().AsNoTracking().Select(guest => guest);

        foreach (var guestActive in guestActives)
        {
            var numberOfDaysHasPassed = DateTime.UtcNow.Subtract(guestActive.LastActiveAt).Days;

            if (numberOfDaysHasPassed > MaxRetentionDayOfGuestHistory)
            {
                await _context
                    .Database.CreateExecutionStrategy()
                    .ExecuteAsync(async () =>
                    {
                        await using var dbTransaction =
                            await _context.Database.BeginTransactionAsync(
                                context.CancellationToken
                            );

                        try
                        {
                            await _context
                                .Set<GuestArtworkViewHistory>()
                                .Where(viewHistory => viewHistory.GuestId == guestActive.GuestId)
                                .ExecuteDeleteAsync(context.CancellationToken);

                            await _context
                                .Set<GuestTracking>()
                                .Where(guest => guest.GuestId == guestActive.GuestId)
                                .ExecuteUpdateAsync(
                                    setPropCall =>
                                        setPropCall.SetProperty(
                                            guest => guest.LastActiveAt,
                                            guest => DateTime.UtcNow
                                        ),
                                    context.CancellationToken
                                );

                            await dbTransaction.CommitAsync(context.CancellationToken);
                        }
                        catch
                        {
                            await dbTransaction.RollbackAsync(context.CancellationToken);
                        }
                    });
            }
        }
    }
}
