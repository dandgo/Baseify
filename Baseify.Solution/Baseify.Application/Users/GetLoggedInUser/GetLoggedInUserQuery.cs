using System.Data;
using Baseify.Application.Abstractions.Authentication;
using Baseify.Application.Abstractions.Data;
using Baseify.Application.Abstractions.Messaging;
using Baseify.Domain.Abstractions;
using Dapper;

namespace Baseify.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;

internal sealed class GetLoggedInUserQueryHandler
    : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IUserContext _userContext;

    public GetLoggedInUserQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        IUserContext userContext)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _userContext = userContext;
    }

    public async Task<Result<UserResponse>> Handle(
        GetLoggedInUserQuery request,
        CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                               id AS Id,
                               first_name AS FirstName,
                               last_name AS LastName,
                               email AS Email
                           FROM users
                           WHERE identity_id = @IdentityId
                           """;

        UserResponse user = await connection.QuerySingleAsync<UserResponse>(
            sql,
            new
            {
                _userContext.IdentityId
            });

        return user;
    }
}
