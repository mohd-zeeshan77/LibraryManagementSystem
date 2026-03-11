using LibraryApi.Core.Dtos;
using LibraryApi.Core.Requests;
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
        issuedGroup.MapPost("book/{bookid:int}/user/{userid:int}", AddIssuedBook);
        issuedGroup.MapPatch("book/{bookid:int}/user/{userid:int}/returned", CheckIsReturn);
        issuedGroup.MapPatch("book/{bookid:int}/user/{userid:int}/renew", UpdateRenew);
        return endpoints;
    }

    private static Ok<IEnumerable<BookIssedDto>> GetIssuedBookList(IssuedBookservice service,
                                                                   string? name,
                                                                   string? bookname,
                                                                   bool? isReturned,
                                                                   DateOnly? issueDate,
                                                                   DateOnly? returnDate,
                                                                   DateOnly? renewDate,
                                                                   DateOnly? startDate,
                                                                   DateOnly? endDate,
                                                                   int? issueYear)
    {
        IEnumerable<BookIssedDto> books = service.GetIssuedBooks(name,
                                                                bookname,
                                                                isReturned,
                                                                issueDate,
                                                                returnDate,
                                                                renewDate,
                                                                startDate,
                                                                endDate,
                                                                issueYear);
        return TypedResults.Ok(books);
    }

    private static IResult AddIssuedBook(IssuedBookservice service, int bookid, int userid,
        CreateIssuedBookRequest request)
    {
        BookIssedDto? issuedbook = service.AddIssuedBook(bookid, userid, request);
        return issuedbook is null ? TypedResults.NotFound() : TypedResults.Ok(issuedbook);
    }

    private static IResult CheckIsReturn(IssuedBookservice service, int bookid, int userid, BoolPatchRequest request)
    {
        BookIssedDto? issued = service.IsReturn(bookid, userid, request);
        return issued is null ? TypedResults.NotFound() : TypedResults.Ok(issued);
    }

    private static IResult UpdateRenew(IssuedBookservice service, int bookid, int userid, BoolPatchRequest request)
    {
        BookIssedDto? issue = service.UpdateRenew(bookid, userid, request);
        return issue is null ? TypedResults.NotFound() : TypedResults.Ok(issue);
    }
}
