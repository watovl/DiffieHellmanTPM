using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebService.Models;
using WebService.Services;
using WebService.Services.Hashing;

namespace WebService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private IUsersDataBase ContextDb;
        private readonly ILogger _logger;
        public UsersController(IUsersDataBase context, ILogger<UsersController> logger) {
            ContextDb = context;
            _logger = logger;
        }

        /// <summary>
        /// Возвращает всех пользователей из БД.
        /// </summary>
        [HttpGet(Name = "GetAllUsers")]
        public async Task<IEnumerable<UserDb>> GetAllUsers() {
            return await ContextDb.GetAsync();
        }

        /// <summary>
        /// Возвращает пользователя по заданному <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Id пользователя, который будет возвращён.</param>
        /// <returns>Возвращает пользователя</returns>
        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<IActionResult> GetUser(int id) {
            UserDb userDb = await ContextDb.GetAsync(id);
            if (userDb == null) {
                return NotFound();
            }
            return new ObjectResult(userDb);
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
            UserDb userDb = await ContextDb.GetAsync(userName);
            if (userDb == null) {
                return NotFound();
            }
            return new ObjectResult(userDb);
        }

        /// <summary>
        /// Записывает нового пользователя в БД.
        /// </summary>
        /// <param name="user">Пользователь, которого надо записать в БД.</param>
        /// <returns>Возвращает созданного пользователя.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserAuth user) {
            if (user == null) {
                return BadRequest();
            }
            if (await ContextDb.GetAsync(user.Name) != null) {
                return Conflict("Пользователь с таким именем уже зарегистрирован");
            }
            UserDb userDb = new UserDb(user.Name, user.Password);
            await ContextDb.CreateAsync(userDb);
            return new ObjectResult(userDb);
        }

        /// <summary>
        /// Проверяет существование пользователя
        /// </summary>
        /// <param name="user">Пользователь на проверку существования</param>
        /// <returns>Возвращает true, если пользователь существует, иначе - false</returns>
        [HttpPost("existence")]
        public async Task<IActionResult> ExistenceUser(UserAuth user) {
            if (user == null) {
                return BadRequest();
            }
            UserDb userDb = await ContextDb.GetAsync(user.Name);
            if (userDb != null && Hash.Validate(user.Password, userDb.Salt, userDb.HashPassword)) {
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
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, UserAuth updateUser) {
            if (updateUser == null) {
                return BadRequest();
            }

            UserDb userDb = await ContextDb.GetAsync(id);
            if (userDb == null) {
                return NotFound();
            }

            UserDb newUser = await ContextDb.GetAsync(updateUser.Name);
            if (newUser != null && !newUser.Equals(userDb)) {
                return Conflict("Пользователь с таким именем уже зарегистрирован");
            }

            userDb.Name = updateUser.Name;
            userDb.HashPassword = updateUser.Password;

            await ContextDb.UpdateAsync(userDb);
            return new ObjectResult(userDb);
        }

        /// <summary>
        /// Обновляет (изменяет) пользователя в БД.
        /// </summary>
        /// <param name="userName">Имя пользователя, которого надо изменить.</param>
        /// <param name="updateUser">Изменённый пользователь.</param>
        /// <returns>Возвращает изменённого пользователя.</returns>
        [HttpPut("name")]
        public async Task<IActionResult> UpdateUser(string userName, UserAuth updateUser) {
            if (updateUser == null) {
                return BadRequest();
            }

            UserDb userDb = await ContextDb.GetAsync(userName);
            if (userDb == null) {
                return NotFound();
            }

            UserDb newUser = await ContextDb.GetAsync(updateUser.Name);
            if (newUser != null && !newUser.Equals(userDb)) {
                return Conflict("Пользователь с таким именем уже зарегистрирован");
            }

            userDb.Name = updateUser.Name;
            userDb.HashPassword = updateUser.Password;

            await ContextDb.UpdateAsync(userDb);
            return new ObjectResult(userDb);
        }

        /// <summary>
        /// Удаляет пользователя из БД.
        /// </summary>
        /// <param name="id">Id пользователя, которого надо удалить.</param>
        /// <returns>Возвращает удалённого пользователя.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id) {
            UserDb userDb = await ContextDb.GetAsync(id);
            if (userDb == null) {
                return NotFound();
            }
            await ContextDb.DeleteAsync(userDb);
            return new ObjectResult(userDb);
        }

        /// <summary>
        /// Удаляет пользователя из БД.
        /// </summary>
        /// <param name="userName">Имя пользователя, которого надо удлаить.</param>
        /// <returns>Возвращает удалённого пользователя.</returns>
        [HttpDelete("name/{userName:maxlength(50)}")]
        public async Task<IActionResult> DeleteUser(string userName) {
            UserDb userDb = await ContextDb.GetAsync(userName);
            if (userDb == null) {
                return NotFound();
            }
            await ContextDb.DeleteAsync(userDb);
            return new ObjectResult(userDb);
        }
    }
}