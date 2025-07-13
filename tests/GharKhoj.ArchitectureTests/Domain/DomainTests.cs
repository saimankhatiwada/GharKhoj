using System.Reflection;
using FluentAssertions;
using GharKhoj.ArchitectureTests.Infrastructure;
using GharKhoj.Domain.Abstractions;
using NetArchTest.Rules;

namespace GharKhoj.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEvent_ShouldHave_DomainEventPostfix()
    {
        TestResult? result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(IEntity))
            .GetTypes();

        var failingTypes = (from entityType in entityTypes
                                let constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                                where !constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0)
                                select entityType)
                                .ToList();

        failingTypes.Should().BeEmpty();
    }
}
