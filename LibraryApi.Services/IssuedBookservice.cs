using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LibraryApi.Core.Dtos;
using LibraryApi.Persistence;
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
    //public IEnumerable<BookIssuedByMember> GetIssuedBook()
    //{

    //}
}
