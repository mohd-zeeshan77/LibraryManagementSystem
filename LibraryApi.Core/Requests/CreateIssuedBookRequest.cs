namespace LibraryApi.Core.Requests;

public sealed class CreateIssuedBookRequest(
    decimal Dues)
{
    public decimal Dues { get; } = Dues;
}
