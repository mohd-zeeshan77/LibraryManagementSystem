using LibraryApi.Core.Dtos;
using LibraryApi.Core.Requests;
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
                                                                    b.AuthorName,
                                                                    b.Publisher,
                                                                    b.Edition,
                                                                    b.Price,
                                                                    b.Category.Name,
                                                                    b.Stock))
                .ToList()
                .ToImmutableList();
            return new CategoryBookDto(category.Id, category.Name, books);
        }
        public CategoryDto? AddCategory(CreateCategoryRequest request)
        {
            Category? category  =  _dbContext.Category.FirstOrDefault(c=>c.Name == request.Name);
            if(category is not null)
            {
                return null;
            }
            category=new Category { Name =  request.Name };
            _dbContext.Add(category);
            _dbContext.SaveChanges();
            return new CategoryDto(category.Id, category.Name);
        }
    }
}
