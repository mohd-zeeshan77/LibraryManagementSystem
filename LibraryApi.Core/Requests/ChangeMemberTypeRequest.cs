namespace LibraryApi.Core.Requests;

public sealed class ChangeMemberTypeRequest(int MemberId)
{
    public int MemberId { get; } = MemberId;
}
