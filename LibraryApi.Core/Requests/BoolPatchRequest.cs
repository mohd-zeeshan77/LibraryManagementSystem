namespace LibraryApi.Core.Requests;

public sealed class BoolPatchRequest(bool BoolRequest)
{
    public bool BoolRequest { get; } = BoolRequest;
}
