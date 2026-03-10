namespace LibraryApi.Core.Requests;

public sealed class CreateUserRequest(string Name)
{
    public string Name { get; } = Name;
}
