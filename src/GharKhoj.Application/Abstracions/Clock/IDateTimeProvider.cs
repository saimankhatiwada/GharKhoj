namespace GharKhoj.Application.Abstracions.Clock;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
