using GharKhoj.Domain.Abstractions;
using MediatR;

namespace GharKhoj.Application.Abstracions.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface  ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{
}

public interface IBaseCommand
{
}
