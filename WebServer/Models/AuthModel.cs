using System.ComponentModel.DataAnnotations;

namespace WebServer.Models {
    /// <summary>
    /// Класс входного пользователя.
    /// </summary>
    public class AuthModel {
        [Required(ErrorMessage = "Не указано имя")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Пароль должен быть от 4 символов")]
        public string Password { get; set; }
    }
}
