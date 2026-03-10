namespace LibraryApi.Core.Requests;

public sealed class UpdateRenewRequest(bool RenewStatus, DateOnly? RenewDate)
{
    public bool RenewStatus { get; } = RenewStatus;
    public DateOnly? RenewDate { get; } = RenewDate;
}
