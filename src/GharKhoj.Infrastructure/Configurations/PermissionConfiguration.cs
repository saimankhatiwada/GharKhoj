﻿using GharKhoj.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GharKhoj.Infrastructure.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(permission => permission.Id);

        builder.HasData(
            Permission.UsersReadSelf,
            Permission.UsersRead,
            Permission.UsersReadSingle,
            Permission.UsersUpdate,
            Permission.UsersDelete,
            Permission.PropertiesRead,
            Permission.PropertiesReadSingle,
            Permission.PropertiesCreate);
    }
}
