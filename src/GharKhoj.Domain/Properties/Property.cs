using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Users;

namespace GharKhoj.Domain.Properties;

public sealed class Property : Entity<PropertyId>
{
    private Property(
        PropertyId id, 
        UserId userId, 
        Tittle tittle, 
        Description description, 
        Location location, 
        PropertyType type, 
        Money price) : base(id)
    {
        UserId = userId;
        Tittle = tittle;
        Description = description;
        Location = location;
        Type = type;
        Price = price;
    }

    private Property() { }
    public Tittle Tittle { get; private set; }
    public Description Description { get; private set; }
    public Location Location { get; private set; }
    public PropertyType Type { get; private set; }
    public Money Price { get; private set; }
    public UserId UserId { get; private set; }

    public static Property Create(
        UserId userId, 
        Tittle tittle, 
        Description description, 
        Location location, 
        PropertyType type, 
        Money price)
    {
        return new Property(
            PropertyId.New(), 
            userId, 
            tittle, 
            description, 
            location, 
            type, 
            price);
    }
}
