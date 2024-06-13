using Baseify.Application.Abstractions.Clock;
using Baseify.Application.Abstractions.Messaging;
using Baseify.Domain.Abstractions;
using Baseify.Domain.Bookings;
using Baseify.Domain.Reviews;

namespace Baseify.Application.Reviews.AddReview;

public sealed record AddReviewCommand(Guid BookingId, int Rating, string Comment) : ICommand;

internal sealed class AddReviewCommandHandler(
    IBookingRepository bookingRepository,
    IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddReviewCommand>
{
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IReviewRepository _reviewRepository = reviewRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<Result> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        Booking? booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking is null)
        {
            return Result.Failure(BookingErrors.NotFound);
        }

        Result<Rating> ratingResult = Rating.Create(request.Rating);

        if (ratingResult.IsFailure)
        {
            return Result.Failure(ratingResult.Error);
        }

        Result<Review> reviewResult = Review.Create(
            booking,
            ratingResult.Value,
            new Comment(request.Comment),
            _dateTimeProvider.UtcNow);

        if (reviewResult.IsFailure)
        {
            return Result.Failure(reviewResult.Error);
        }

        _reviewRepository.Add(reviewResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
