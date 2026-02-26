using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Dtos;

public sealed class CategoryDto(int Id, string Name)
{
    public int Id { get; } = Id;
    public string Name { get; } = Name;
}
