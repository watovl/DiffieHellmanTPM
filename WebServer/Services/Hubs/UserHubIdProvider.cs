using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WebServer.Services.Hubs {
    public class UserHubIdProvider : IUserIdProvider {
        /// <summary>
        /// Устанавливает, что будет возвращаться в качестве индефикатора пользователя для hub соединения.
        /// </summary>
        /// <param name="connection">Контекст hub соединения (сокеты).</param>
        /// <returns>Возвращает имя пользователя, установленное при аутентификации через JWT токены.</returns>
        public virtual string GetUserId(HubConnectionContext connection) {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
