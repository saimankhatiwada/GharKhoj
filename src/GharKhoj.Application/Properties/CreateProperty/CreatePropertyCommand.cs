using GharKhoj.Application.Abstracions.Messaging;

namespace GharKhoj.Application.Properties.CreateProperty;

public sealed record CreatePropertyCommand(
    string UserId, 
    string Tittle, 
    string Desciption, 
    string Country, 
    string State, 
    string City, 
    string Street, 
    int Type, 
    string Currency, 
    decimal Amount) : ICommand<string>;
