using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using WebServer.Models;
using WebServer.Services.ProxyDb;
using WebServer.Services.Hashing;
using WebServer.Util;

namespace WebServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private IUsersDataBase ContextUserDb;
        private IRolesDataBase ContextRoleDb; 
        private readonly ILogger _logger;
        public UsersController(IUsersDataBase contextUser, IRolesDataBase contextRole, ILogger<UsersController> logger) {
            ContextUserDb = contextUser;
            ContextRoleDb = contextRole;
            _logger = logger;
        }

        /// <summary>
        /// Возвращает всех пользователей из БД.
        /// </summary>
        [Authorize(Roles = "admin")]
        [HttpGet(Name = "GetAllUsers")]
        public async Task<IEnumerable<User>> GetAllUsers() {
            return await ContextUserDb.GetAllAsync();
        }

        /// <summary>
        /// Возвращает пользователя по заданному <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Id пользователя, который будет возвращён.</param>
        /// <returns>Возвращает пользователя</returns>
        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<IActionResult> GetUser(int id) {
            User userDb = await ContextUserDb.GetAsync(id);
            if (userDb == null) {
                return NotFound("Пользователь не найден");
            }
            return new UserResult(userDb);
        }

        /// <summary>
        /// Возвращает пользователя по заданному имени <paramref name="userName"/>.
        /// </summary>
        /// <param name="userName">Имя пользователя, который будет возвращён.</param>
        /// <returns>Возвращает пользователя</returns>
        [HttpGet("name/{userName:maxlength(50)}", Name = "GetUserByName")]
        public async Task<IActionResult> GetUser(string userName) {
            if (string.IsNullOrWhiteSpace(userName)) {
                return BadRequest();
            }
            User userDb = await ContextUserDb.GetAsync(userName);
            if (userDb == null) {
                return NotFound("Пользователь не найден");
            }
            return new UserResult(userDb);
        }

        /// <summary>
        /// Записывает нового пользователя в БД.
        /// </summary>
        /// <param name="user">Пользователь, которого надо записать в БД.</param>
        /// <returns>Возвращает созданного пользователя.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(AuthModel user) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (await ContextUserDb.GetAsync(user.Name) != null) {
                return Conflict("Пользователь с таким именем уже зарегистрирован");
            }
            Role userRole = await ContextRoleDb.GetUserAsync();
            User userDb = new User(user.Name, user.Password, userRole);
            await ContextUserDb.CreateAsync(userDb);
            return new UserResult(userDb);
        }

        /// <summary>
        /// Проверяет существование пользователя
        /// </summary>
        /// <param name="user">Пользователь на проверку существования</param>
        /// <returns>Возвращает true, если пользователь существует, иначе - false</returns>
        [HttpPost("existence")]
        public async Task<IActionResult> ExistenceUser(AuthModel user) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            User userDb = await ContextUserDb.GetAsync(user.Name);
            if (userDb != null && Hash.Validate(user.Password, userDb.Sault, userDb.HashPassword)) {
                return new ObjectResult(true);
            }
            else {
                return new ObjectResult(false);
            }
        }

        /// <summary>
        /// Обновляет (изменяет) пользователя в БД.
        /// </summary>
        /// <param name="id">Id пользователя, которого надо изменить.</param>
        /// <param name="updateUser">Изменённый пользователь.</param>
        /// <returns>Возвращает изменённого пользователя.</returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, AuthModel updateUser) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            User userDb = await ContextUserDb.GetAsync(id);
            if (userDb == null) {
                return NotFound("Пользователь не найден"); 
            }

            User newUser = await ContextUserDb.GetAsync(updateUser.Name);
            if (newUser != null && !newUser.Equals(userDb)) {
                return Conflict("Пользователь с таким именем уже зарегистрирован");
            }

            userDb.Name = updateUser.Name;
            userDb.HashPassword = updateUser.Password;

            await ContextUserDb.UpdateAsync(userDb);
            return new UserResult(userDb);
        }

        /// <summary>
        /// Обновляет (изменяет) пользователя в БД.
        /// </summary>
        /// <param name="userName">Имя пользователя, которого надо изменить.</param>
        /// <param name="updateUser">Изменённый пользователь.</param>
        /// <returns>Возвращает изменённого пользователя.</returns>
        [Authorize]
        [HttpPut("name")]
        public async Task<IActionResult> UpdateUser(string userName, AuthModel updateUser) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            User userDb = await ContextUserDb.GetAsync(userName);
            if (userDb == null) {
                return NotFound("Пользователь не найден");
            }

            User newUser = await ContextUserDb.GetAsync(updateUser.Name);
            if (newUser != null && !newUser.Equals(userDb)) {
                return Conflict("Пользователь с таким именем уже зарегистрирован");
            }

            userDb.Name = updateUser.Name;
            userDb.HashPassword = updateUser.Password;

            await ContextUserDb.UpdateAsync(userDb);
            return new UserResult(userDb);
        }

        /// <summary>
        /// Удаляет пользователя из БД.
        /// </summary>
        /// <param name="id">Id пользователя, которого надо удалить.</param>
        /// <returns>Возвращает удалённого пользователя.</returns>
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id) {
            User userDb = await ContextUserDb.GetAsync(id);
            if (userDb == null) {
                return NotFound("Пользователь не найден");
            }
            await ContextUserDb.DeleteAsync(userDb);
            return new UserResult(userDb);
        }

        /// <summary>
        /// Удаляет пользователя из БД.
        /// </summary>
        /// <param name="userName">Имя пользователя, которого надо удлаить.</param>
        /// <returns>Возвращает удалённого пользователя.</returns>
        [Authorize]
        [HttpDelete("name/{userName:maxlength(50)}")]
        public async Task<IActionResult> DeleteUser(string userName) {
            User userDb = await ContextUserDb.GetAsync(userName);
            if (userDb == null) {
                return NotFound("Пользователь не найден");
            }
            await ContextUserDb.DeleteAsync(userDb);
            return new UserResult(userDb);
        }
    }
}