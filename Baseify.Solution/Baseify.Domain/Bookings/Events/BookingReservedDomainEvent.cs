using Baseify.Domain.Abstractions;

namespace Baseify.Domain.Bookings.Events;

public sealed record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent;
