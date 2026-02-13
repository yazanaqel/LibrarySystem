using MediatR;

namespace LibrarySystem.Application.Features.Book.Search;

public record SearchBookCommand(string key, string value) : IRequest<List<Domain.Entities.Book>>;
