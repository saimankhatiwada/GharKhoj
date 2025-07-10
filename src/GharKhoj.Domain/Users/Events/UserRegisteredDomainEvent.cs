using GharKhoj.Domain.Abstractions;

namespace GharKhoj.Domain.Users.Events;

public sealed record UserRegisteredDomainEvent(UserId UserId) : IDomainEvent;
