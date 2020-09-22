using Microsoft.IdentityModel.Tokens;

namespace WebServer.Services.Authentication {
    /// <summary>
    /// Определяет функцию возвращения секретного ключа для декодирования
    /// </summary>
    public interface IJwtSigningDecodingKey {
        SecurityKey GetKey();
    }
}
