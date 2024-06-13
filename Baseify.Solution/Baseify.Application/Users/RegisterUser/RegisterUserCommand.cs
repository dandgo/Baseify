using Baseify.Application.Abstractions.Authentication;
using Baseify.Application.Abstractions.Messaging;
using Baseify.Domain.Abstractions;
using Baseify.Domain.Users;
using FluentValidation;

namespace Baseify.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
        string Email,
        string FirstName,
        string LastName,
        string Password) : ICommand<Guid>;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();

        RuleFor(c => c.LastName).NotEmpty();

        RuleFor(c => c.Email).EmailAddress();

        RuleFor(c => c.Password).NotEmpty().MinimumLength(5);
    }
}


internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IAuthenticationService authenticationService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        /*
         * TODO: Add validations like if the email already exists but don't inform the user the email already exists to avoid security issues.
         */

        var user = User.Create(
            new FirstName(request.FirstName),
            new LastName(request.LastName),
            new Email(request.Email));

        string identityId = await _authenticationService.RegisterAsync(
            user,
            request.Password,
            cancellationToken);

        user.SetIdentityId(identityId);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
