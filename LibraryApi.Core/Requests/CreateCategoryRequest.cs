namespace LibraryApi.Core.Requests;

public sealed class CreateCategoryRequest(string Name)
{
    public string Name { get; }= Name;
}
