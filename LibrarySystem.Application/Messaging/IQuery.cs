using LibrarySystem.Domain.Shared;
using MediatR;

namespace LibrarySystem.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
public interface IQueryHandler<TQuery, TResponse>
: IRequestHandler<TQuery,Result<TResponse>>
where TQuery : IQuery<TResponse>
{
}

