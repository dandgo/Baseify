﻿using Microsoft.AspNetCore.Authorization;

namespace Baseify.Infrastructure.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(permission)
    {
    }
}
