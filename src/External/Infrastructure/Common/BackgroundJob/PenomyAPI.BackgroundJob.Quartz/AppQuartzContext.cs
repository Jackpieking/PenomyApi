using AppAny.Quartz.EntityFrameworkCore.Migrations;
using AppAny.Quartz.EntityFrameworkCore.Migrations.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace PenomyAPI.BackgroundJob.Quartz;

public class AppQuartzContext : DbContext
{
    public AppQuartzContext(DbContextOptions<AppQuartzContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Prefix and schema can be passed as parameters
        modelBuilder.AddQuartz(builder => builder.UsePostgreSql());
    }
}
