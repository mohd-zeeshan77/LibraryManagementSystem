using System.Collections.ObjectModel;
using LibraryApi.Core.Dtos;
using LibraryApi.Core.Requests;
using LibraryApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryApi.Services;

public sealed class IssuedBookservice
{
    private readonly AppDbContext _context;
    private readonly ILogger<IssuedBookservice> _logger;

    public IssuedBookservice(AppDbContext context, ILogger<IssuedBookservice> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    public IEnumerable<BookIssedDto> GetIssuedBooks(
                                                    string? name = null,
                                                    string? bookname = null,
                                                    bool? isReturned = null,
                                                    DateOnly? issueDate = null,
                                                    DateOnly? returnDate = null,
                                                    DateOnly? renewDate = null,
                                                    DateOnly? startDate = null,
                                                    DateOnly? endDate = null,
                                                    int? issueYear = null)
    {
        IQueryable<IssuedBook> query = _context.IssuedBook.AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(u => u.User.Name.Contains(name));
        }

        if (!string.IsNullOrEmpty(bookname))
        {
            query = query.Where(u => u.Book.Name.Contains(bookname));
        }

        if (isReturned != null)
        {
            query = query.Where(u => u.IsReturned == isReturned.Value);
        }
        if(issueDate != null)
        {
            query = query.Where(u => u.IssuedDate == issueDate.Value);
        }
        if (returnDate.HasValue)
        {
            query = query.Where(u => u.ReturnDate == returnDate.Value);
        }
        if (renewDate.HasValue)
        {
            query = query.Where(u => u.RenewDate == renewDate.Value);
        }
        if (startDate.HasValue && endDate.HasValue)
        {
            query = query.Where(u => u.IssuedDate >= startDate.Value && u.IssuedDate <= endDate.Value);
        }
        else if (startDate.HasValue)
        {
            query = query.Where(u => u.ReturnDate >= startDate.Value);
        }
        else if (endDate.HasValue)
        {
            query = query.Where(u => u.ReturnDate <= endDate.Value);
        }
        if (issueYear.HasValue)
        {
            query = query.Where(u=>u.IssuedDate.Year == issueYear.Value);
        }
        IList<BookIssedDto> books = query
            .Include(i => i.Book)
            .Include(i => i.User)
            .ThenInclude(u => u.MemberType)
            .Select(i => new BookIssedDto(
                i.Id,
                i.UserId,
                i.User.Name,
                i.User.MemberType.Name,
                i.BookId,
                i.Book.Name,
                i.IssuedDate,
                i.ReturnDate,
                i.RenewStatus,
                i.RenewDate,
                i.IsReturned))
            .ToArray();
        return new ReadOnlyCollection<BookIssedDto>(books);
    }

    public BookIssedDto? AddIssuedBook(int bookid, int userid)
    {
        Book? book = _context.Book.FirstOrDefault(b => b.Id == bookid);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book not found {bookid}");
        }

        User? user = _context.User.Include(u => u.MemberType).FirstOrDefault(u => u.Id == userid);
        if (user == null)
        {
            throw new KeyNotFoundException($"User not found with UserId: {userid}");
        }

        if (book.Stock <= 0)
        {
            throw new FileNotFoundException($"Book you are trying to issue is out of stock. BookId: {bookid}");
        }

        int issuedCount = _context.IssuedBook
            .Count(i => i.UserId == userid && !i.IsReturned);
        if (issuedCount >= user.MemberType.MaxBookAllowed)
        {
            throw new ArgumentOutOfRangeException($"User already have Issued maximum number of books. UserId: {userid}");
        }

