using System.ComponentModel.DataAnnotations;
using WebService.Services.Hashing;

namespace WebService.Models {
    /// <summary>
    /// Класс пользователя для БД
    /// </summary>
    public class UserDb {
        public int Id { get; set; } // Id пользователя

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } // Имя пользователя (не более 50 символов)

        public byte[] Salt { get; private set; } // Случайная последовательность использующаяся в хэше

        [Required]
        public string HashPassword {    // Хэш пароля пользователя
            get => HashPassword;
            set {
                Salt = Services.Hashing.Salt.Create();
                HashPassword = Hash.Create(value, Salt);
            }
        }


        public UserDb() { }

        public UserDb(string name, string password) {
            Name = name;
            HashPassword = password;
        }

        public bool Equals(UserDb userDb) {
            if (userDb == null) {
                return false;
            }
            return Id == userDb.Id &&
                Name == userDb.Name &&
                HashPassword == userDb.HashPassword &&
                Salt == userDb.Salt;
        }
    }
}
