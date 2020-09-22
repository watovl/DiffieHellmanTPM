using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebServer.Services.Hashing;

namespace WebServer.Models {
    /// <summary>
    /// Класс пользователя для БД
    /// </summary>
    public class User {
        public int Id { get; set; } // Id пользователя

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } // Имя пользователя (не более 50 символов)

        [Required]
        public byte[] Sault { get; set; } // Случайная последовательность использующаяся в хэше

        [Required]
        public string HashPassword { get; set; } // Хэш пароля пользователя

        public int RoleId { get; set; }

        public Role Role { get; set; }


        public User() { }

        public User(string name, string password, Role role) {
            Name = name;
            Sault = Salt.Create();
            HashPassword = Hash.Create(password, Sault);
            Role = role;
        }

        public bool Equals(User userDb) {
            return userDb == null
                ? false
                : Id == userDb.Id &&
                Name == userDb.Name &&
                HashPassword == userDb.HashPassword &&
                Sault == userDb.Sault;
        }
    }
}
