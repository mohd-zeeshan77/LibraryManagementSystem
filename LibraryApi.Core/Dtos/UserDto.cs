using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Dtos
{
    public sealed class UserDto(int Id, string Name, bool Type )
    {
        public int Id { get; } = Id;
        public string Name { get; } = Name;
        public bool Type { get; } = Type;
    }
}
