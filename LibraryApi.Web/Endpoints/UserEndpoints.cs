using LibraryApi.Core.Dtos;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryApi.Web.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserGroup(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGroup("users");
    }
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        IEndpointRouteBuilder userGroup = endpoints.MapUserGroup();
        userGroup.MapGet("", GetUsers);
        userGroup.MapGet("type/{Id:int}", GetMemberType);
        return endpoints;
    }
    public static Ok<IEnumerable<UserDto>> GetUsers(UserService service)
    {
        ArgumentNullException.ThrowIfNull(service);
        IEnumerable<UserDto> list = service.GetUsers();
        return TypedResults.Ok(list);
    }
    public static IResult GetMemberType(UserService service,int Id)
    {
        MemberTypeDto? member = service.GetMemberType(Id);
        return member is null ? TypedResults.NotFound():TypedResults.Ok(member);
    }
}
