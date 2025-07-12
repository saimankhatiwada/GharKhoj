using GharKhoj.Domain.Abstractions;

namespace GharKhoj.Domain.Properties;

public static class PropertyErrors
{
    public static Error NotFound(string Value) => new(
        "Properties.NotFound",
        $"The property with the Id = '{Value}' was not found");
}
