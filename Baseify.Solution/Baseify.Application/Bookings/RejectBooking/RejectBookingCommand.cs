﻿using Baseify.Application.Abstractions.Clock;
using Baseify.Application.Abstractions.Messaging;
using Baseify.Domain.Abstractions;
using Baseify.Domain.Bookings;

namespace Baseify.Application.Bookings.RejectBooking;

public sealed record RejectBookingCommand(Guid BookingId) : ICommand;

internal sealed class RejectBookingCommandCommandHandler : ICommandHandler<RejectBookingCommand>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectBookingCommandCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        RejectBookingCommand request,
        CancellationToken cancellationToken)
    {
        Booking? booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking is null)
        {
            return Result.Failure(BookingErrors.NotFound);
        }

        Result result = booking.Reject(_dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}