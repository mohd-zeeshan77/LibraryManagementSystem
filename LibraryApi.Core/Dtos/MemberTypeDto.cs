namespace LibraryApi.Core.Dtos;

public sealed class MemberTypeDto(int Id, string Name, IReadOnlyList<UserDto> Users)
{
    public int Id { get; } = Id;
    public string Name { get; } = Name;
    public IReadOnlyList<UserDto> Users { get; } = Users;
}
