using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Dtos;

public sealed class MemberTypeDto(int Id, string Name,int TypeId, IReadOnlyList<UserDto> Users )
{
    public int Id { get; } = Id;
    public string Name { get; } = Name;
    public int TypeId { get; } = TypeId;
    public IReadOnlyList<UserDto> Users { get; } = Users;

}
