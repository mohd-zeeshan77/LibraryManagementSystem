using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Requests;

public sealed class ChangeMemberTypeRequest(int MemberId)
{
    public int MemberId { get; } = MemberId;   
}
