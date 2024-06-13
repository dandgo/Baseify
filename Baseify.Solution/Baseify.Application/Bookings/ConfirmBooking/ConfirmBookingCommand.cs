using Baseify.Application.Abstractions.Clock;
using Baseify.Application.Abstractions.Messaging;
using Baseify.Domain.Abstractions;
using Baseify.Domain.Bookings;

namespace Baseify.Application.Bookings.ConfirmBooking;

public sealed record ConfirmBookingCommand(Guid BookingId) : ICommand;

internal sealed class ConfirmBookingCommandHandler : ICommandHandler<ConfirmBookingCommand>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmBookingCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ConfirmBookingCommand request,
        CancellationToken cancellationToken)
    {
        Booking? booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking is null)
        {
            return Result.Failure(BookingErrors.NotFound);
        }

        Result result = booking.Confirm(_dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}