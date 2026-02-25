using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
using LibraryApi.Core.Dtos;
using LibraryApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryApi.Services;

public sealed class UserService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<UserService> _logger;
    public UserService(AppDbContext dbContext, ILogger<UserService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;
    }
    public IEnumerable<UserDto> GetUsers()
    {
        IList<UserDto> users = _dbContext.User
                                    .Select(u=>new UserDto(u.Id,u.Name,u.TypeId))
                                    .ToArray();
        return new ReadOnlyCollection<UserDto>(users);
    }
    public MemberTypeDto? GetMemberType(int Id)
    {
        MemberType? member = _dbContext.MemberType
                                .Include(m => m.Users)
                                .FirstOrDefault(m=>m.Id == Id);
        if(member == null)
        {
            return null;
        }
        IImmutableList<UserDto> users = member.Users
                                            .Select(u => new UserDto(u.Id, u.Name, u.TypeId))
                                            .ToList()
                                            .ToImmutableList();

        return new MemberTypeDto(member.Id, member.Name, users);
    }
}
