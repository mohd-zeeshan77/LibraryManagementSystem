using LibraryApi.Core.Dtos;
using LibraryApi.Core.Requests;
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
        userGroup.MapGet("{Id:int}", GetUserById);
        userGroup.MapGet("type/{Id:int}", GetMemberType);
        userGroup.MapPost("type/{TypeId:int}", AddUser);
        userGroup.MapPut("userid/{userid:int}", ChangeType);
        return endpoints;
    }

    public static Ok<IEnumerable<UserDto>> GetUsers(UserService service)
    {
        ArgumentNullException.ThrowIfNull(service);
        IEnumerable<UserDto> list = service.GetUsers();
        return TypedResults.Ok(list);
    }

    public static IResult GetMemberType(UserService service, int Id)
    {
        MemberTypeDto? member = service.GetMemberType(Id);
        return member is null ? TypedResults.NotFound() : TypedResults.Ok(member);
    }

    public static IResult GetUserById(UserService service, int Id)
    {
        UserDto? user = service.GetUserById(Id);
        return user is null ? TypedResults.NotFound() : TypedResults.Ok(user);
    }

    public static IResult AddUser(UserService service, int TypeId, CreateUserRequest request)
    {
        UserDto? user = service.AddUser(TypeId, request);
        return user is null ? TypedResults.NotFound() : TypedResults.Ok(user);
    }

    public static IResult ChangeType(UserService service, int userid, ChangeMemberTypeRequest request)
    {
        UserDto? user = service.EditPremium(userid, request);
        return user is null ? TypedResults.NotFound() : TypedResults.Ok(user);
    }
}
