using GharKhoj.Domain.Abstractions;
using MediatR;

namespace GharKhoj.Application.Abstracions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
