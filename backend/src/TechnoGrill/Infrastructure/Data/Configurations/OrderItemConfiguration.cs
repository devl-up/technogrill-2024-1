using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechnoGrill.Features.Orders.Entities;
using TechnoGrill.Features.Orders.Mementos;

namespace TechnoGrill.Infrastructure.Data.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemMemento>
{
    public void Configure(EntityTypeBuilder<OrderItemMemento> builder)
    {
        builder.ToTable(nameof(OrderItem));

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedNever();

        builder.Property(i => i.ProductId)
            .IsRequired();

        builder.Property(i => i.Amount)
            .IsRequired();

        builder.Property(i => i.OrderId)
            .IsRequired();

        builder.HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .IsRequired();
    }
}