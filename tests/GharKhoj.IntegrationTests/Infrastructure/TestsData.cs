using System.Threading;
using GharKhoj.Api.Models.Properties;
using GharKhoj.Domain.Properties;

namespace GharKhoj.IntegrationTests.Infrastructure;

public static class TestsData
{
    public static class Properties
    {
        public static PropertyDto CreateMockProperty() => new()
        {
            Id = PropertyId.New().Value,
            Tittle = "Mock Property",
            Description = "This is a mock property for testing purposes.",
            Location = new LocationDto("Mock Country", "Mock City", "Mock State", "123 Mock St"),
            Type = (int)PropertyType.Villa,
            Price = new MoneyDto(1000, "NPR")
        };
    }
}
