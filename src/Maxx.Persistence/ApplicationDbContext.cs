namespace Maxx.Persistence;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Customer>()
            .HasMany(customer => customer.Rewards)
            .WithMany(reward => reward.Customers)
            .UsingEntity(
                nameof(Entitlement),
                l => l.HasOne(typeof(Customer)).WithMany().HasForeignKey(nameof(Entitlement.CustomerId)).HasPrincipalKey(nameof(Customer.Id)),
                r => r.HasOne(typeof(Reward)).WithMany().HasForeignKey(nameof(Entitlement.RewardId)).HasPrincipalKey(nameof(Reward.Id)),
                j => j.HasKey(nameof(Entitlement.CustomerId), nameof(Entitlement.CustomerId)));

        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}