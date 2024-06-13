using Baseify.Domain.Abstractions;

namespace Baseify.Domain.Bookings.Events;

public sealed record BookingCompletedDomainEvent(Guid BookingId) : IDomainEvent;
