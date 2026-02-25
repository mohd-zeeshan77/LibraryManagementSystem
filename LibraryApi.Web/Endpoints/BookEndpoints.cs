using LibraryApi.Core.Dtos;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryApi.Web.Endpoints
{
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
            return endpoints;
        }
        public static Ok<IEnumerable<BookDto>> GetBooks(BookService service,string? keyword)
        {
            IEnumerable<BookDto> list = service.GetBooks(keyword);
            return TypedResults.Ok(list);
        }
        public static IResult GetBook(BookService service, int Id)
        {
            BookDto? book  =  service.GetBook(Id);
            return book is null ? TypedResults.NotFound(): TypedResults.Ok(book);
        }
    }
}
