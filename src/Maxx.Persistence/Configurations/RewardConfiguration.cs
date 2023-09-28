namespace Maxx.Persistence.Configurations;

using Constants;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class RewardConfiguration
{
    public void Configure(EntityTypeBuilder<Reward> builder)
    {
        builder.ToTable(TableNames.Rewards);

        builder.HasKey(x => x.Id);

        builder
            .Property(e => e.MetaData)
            .HasConversion<string>();
    }
}