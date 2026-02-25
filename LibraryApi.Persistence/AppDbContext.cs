using Microsoft.EntityFrameworkCore;


namespace LibraryApi.Persistence
{
    public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> User { get; init; }
        public DbSet<Book> Book { get; init; }
        public DbSet<IssuedBook> IssuedBook { get; init; }
        public DbSet<Category> Category { get; init; }
        public DbSet<MemberType> MemberType { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Type t  = typeof(AppDbContext);
            modelBuilder.ApplyConfigurationsFromAssembly(t.Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
