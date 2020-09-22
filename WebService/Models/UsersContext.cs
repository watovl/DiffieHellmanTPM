using Microsoft.EntityFrameworkCore;

namespace WebService.Models {
    /// <summary>
    /// Класс для работы с БД в качестве контекста
    /// </summary>
    public class UsersContext : DbContext {
        public DbSet<UserDb> Users { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options) {
            //Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=webserverusersdb;Trusted_Connection=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Установка значение свойства Name как уникальное
            modelBuilder.Entity<UserDb>().HasIndex(u => u.Name).IsUnique();
        }
    }
}
