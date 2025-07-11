using GharKhoj.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GharKhoj.Infrastructure.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(r => r.Id);

        builder.HasMany(r => r.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder.HasData(
            Role.Seeker,
            Role.Broker,
            Role.SuperAdmin);
    }
}
