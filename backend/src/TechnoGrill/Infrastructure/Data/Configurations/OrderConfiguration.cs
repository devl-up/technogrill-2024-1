using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechnoGrill.Features.Orders.Entities;
using TechnoGrill.Features.Orders.Mementos;

namespace TechnoGrill.Infrastructure.Data.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<OrderMemento>
{
    public void Configure(EntityTypeBuilder<OrderMemento> builder)
    {
        builder.ToTable(nameof(Order));
        
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever();

        builder.Property(o => o.Status)
            .IsRequired();

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .IsRequired();
    }
}