        IssuedBook? issuedBook =
            _context.IssuedBook.FirstOrDefault(b => b.BookId == bookid && b.UserId == userid && !b.IsReturned);
        if (issuedBook is not null)
        {
            throw new DuplicateWaitObjectException($" This book {bookid} already issued by user {userid}");
        }

        
        issuedBook = new IssuedBook
        {
            BookId = bookid,
            UserId = userid,
            IssuedDate = DateOnly.FromDateTime(DateTime.Today),
            ReturnDate = DateOnly.FromDateTime(DateTime.Today).AddDays(7),
            RenewStatus = false,
            RenewDate = null,
            IsReturned = false
        };
        _context.Add(issuedBook);
        _context.SaveChanges();
        book.Stock -= 1;
        return new BookIssedDto(
            issuedBook.Id,
            issuedBook.UserId,
            user.Name,
            user.MemberType.Name,
            issuedBook.BookId,
            book.Name,
            issuedBook.IssuedDate,
            issuedBook.ReturnDate,
            issuedBook.RenewStatus,
            issuedBook.RenewDate,
            issuedBook.IsReturned
        );
    }

    public BookIssedDto? IsReturn(int bookid, int userid, BoolPatchRequest request)
    {
        Book? book = _context.Book.FirstOrDefault(b => b.Id == bookid);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book not found {bookid}");
        }

        User? user = _context.User.Include(u => u.MemberType).FirstOrDefault(u => u.Id == userid);
        if (user == null)
        {
            throw new KeyNotFoundException($"User not found with UserId: {userid}");
        }

        IssuedBook? issuedBook = _context.IssuedBook.FirstOrDefault(b => b.BookId == bookid && b.UserId == userid);
        if (issuedBook is null)
        {
            throw new KeyNotFoundException($" This book {bookid} issued by user {userid} does not exist");
        }

        if (request.BoolRequest != issuedBook.IsReturned)
        {
            if (request.BoolRequest)
            {
                issuedBook.IsReturned = true;
                book.Stock += 1;
            }
            else
            {
                if (book.Stock <= 0)
                {
                    throw new FileNotFoundException($"Book you are trying to issue is out of stock. BookId: {bookid}");
                }

                issuedBook.IsReturned = false;
                book.Stock -= 1;
            }
        }

        _context.SaveChanges();
        return new BookIssedDto(
            issuedBook.Id,
            issuedBook.UserId,
            user.Name,
            user.MemberType.Name,
            issuedBook.BookId,
            book.Name,
            issuedBook.IssuedDate,
            issuedBook.ReturnDate,
            issuedBook.RenewStatus,
            issuedBook.RenewDate,
            issuedBook.IsReturned
        );
    }

    public BookIssedDto? UpdateRenew(int bookid, int userid, BoolPatchRequest request)
    {
        Book? book = _context.Book.FirstOrDefault(b => b.Id == bookid);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book not found {bookid}");
        }

        User? user = _context.User.Include(u => u.MemberType).FirstOrDefault(u => u.Id == userid);
        if (user is null)
        {
            throw new KeyNotFoundException($"User not found with UserId: {userid}");
        }

        IssuedBook? issuedBook = _context.IssuedBook.FirstOrDefault(b => b.BookId == bookid && b.UserId == userid);
        if (issuedBook is null)
        {
            throw new KeyNotFoundException($" This book {bookid} issued by user {userid} does not exist");
        }

        if (request.BoolRequest)
        {
            issuedBook.RenewStatus = true;
            issuedBook.RenewDate = DateOnly.FromDateTime(DateTime.Today);
        }
        else
        {
            issuedBook.RenewStatus = false;
            issuedBook.RenewDate = null;
        }

        _context.SaveChanges();
        return new BookIssedDto(
            issuedBook.Id,
            issuedBook.UserId,
            user.Name,
            user.MemberType.Name,
            issuedBook.BookId,
            book.Name,
            issuedBook.IssuedDate,
            issuedBook.ReturnDate,
            issuedBook.RenewStatus,
            issuedBook.RenewDate,
            issuedBook.IsReturned
        );
    }
    public BookIssedDto? DeleteIssuedBook(int id)
    {
        IssuedBook? issue = _context.IssuedBook
    .Include(i => i.User)
        .ThenInclude(u => u.MemberType)
    .Include(i => i.Book)
    .FirstOrDefault(i => i.Id == id);
        if (issue is null)
        {
            throw new FileNotFoundException($"Issued ID {id} not found");
        }
        _context.Remove(issue);
        _context.SaveChanges();
        return new BookIssedDto(
                        issue.Id,
                        issue.UserId,
                        issue.User.Name,
                        issue.User.MemberType.Name,
                        issue.BookId,
                        issue.Book.Name,
                        issue.IssuedDate,
                        issue.ReturnDate,
                        issue.RenewStatus,
                        issue.RenewDate,
                        issue.IsReturned
            );
    }
}
