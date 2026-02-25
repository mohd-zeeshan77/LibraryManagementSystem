using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Dtos
{
    public sealed class BookDto(
        int Id,
        string Name,
        string AutherName,
        string Publisher,
        string Edition,
        decimal Price,
        int CategoryId
        )
    {
        public int Id { get; } = Id;
        public string Name { get; } = Name;
        public string AutherName { get; } = AutherName;
        public string Publisher { get; } = Publisher;
        public string Edition { get; }= Edition;
        public decimal Price { get; }= Price;
        public int CategoryId { get; }= CategoryId;
    }
}
