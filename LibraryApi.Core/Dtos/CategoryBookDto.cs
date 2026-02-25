using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Dtos
{
    public sealed class CategoryBookDto(int Id, string Name, IReadOnlyList<BookDto> Books)
    {
        public int Id { get; } = Id;
        public string Name { get; } = Name;
        public IReadOnlyList<BookDto> Books { get; } = Books ?? throw new ArgumentNullException(nameof(Books));
    }
}
