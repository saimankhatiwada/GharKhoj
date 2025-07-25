﻿using Microsoft.AspNetCore.Authorization;

namespace GharKhoj.Infrastructure.Authorization;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) : base(permission)
    {
    }
}
