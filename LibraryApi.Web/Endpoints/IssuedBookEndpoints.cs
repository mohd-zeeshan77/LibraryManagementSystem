using LibraryApi.Core.Dtos;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryApi.Web.Endpoints;

public static class IssuedBookEndpoints
{
    public static IEndpointRouteBuilder MapIssuedBookGroup(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGroup("issued");
    }
    public static IEndpointRouteBuilder MapIssuedBookEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        IEndpointRouteBuilder issuedGroup = endpoints.MapIssuedBookGroup();
        issuedGroup.MapGet("", GetIssuedBookList);
        return endpoints;
    }
    private static Ok<IEnumerable<BookIssedDto>> GetIssuedBookList(IssuedBookservice service,string? name,string? bookname)
    {
        IEnumerable<BookIssedDto> books = service.GetIssuedBook(name, bookname);
        return TypedResults.Ok(books);
    }
}
