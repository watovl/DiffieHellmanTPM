using Microsoft.EntityFrameworkCore;
using WebServer.Services.Hashing;

namespace WebServer.Models {
    /// <summary>
    /// Класс для работы с БД в качестве контекста
    /// </summary>
    public class DataBaseContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options) 
        {
            //Database.EnsureDeleted();   // удаляем бд со старой схемой
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=WebserverDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Установка значение свойства Name как уникальное
            modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();

            //// настройка связи один ко многим
            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Role)
            //    .WithMany(r => r.Users);

            // Добавление ролей
            Role adminRole = new Role { Id = 1, Name = "admin" };
            Role userRole = new Role { Id = 2, Name = "user" };
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });

            // Добавление админа
            byte[] sault = Salt.Create();
            User adminUser = new User {
                Id = 1,
                Name = "admin",
                Sault = sault,
                HashPassword = Hash.Create("admin", sault),
                RoleId = adminRole.Id
            };
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });

            base.OnModelCreating(modelBuilder);
        }
    }
}
