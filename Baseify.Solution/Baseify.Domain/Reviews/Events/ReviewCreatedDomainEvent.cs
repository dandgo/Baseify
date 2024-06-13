using Baseify.Domain.Abstractions;

namespace Baseify.Domain.Reviews.Events;

public sealed record ReviewCreatedDomainEvent(Guid ReviewId) : IDomainEvent;
