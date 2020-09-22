using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Services {
    /// <summary>
    /// Реализация интерфейса <see cref="IUsersDataBase"/> для работы с БД.
    /// </summary>
    public class UsersDataBase : IUsersDataBase {
        private UsersContext Context;

        public UsersDataBase(UsersContext context) {
            Context = context;
        }

        /// <summary>
        /// Возвращает список всех пользователей <see cref="UserDb"/>.
        /// </summary>
        public Task<IEnumerable<UserDb>> GetAsync() {
            return Task.FromResult(Context.Users.AsEnumerable());
        }

        /// <summary>
        /// Возвращает пользователя <see cref="UserDb"/> по заданному <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Id пользователя, которого надо вернуть.</param>
        /// <returns>Возвращает пользователя <see cref="UserDb"/></returns>
        public async Task<UserDb> GetAsync(int id) {
            return await Context.Users.FindAsync(id);
        }

        /// <summary>
        /// Возвращает пользователя <see cref="UserDb"/> по заданному имени <paramref name="userName"/>.
        /// </summary>
        /// <param name="userName">Имя пользователя, которого надо вернуть.</param>
        /// <returns>Возвращает пользователя <see cref="UserDb"/></returns>
        public Task<UserDb> GetAsync(string userName) {
            return Task.FromResult(Context.Users.FirstOrDefault(user => user.Name == userName));
        }

        /// <summary>
        /// Записывает пользователя <see cref="UserDb"/> в БД.
        /// </summary>
        /// <param name="userDb">Пользователь <see cref="UserDb"/>, который записывается в БД.</param>
        public async Task CreateAsync(UserDb userDb) {
            await Context.Users.AddAsync(userDb);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновляет (изменяет) пользователя <see cref="UserDb"/> в БД.
        /// </summary>
        /// <param name="updateUserDb">Изменёный пользователь <see cref="UserDb"/>.</param>
        public async Task UpdateAsync(UserDb updateUserDb) {
            Context.Users.Update(updateUserDb);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет пользователя <see cref="UserDb"/> из БД.
        /// </summary>
        /// <param name="userDb">Пользователь <see cref="UserDb"/>, который будет удалён.</param>
        public async Task DeleteAsync(UserDb userDb) {
            Context.Remove(userDb);
            await Context.SaveChangesAsync();
        }
    }
}
