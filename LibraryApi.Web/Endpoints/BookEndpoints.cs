using LibraryApi.Core.Dtos;
using LibraryApi.Core.Requests;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryApi.Web.Endpoints;

public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookGroup(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGroup("books");
    }

    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        IEndpointRouteBuilder bookGroup = endpoints.MapBookGroup();
        bookGroup.MapGet("", GetBooks);
        bookGroup.MapGet("{Id:int}", GetBook);
        bookGroup.MapPost("category/{categoryId:int}", AddBook);
        return endpoints;
    }

    public static Ok<IEnumerable<BookDto>> GetBooks(BookService service, string? keyword)
    {
        IEnumerable<BookDto> list = service.GetBooks(keyword);
        return TypedResults.Ok(list);
    }

    public static IResult GetBook(BookService service, int Id)
    {
        BookDto? book = service.GetBook(Id);
        return book is null ? TypedResults.NotFound() : TypedResults.Ok(book);
    }

    public static IResult AddBook(BookService service, int categoryId, CreateBookRequest request)
    {
        BookDto? book = service.AddBook(categoryId, request);
        return book is null ? TypedResults.NotFound() : TypedResults.Ok(book);
    }
}
