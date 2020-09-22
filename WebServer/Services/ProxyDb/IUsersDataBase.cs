using System.Collections.Generic;
using System.Threading.Tasks;
using WebServer.Models;

namespace WebServer.Services.ProxyDb {
    /// <summary>
    /// Интерфейс для работы с БД
    /// </summary>
    public interface IUsersDataBase {
        /// <summary>
        /// Возвращает список всех пользователей <see cref="User"/>.
        /// </summary>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Возвращает пользователя <see cref="User"/> по заданному <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Id пользователя, которого надо вернуть.</param>
        /// <returns>Возвращает пользователя <see cref="User"/></returns>
        Task<User> GetAsync(int id);

        /// <summary>
        /// Возвращает пользователя <see cref="User"/> по заданному имени <paramref name="userName"/>.
        /// </summary>
        /// <param name="userName">Имя пользователя, которого надо вернуть.</param>
        /// <returns>Возвращает пользователя <see cref="User"/></returns>
        Task<User> GetAsync(string userName);

        /// <summary>
        /// Записывает пользователя <see cref="User"/> в БД.
        /// </summary>
        /// <param name="userDb">Пользователь <see cref="User"/>, который записывается в БД.</param>
        Task CreateAsync(User userDb);

        /// <summary>
        /// Обновляет (изменяет) пользователя <see cref="User"/> в БД.
        /// </summary>
        /// <param name="updateUserDb">Изменёный пользователь <see cref="User"/>.</param>
        Task UpdateAsync(User updateUserDb);

        /// <summary>
        /// Удаляет пользователя <see cref="User"/> из БД.
        /// </summary>
        /// <param name="userDb">Пользователь <see cref="User"/>, который будет удалён.</param>
        Task DeleteAsync(User userDb);
    }
}
