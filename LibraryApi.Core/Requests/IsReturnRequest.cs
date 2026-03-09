using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Requests;

public sealed class IsReturnRequest(bool IsReturn)
{
    public bool IsReturn { get; } = IsReturn;
}
