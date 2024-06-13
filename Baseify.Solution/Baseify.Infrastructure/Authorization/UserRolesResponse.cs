using Baseify.Domain.Users;

namespace Baseify.Infrastructure.Authorization;

internal sealed class UserRolesResponse
{
    public Guid UserId { get; init; }

    public List<Role> Roles { get; init; } = [];
}
