using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Requests;

public sealed class CreateUserRequest(string Name)
{
    public string Name { get; } = Name;
}
