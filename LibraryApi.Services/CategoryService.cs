using LibraryApi.Core.Dtos;
using LibraryApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;

namespace LibraryApi.Services
{
    public sealed class CategoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(AppDbContext dbContext, ILogger<CategoryService> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        public IEnumerable<CategoryDto> GetCategories()
        {
            IList<CategoryDto> Categories = _dbContext.Category
                .Select(c=> new CategoryDto(c.Id,c.Name))
                .ToArray();
            return new ReadOnlyCollection<CategoryDto>(Categories);
        }
        public CategoryBookDto? GetCategoryBook(int Id)
        {
            Category? category = _dbContext.Category
                .Include(c=>c.Books)
                .FirstOrDefault(b=>b.Id == Id);
            if (category == null)
            {
                return null;
            }
            IImmutableList<BookDto> books = category.Books.Select(b=> new BookDto(
                                                                    b.Id,
                                                                    b.Name,
                                                                    b.AutherName,
                                                                    b.Publisher,
                                                                    b.Edition,
                                                                    b.Price,
                                                                    b.CategoryId))
                .ToList()
                .ToImmutableList();
            return new CategoryBookDto(category.Id, category.Name, books);
        }
    }
}
