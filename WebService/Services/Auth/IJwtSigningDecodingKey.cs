using Microsoft.IdentityModel.Tokens;

namespace WebService.Services.Authentication {
    /// <summary>
    /// Определяет функцию возвращения секретного ключа для декодирования
    /// </summary>
    public interface IJwtSigningDecodingKey {
        SecurityKey GetKey();
    }
}
