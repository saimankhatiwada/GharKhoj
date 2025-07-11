using GharKhoj.Domain.Abstractions;
using MediatR;

namespace GharKhoj.Application.Abstracions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
