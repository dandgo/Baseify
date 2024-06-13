using Baseify.Domain.Abstractions;

namespace Baseify.Domain.Bookings.Events;

public sealed record BookingRejectedDomainEvent(Guid BookingId) : IDomainEvent;
