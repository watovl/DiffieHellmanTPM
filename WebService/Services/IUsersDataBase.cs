using System.Collections.Generic;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Services {
    /// <summary>
    /// Интерфейс для работы с БД
    /// </summary>
    public interface IUsersDataBase {
        /// <summary>
        /// Возвращает список всех пользователей <see cref="UserDb"/>.
        /// </summary>
        Task<IEnumerable<UserDb>> GetAsync();

        /// <summary>
        /// Возвращает пользователя <see cref="UserDb"/> по заданному <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Id пользователя, которого надо вернуть.</param>
        /// <returns>Возвращает пользователя <see cref="UserDb"/></returns>
        Task<UserDb> GetAsync(int id);

        /// <summary>
        /// Возвращает пользователя <see cref="UserDb"/> по заданному имени <paramref name="userName"/>.
        /// </summary>
        /// <param name="userName">Имя пользователя, которого надо вернуть.</param>
        /// <returns>Возвращает пользователя <see cref="UserDb"/></returns>
        Task<UserDb> GetAsync(string userName);

        /// <summary>
        /// Записывает пользователя <see cref="UserDb"/> в БД.
        /// </summary>
        /// <param name="userDb">Пользователь <see cref="UserDb"/>, который записывается в БД.</param>
        Task CreateAsync(UserDb userDb);

        /// <summary>
        /// Обновляет (изменяет) пользователя <see cref="UserDb"/> в БД.
        /// </summary>
        /// <param name="updateUserDb">Изменёный пользователь <see cref="UserDb"/>.</param>
        Task UpdateAsync(UserDb updateUserDb);

        /// <summary>
        /// Удаляет пользователя <see cref="UserDb"/> из БД.
        /// </summary>
        /// <param name="userDb">Пользователь <see cref="UserDb"/>, который будет удалён.</param>
        Task DeleteAsync(UserDb userDb);
    }
}
