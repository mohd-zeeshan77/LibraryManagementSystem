using LibraryApi.Core.Dtos;
using LibraryApi.Core.Requests;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryApi.Web.Endpoints;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryGroup(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGroup("categories");
    }

    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        IEndpointRouteBuilder categoryGroup = endpoints.MapCategoryGroup();
        categoryGroup.MapGet("", GetCategories);
        categoryGroup.MapGet("{Id:int}", GetCategoryBook);
        categoryGroup.MapPost("", AddCategory);
        return endpoints;
    }

    private static Ok<IEnumerable<CategoryDto>> GetCategories(CategoryService service)
    {
        IEnumerable<CategoryDto> categories = service.GetCategories();
        return TypedResults.Ok(categories);
    }

    private static IResult GetCategoryBook(CategoryService service, int Id)
    {
        CategoryBookDto? dto = service.GetCategoryBook(Id);
        return dto is null ? TypedResults.NotFound() : TypedResults.Ok(dto);
    }

    public static IResult AddCategory(CategoryService service, CreateCategoryRequest request)
    {
        CategoryDto? category = service.AddCategory(request);
        return category is null ? TypedResults.NotFound() : TypedResults.Ok(category);
    }
}
