using Baseify.Application.Abstractions.Authentication;
using Baseify.Application.Abstractions.Messaging;
using Baseify.Domain.Abstractions;
using Baseify.Domain.Users;

namespace Baseify.Application.Users.LogInUser;

public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<AccessTokenResponse>;

internal sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, AccessTokenResponse>
{
    private readonly IJwtService _jwtService;

    public LogInUserCommandHandler(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task<Result<AccessTokenResponse>> Handle(
        LogInUserCommand request,
        CancellationToken cancellationToken)
    {
        Result<string> result = await _jwtService.GetAccessTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);
        }

        return new AccessTokenResponse(result.Value);
    }
}