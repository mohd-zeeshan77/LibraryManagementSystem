namespace LibraryApi.Persistence;

public sealed class MemberType
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int MaxBookAllowed { get; set; }
    public IList<User> Users { get; set; } = [];
}
