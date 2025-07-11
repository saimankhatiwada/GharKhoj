using GharKhoj.Application.Abstracions.Clock;

namespace GharKhoj.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
