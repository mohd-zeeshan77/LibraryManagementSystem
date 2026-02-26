using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
using LibraryApi.Core.Dtos;
using LibraryApi.Core.Requests;
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
                                    .Include(u=>u.MemberType)
                                    .Select(u=>new UserDto(u.Id,u.Name,u.MemberType.Name))
                                    .ToArray();
        return new ReadOnlyCollection<UserDto>(users);
    }
    public UserDto? GetUserById(int Id)
    {
        User? user = _dbContext.User
            .Include(u=>u.MemberType)
            .FirstOrDefault(u => u.Id == Id);
        if(user == null)
        {
            return null;
        }
        return new UserDto(user.Id, user.Name, user.MemberType.Name);
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
                                            .Select(u => new UserDto(u.Id, u.Name, u.MemberType.Name))
                                            .ToList()
                                            .ToImmutableList();

        return new MemberTypeDto(member.Id, member.Name, users);
    }
    public UserDto? AddUser(int TypeId , CreateUserRequest request)
    {
        MemberType? member  =  _dbContext.MemberType.FirstOrDefault(s=>s.Id == TypeId);
        if(member == null)
        {
            return null;
        }
        User? user = _dbContext.User.FirstOrDefault(u => u.Name == request.Name);
        if(user is not null)
        {
            return null;
        }
        user = new User{ Name =  request.Name ,
                            TypeId = TypeId};
        _dbContext.Add(user);
        _dbContext.SaveChanges();
        return new UserDto(user.Id,user.Name,user.MemberType.Name);
    }
}
