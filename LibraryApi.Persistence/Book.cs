using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Persistence
{
    public sealed class Book
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string AutherName { get; set; }
        public required string Publisher {  get; set; }
        public required string Edition { get; set; }
        public required decimal Price { get; set; }
        public required int CategoryId { get; set; }
        public required Category Category { get; set; }
        public IList<IssuedBook> IssuedBooks { get; set; } = [];
    }
}
