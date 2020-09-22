using System;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using WebServer.Models;
using WebServer.Services.Authentication;
using WebServer.Services.Hashing;
using WebServer.Services.ProxyDb;

namespace WebServer.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации пользователя через JWT-токены
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUsersDataBase ContextDb;
        private readonly ILogger _logger;

        // подключение БД
        public AuthController(IUsersDataBase context, ILogger<AuthController> logger) {
            ContextDb = context;
            _logger = logger;
        }

        /// <summary>
        /// Генерирует и возвращает токен для пользователя зарегестрированного в системе.
        /// </summary>
        /// <param name="userAuth">Пользователь класса <see cref="AuthModel"/> для которого генерируется токен.</param>
        /// <param name="signingEncodingKey">Устанавливается сервером.
        /// Интерфейс <see cref="IJwtSigningEncodingKey"/> определяющий алгоритм кодирование токена и секретный ключ.</param>
        /// <returns>Возвращает сгенерированный токен пользователя. Используется для аутентификации.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<string>> GetToken(AuthModel userAuth, [FromServices] IJwtSigningEncodingKey signingEncodingKey) {
            // Проверяем данные пользователя из запроса.
            if (!ModelState.IsValid) {
                _logger.LogInformation($"Неверный запрос");
                return BadRequest(ModelState);
            }

            User userDb = await ContextDb.GetAsync(userAuth.Name);
            if (userDb == null || !Hash.Validate(userAuth.Password, userDb.Sault, userDb.HashPassword)) {
                _logger.LogInformation($"Неверный пользователь");
                return Unauthorized($"Данный пользователь не зарегистрирован в системе.");
            }

            // Создаем утверждения для токена.
            var claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, userAuth.Name),
                new Claim(ClaimTypes.Role, userDb.Role.Name)
            };

            // Генерируем JWT.
            var token = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.Now.Add(AuthOptions.LIFETIME),
                signingCredentials: new SigningCredentials(
                        signingEncodingKey.GetKey(),
                        signingEncodingKey.SigningAlgorithm)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation($"Для пользователя {userAuth.Name} создан токен: {jwtToken}");
            return Ok(jwtToken);
        }
    }
}
