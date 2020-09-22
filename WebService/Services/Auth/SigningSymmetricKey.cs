using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebService.Services.Authentication {
    /// <summary>
    /// Реализация интерфейсов <see cref="IJwtSigningEncodingKey"/> и <see cref="IJwtSigningDecodingKey"/>.
    /// Генерирует и возвращает секретный ключ и определяет алгоритм кодирования.
    /// </summary>
    public class SigningSymmetricKey : IJwtSigningEncodingKey, IJwtSigningDecodingKey {
        private readonly SymmetricSecurityKey _secretKey;

        /// <summary>
        /// Алгоритм кодирования
        /// </summary>
        public string SigningAlgorithm { get; } = SecurityAlgorithms.HmacSha256;

        /// <summary>
        /// Генерирует новый секретный ключ на основе входящего <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Ключ для генерации нового секретного ключа.</param>
        public SigningSymmetricKey(string key) {
            _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// Возвращает секретный ключ типа <see cref="SecurityKey"/>.
        /// </summary>
        public SecurityKey GetKey() => _secretKey;
    }
}
