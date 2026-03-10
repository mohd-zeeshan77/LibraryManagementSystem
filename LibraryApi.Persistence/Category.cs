namespace LibraryApi.Persistence;

public sealed class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public IList<Book> Books { get; set; } = [];
}
