using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PenomyAPI.Domain.RelationalDb.Entities.Subscriptions;
using PenomyAPI.Persist.Postgres.Common.DatabaseConstants;
using PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

namespace TestFeatApp.Persist.Rel.PostgreSQL.EntityConfigurations.Subscriptions;

internal sealed class UserRegisterSubscriptionPlanEntityConfiguration
    : IEntityConfiguration<UserRegisterSubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<UserRegisterSubscriptionPlan> builder)
    {
        builder.ToTable("penomy_user_register_subscription_plan");

        builder.HasKey(plan => plan.Id);

        builder.Property(plan => plan.UserId).IsRequired();

        builder.Property(plan => plan.SubscriptionPlanId).IsRequired();

        builder
            .Property(plan => plan.SubscribedPrice)
            .HasColumnType(
                DatabaseNativeTypes.DECIMAL(
                    precision: SubscriptionPlan.MetaData.PricePrecision,
                    scale: SubscriptionPlan.MetaData.PriceScale
                )
            )
            .IsRequired();

        builder.Property(plan => plan.IsActive).IsRequired();

        builder
            .Property(plan => plan.StartedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        builder
            .Property(plan => plan.EndedAt)
            .HasColumnType(DatabaseNativeTypes.TIMESTAMPTZ)
            .IsRequired();

        #region Relationships
        builder
            .HasOne(userBan => userBan.SubscriptionPlan)
            .WithMany(subscriptionPlan => subscriptionPlan.UserRegisterSubscriptionPlans)
            .HasForeignKey(registerSubscriptionPlan => registerSubscriptionPlan.SubscriptionPlanId)
            .HasPrincipalKey(subscriptionPlan => subscriptionPlan.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(subscriptionPlan => subscriptionPlan.Subscriber)
            .WithMany(user => user.RegisterSubscriptionPlans)
            .HasForeignKey(subscriptionPlan => subscriptionPlan.UserId)
            .HasPrincipalKey(user => user.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        #endregion

        #region Indexes
        // builder.HasIndex(userRegisterSubscriptionPlan => new
        // {
        //     userRegisterSubscriptionPlan.UserId,
        //     userRegisterSubscriptionPlan.SubscriptionPlanId,
        //     userRegisterSubscriptionPlan.IsActive
        // });
        #endregion
    }
}
