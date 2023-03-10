using API.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class TodoAppDbContext : DbContext
    {
        public DbSet<TodoApp> TodosApps { get; set; }

        public TodoAppDbContext(DbContextOptions<TodoAppDbContext> options)
                                 : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TodoApp>()
                   .Property(x => x.Descricao)
                   .HasMaxLength(120)
                   .IsRequired();

            builder.Entity<TodoApp>()
                .Property(x => x.Data)
                .IsRequired();

            builder.Entity<TodoApp>()
                .Property(x => x.Status)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
