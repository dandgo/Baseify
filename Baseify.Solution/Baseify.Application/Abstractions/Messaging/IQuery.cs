using Baseify.Domain.Abstractions;
using MediatR;

namespace Baseify.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
