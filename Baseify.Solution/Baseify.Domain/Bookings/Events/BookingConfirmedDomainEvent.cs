using Baseify.Domain.Abstractions;

namespace Baseify.Domain.Bookings.Events;

public sealed record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent;
