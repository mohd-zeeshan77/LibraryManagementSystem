using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
        _context = context?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }
    public IEnumerable<BookIssedDto> GetIssuedBook(string? name = null,string? bookname = null, bool? isReturned = null)
    {
        IQueryable<IssuedBook> query = _context.IssuedBook.AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(u=>u.User.Name.Contains(name));
        }
        if (!string.IsNullOrEmpty(bookname))
        {
            query = query.Where(u=>u.Book.Name.Contains(bookname));
        }
        if(isReturned != null)
        {
            query = query.Where(u=>u.IsReturned == isReturned.Value);
        }
        IList<BookIssedDto> books =query
                                    .Include(i => i.Book)
                                    .Include(i => i.User)
                                    .ThenInclude(u=>u.MemberType)
                                    .Select(i => new BookIssedDto(
                                                                  i.Id,
                                                                  i.User.Name,
                                                                  i.User.MemberType.Name,
                                                                  i.Book.Name,
                                                                  i.Dues,
                                                                  i.IssuedDate,
                                                                  i.ReturnDate,
                                                                  i.RenewStatus,
                                                                  i.RenewDate,
                                                                  i.IsReturned))
                                    .ToArray();
       return new ReadOnlyCollection<BookIssedDto>(books);
    }
    public BookIssedDto? AddBook(int bookid,int userid,CreateIssuedBookRequest request)
    {
        Book? book = _context.Book.FirstOrDefault(b=>b.Id ==  bookid);
        User? user = _context.User.Include(u => u.MemberType).FirstOrDefault(u=>u.Id == userid);
        if(book == null || user == null)
        {
            return null;
        }
        if (book.Stock <= 0) { return null; }
        int issuedCount = _context.IssuedBook
        .Count(i => i.UserId == userid && !i.IsReturned);
        if (issuedCount >= user.MemberType.MaxBookAllowed) { return null; }

        IssuedBook? issuedBook = _context.IssuedBook.FirstOrDefault(b=>b.BookId == bookid && b.UserId == userid && !b.IsReturned);
        if(issuedBook is not null)
        {
            return null;
        }
        book.Stock -= 1;
        issuedBook = new IssuedBook
        {
            BookId = bookid,
            UserId = userid,
            Dues = request.Dues,
            IssuedDate = request.IssuedDate,
            ReturnDate = request.ReturnedDate,
            RenewStatus = request.RenewStatus,
            RenewDate = request.RenewStatus ? request.RenewDate : null,
            IsReturned = false
        };
        _context.Add(issuedBook);
        _context.SaveChanges();
        return new BookIssedDto(
            issuedBook.Id,
            user.Name,
            user.MemberType.Name,
            book.Name,
            issuedBook.Dues,
            issuedBook.IssuedDate,
            issuedBook.ReturnDate,
            issuedBook.RenewStatus,
            issuedBook.RenewDate,
            issuedBook.IsReturned
            );
    }
    public BookIssedDto? IsReturn(int bookid,int userid,IsReturnRequest request)
    {
        Book? book = _context.Book.FirstOrDefault(b => b.Id == bookid);
        User? user = _context.User.Include(u => u.MemberType).FirstOrDefault(u => u.Id == userid);
        if(book == null || user == null)
        {
            return null;
        }
        IssuedBook? issuedBook = _context.IssuedBook.FirstOrDefault(b => b.BookId == bookid && b.UserId == userid);
        if (issuedBook is null)
        {
            return null;
        }
        if (request.IsReturn != issuedBook.IsReturned)
        {
            if (request.IsReturn)
            {
                issuedBook.IsReturned = true;
                book.Stock += 1;
            }
            else
            {
                if (book.Stock <= 0)
                {
                    return null;
                }
                issuedBook.IsReturned = false;
                book.Stock -= 1;
            }
        }
        _context.SaveChanges();
        return new BookIssedDto(
            issuedBook.Id,
            user.Name,
            user.MemberType.Name,
            book.Name,
            issuedBook.Dues,
            issuedBook.IssuedDate,
            issuedBook.ReturnDate,
            issuedBook.RenewStatus,
            issuedBook.RenewDate,
            issuedBook.IsReturned
            );
    }
    public BookIssedDto? UpdateRenew(int bookid,int userid ,UpdateRenewRequest request)
    {
        Book? book = _context.Book.FirstOrDefault(b => b.Id == bookid);
        User? user = _context.User.Include(u=>u.MemberType).FirstOrDefault(u => u.Id == userid);
        if(book is null || user is null) { return null; }
        IssuedBook? issuedBook = _context.IssuedBook.FirstOrDefault(b => b.BookId == bookid && b.UserId == userid);
        if (issuedBook is null)
        {
            return null;
        }
        if (request.RenewStatus)
        {
            issuedBook.RenewStatus = true;
            issuedBook.RenewDate = request.RenewDate;
        }
        else
        {
            issuedBook.RenewStatus = false;
            issuedBook.RenewDate = null;
        }
        _context.SaveChanges();
        return new BookIssedDto(
            issuedBook.Id,
            user.Name,
            user.MemberType.Name,
            book.Name,
            issuedBook.Dues,
            issuedBook.IssuedDate,
            issuedBook.ReturnDate,
            issuedBook.RenewStatus,
            issuedBook.RenewDate,
            issuedBook.IsReturned
            );
    }
}
