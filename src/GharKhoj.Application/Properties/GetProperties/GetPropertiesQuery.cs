using GharKhoj.Application.Abstracions.Messaging;
using GharKhoj.Application.Common;
using GharKhoj.Domain.Properties;

namespace GharKhoj.Application.Properties.GetProperties;

public sealed record GetPropertiesQuery(string? Search, string? Sort, string? Fields, int? Page, int? PageSize) 
    : IQuery<PaginationResult<Property>>;
