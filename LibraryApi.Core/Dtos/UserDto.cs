namespace LibraryApi.Core.Dtos;

public sealed class UserDto(int Id, string Name, string TypeName,int MaxBookAllowed)
{
    public int Id { get; } = Id;
    public string Name { get; } = Name;
    public string TypeName { get; } = TypeName;
    public int MaxBookAllowed { get; } = MaxBookAllowed;
}
