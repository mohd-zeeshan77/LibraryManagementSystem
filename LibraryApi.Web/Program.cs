using LibraryApi.Persistence;
using LibraryApi.Services;
using LibraryApi.Web.Endpoints;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbContext"));
});
builder.Services
    .AddScoped<BookService>()
    .AddScoped<CategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
RouteGroupBuilder apiGroup = app.MapGroup("api");
apiGroup.MapBookEndpoints()
    .MapCategoryEndpoints();

app.MapGet("/", () => $"Running in {app.Environment.EnvironmentName} right now.");

app.Run();