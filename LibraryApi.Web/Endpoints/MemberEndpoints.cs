using LibraryApi.Core.Dtos;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryApi.Web.Endpoints;

public static class MemberEndpoints
{
    public static IEndpointRouteBuilder MapMemberGroup(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGroup("members");
    }
    public static IEndpointRouteBuilder MapMemberEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        IEndpointRouteBuilder memberGroup = endpoints.MapMemberGroup();
        memberGroup.MapGet("", GetMembers);
        return endpoints;
    }
    private static Ok<IEnumerable<MemberDto>> GetMembers(MemberService service)
    {
        IEnumerable<MemberDto> member = service.GetMemberList();
        return TypedResults.Ok(member);
    }
}
