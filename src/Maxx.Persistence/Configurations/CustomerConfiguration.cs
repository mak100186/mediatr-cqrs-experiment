namespace Maxx.Persistence.Configurations;

using Constants;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CustomerConfiguration
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(TableNames.Customers);

        builder.HasKey(x => x.Id);
    }
}