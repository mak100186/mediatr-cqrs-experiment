namespace Maxx.Persistence.Configurations;

using Constants;

using Domain.Entities;
using Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CustomerConfiguration
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(TableNames.Customers);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Email)
            .HasConversion(x => x.Value, v => Email.Create(v).Value);

        builder
            .Property(x => x.FirstName)
            .HasConversion(x => x.Value, v => FirstName.Create(v).Value)
            .HasMaxLength(FirstName.MaxLength);

        builder
            .Property(x => x.LastName)
            .HasConversion(x => x.Value, v => LastName.Create(v).Value)
            .HasMaxLength(LastName.MaxLength);

        builder.HasIndex(x => x.Email).IsUnique();
    }
}