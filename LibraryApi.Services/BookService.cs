using LibraryApi.Core.Dtos;
using LibraryApi.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LibraryApi.Services
{
    public sealed class BookService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<BookService> _logger;
        public BookService(AppDbContext dbContext, ILogger<BookService> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        public IEnumerable<BookDto> GetBooks()
        {
            IList<BookDto> books =_dbContext.Book
                            .Select(b=> new BookDto(
                                b.Id,
                                b.Name,
                                b.AutherName,
                                b.Publisher,
                                b.Edition,
                                b.Price,
                                b.CategoryId))
                            .ToArray();
            return new ReadOnlyCollection<BookDto>(books);
        }
        public BookDto? GetBook(int Id)
        {
            Book? book  = _dbContext.Book.FirstOrDefault(b => b.Id == Id);
            if(book == null)
            {
                return null;
            }
            return new BookDto(book.Id,
                book.Name,
                book.AutherName,
                book.Publisher,
                book.Edition,
                book.Price,
                book.CategoryId);
        }
    }
}
