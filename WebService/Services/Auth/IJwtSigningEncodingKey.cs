using Microsoft.IdentityModel.Tokens;

namespace WebService.Services.Authentication {
    /// <summary>
    /// Определяет способ кодирования и функцию получения секретного ключа.
    /// </summary>
    public interface IJwtSigningEncodingKey {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}
