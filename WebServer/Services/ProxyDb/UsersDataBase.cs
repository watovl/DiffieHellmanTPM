using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Models;

namespace WebServer.Services.ProxyDb {
    /// <summary>
    /// Реализация интерфейса <see cref="IUsersDataBase"/> для работы с БД.
    /// </summary>
    public class UsersDataBase : IUsersDataBase {
        private DataBaseContext Context;

        public UsersDataBase(DataBaseContext context) {
            Context = context;
        }

        /// <summary>
        /// Возвращает список всех пользователей <see cref="User"/>.
        /// </summary>
        public Task<IEnumerable<User>> GetAllAsync() {
            return Task.FromResult(Context.Users.Include(u => u.Role).AsEnumerable());
        }

        /// <summary>
        /// Возвращает пользователя <see cref="User"/> по заданному <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Id пользователя, которого надо вернуть.</param>
        /// <returns>Возвращает пользователя <see cref="User"/></returns>
        public async Task<User> GetAsync(int id) {
            return await Context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id); //FindAsync(id);
        }

        /// <summary>
        /// Возвращает пользователя <see cref="User"/> по заданному имени <paramref name="userName"/>.
        /// </summary>
        /// <param name="userName">Имя пользователя, которого надо вернуть.</param>
        /// <returns>Возвращает пользователя <see cref="User"/></returns>
        public async Task<User> GetAsync(string userName) {
            return await Context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Name == userName);
        }

        /// <summary>
        /// Записывает пользователя <see cref="User"/> в БД.
        /// </summary>
        /// <param name="userDb">Пользователь <see cref="User"/>, который записывается в БД.</param>
        public async Task CreateAsync(User userDb) {
            await Context.Users.AddAsync(userDb);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновляет (изменяет) пользователя <see cref="User"/> в БД.
        /// </summary>
        /// <param name="updateUserDb">Изменёный пользователь <see cref="User"/>.</param>
        public async Task UpdateAsync(User updateUserDb) {
            Context.Users.Update(updateUserDb);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет пользователя <see cref="User"/> из БД.
        /// </summary>
        /// <param name="userDb">Пользователь <see cref="User"/>, который будет удалён.</param>
        public async Task DeleteAsync(User userDb) {
            Context.Remove(userDb);
            await Context.SaveChangesAsync();
        }
    }
}
