using System.Reflection;
using GharKhoj.Application.Abstracions.Messaging;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Infrastructure;

namespace GharKhoj.ArchitectureTests.Infrastructure;
public class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(IBaseCommand).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(IEntity).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
