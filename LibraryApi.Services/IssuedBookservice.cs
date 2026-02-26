using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using LibraryApi.Core.Dtos;
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
    public IEnumerable<BookIssedDto> GetIssuedBook(string? name = null)
    {
        IQueryable<IssuedBook> query = _context.IssuedBook.AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(u=>u.User.Name.Contains(name));
        }
        IList<BookIssedDto> books =query
                                    .Include(i => i.Book)
                                    .Include(i => i.User)
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

}
