namespace LibraryApi.Core.Requests;

public sealed class IsReturnRequest(bool IsReturn)
{
    public bool IsReturn { get; } = IsReturn;
}
