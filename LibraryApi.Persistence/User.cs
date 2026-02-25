using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Persistence
{
    public sealed class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required bool Type { get; set; }
        public IList<IssuedBook> IssuedBooks { get; set; } = [];
    }
}
