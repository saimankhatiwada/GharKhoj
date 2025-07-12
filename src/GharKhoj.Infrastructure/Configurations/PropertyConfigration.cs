using GharKhoj.Domain.Properties;
using GharKhoj.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GharKhoj.Infrastructure.Configurations;

internal sealed class PropertyConfigration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("properties");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasMaxLength(100)
            .HasConversion(id => id.Value, value => new PropertyId(value));

        builder.Property(p => p.Tittle)
            .HasMaxLength(100)
            .HasConversion(tittle => tittle.Value, value => new Tittle(value));

        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .HasConversion(description => description.Value, value => new Description(value));

        builder.OwnsOne(p => p.Location);

        builder.OwnsOne(p => p.Price, priceBuiler => priceBuiler.Property(money => money.Currency)
            .HasConversion(currency => currency.Code, code => Currency.FromCode(code)));

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
