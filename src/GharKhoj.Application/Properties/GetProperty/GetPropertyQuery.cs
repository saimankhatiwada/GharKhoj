using GharKhoj.Application.Abstracions.Messaging;
using GharKhoj.Domain.Properties;

namespace GharKhoj.Application.Properties.GetProperty;

public sealed record GetPropertyQuery(string PropertyId, string? Fields) : IQuery<Property>;
