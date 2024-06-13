using Baseify.Domain.Abstractions;

namespace Baseify.Domain.Bookings.Events;

public sealed record BookingCancelledDomainEvent(Guid BookingId) : IDomainEvent;
