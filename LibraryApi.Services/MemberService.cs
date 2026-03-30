using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using LibraryApi.Core.Dtos;
using LibraryApi.Persistence;
using Microsoft.Extensions.Logging;

namespace LibraryApi.Services;

public sealed class MemberService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<MemberService> _logger;
    public MemberService(AppDbContext dbContext, ILogger<MemberService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public IEnumerable<MemberDto> GetMemberList()
    {
        IList<MemberDto> member = _dbContext.MemberType.Select(m=> new MemberDto(m.Id, m.Name,m.MaxBookAllowed)).ToArray();
        return new ReadOnlyCollection<MemberDto>(member);
    }
}
