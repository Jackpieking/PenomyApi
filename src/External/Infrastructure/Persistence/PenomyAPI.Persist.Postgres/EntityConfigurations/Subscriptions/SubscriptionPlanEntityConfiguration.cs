using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Subscriptions;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Subscriptions;

internal sealed class SubscriptionPlanEntityConfiguration : IEntityConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.ToTable("penomy_subscription_plan");

        builder.HasKey(subscriptionPlan => subscriptionPlan.Id);

        builder
            .Property(subscriptionPlan => subscriptionPlan.PlanName)
            .HasMaxLength(SubscriptionPlan.MetaData.PlanNameLength)
            .IsRequired();

        builder
            .Property(subscriptionPlan => subscriptionPlan.Price)
            .HasColumnType(
                DatabaseNativeTypes.DECIMAL(
                    precision: SubscriptionPlan.MetaData.PricePrecision,
                    scale: SubscriptionPlan.MetaData.PriceScale
                )
            )
            .IsRequired();

        builder
            .Property(subscriptionPlan => subscriptionPlan.Description)
            .HasMaxLength(SubscriptionPlan.MetaData.DescriptionLength)
            .IsRequired();

        builder.Property(subscriptionPlan => subscriptionPlan.DurationInDays).IsRequired();

        builder.Property(subscriptionPlan => subscriptionPlan.CreatedBy).IsRequired();

        builder
            .Property(subscriptionPlan => subscriptionPlan.CreatedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(subscriptionPlan => subscriptionPlan.Creator)
            .WithMany(systemAccount => systemAccount.CreatedSubscriptionPlans)
            .HasForeignKey(subscriptionPlan => subscriptionPlan.CreatedBy)
            .HasPrincipalKey(systemAccount => systemAccount.Id)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(subscriptionPlan => subscriptionPlan.CreatedBy);
        #endregion
    }
}
