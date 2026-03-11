using System.Collections.ObjectModel;
using LibraryApi.Core.Dtos;
using LibraryApi.Core.Requests;
using LibraryApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryApi.Services;

public sealed class BookService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<BookService> _logger;

    public BookService(AppDbContext dbContext, ILogger<BookService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;
    }

    public IEnumerable<BookDto> GetBooks(string? keyword = null)
    {
        IQueryable<Book> query = _dbContext.Book.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(b => b.Name.Contains(keyword));
        }

        IList<BookDto> books = query
            .Include(b => b.Category)
            .Select(b => new BookDto(
                b.Id,
                b.Name,
                b.AuthorName,
                b.Publisher,
                b.Edition,
                b.Price,
                b.Category.Name,
                b.Stock))
            .ToArray();
        return new ReadOnlyCollection<BookDto>(books);
    }

    public BookDto? GetBook(int Id)
    {
        Book? book = _dbContext.Book
            .Include(b => b.Category)
            .FirstOrDefault(b => b.Id == Id);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book not exist with BookId {Id}");
        }

        return new BookDto(book.Id,
            book.Name,
            book.AuthorName,
            book.Publisher,
            book.Edition,
            book.Price,
            book.Category.Name,
            book.Stock);
    }

    public BookDto? AddBook(int categoryId, CreateBookRequest request)
    {
        Category? category = _dbContext.Category.FirstOrDefault(c => c.Id == categoryId);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category does not exist with {categoryId}");
        }

        Book? book = _dbContext.Book.FirstOrDefault(b => b.Name == request.Name
                                                         && b.AuthorName == request.AuthorName
                                                         && b.Publisher == request.Publisher
                                                         && b.Edition == request.Edition);
        if (book is not null)
        {
           throw new DuplicateWaitObjectException($"This book already exists with {request.Name}");
        }

        book = new Book
        {
            Name = request.Name,
            AuthorName = request.AuthorName,
            Publisher = request.Publisher,
            Edition = request.Edition,
            Price = request.Price,
            CategoryId = categoryId,
            Stock = request.Stock
        };
        _dbContext.Add(book);
        _dbContext.SaveChanges();
        return new BookDto(book.Id,
            book.Name,
            book.AuthorName,
            book.Publisher,
            book.Edition,
            book.Price,
            book.Category.Name,
            book.Stock);
    }
}
