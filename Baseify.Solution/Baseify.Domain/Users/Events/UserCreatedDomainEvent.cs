using Baseify.Domain.Abstractions;

namespace Baseify.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;
