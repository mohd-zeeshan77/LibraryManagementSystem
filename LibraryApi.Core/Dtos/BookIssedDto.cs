namespace LibraryApi.Core.Dtos;

public sealed class BookIssedDto(
    int Id,
    int UserId,
    string UserName,
    string UserType,
    int BookId,
    string BookName,
    decimal Dues,
    DateOnly IssuedDate,
    DateOnly ReturnDate,
    bool RenewStatus,
    DateOnly? RenewDate,
    bool IsReturned)
{
    public int Id { get; } = Id;
    public int UserId { get; } = UserId;
    public string UserName { get; } = UserName;
    public string UserType { get; } = UserType;
    public int BookId { get; } = BookId;
    public string BookName { get; } = BookName;
    public decimal Dues { get; } = Dues;
    public DateOnly IssuedDate { get; } = IssuedDate;
    public DateOnly ReturnDate { get; } = ReturnDate;
    public bool RenewStatus { get; } = RenewStatus;
    public DateOnly? RenewDate { get; } = RenewDate;
    public bool IsReturned { get; } = IsReturned;
}
