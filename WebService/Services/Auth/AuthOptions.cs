
using System;
using WebService.Services.Hashing;

namespace WebService.Services.Authentication {
    /// <summary>
    /// Устанавливает параметры для JWT сокетов
    /// </summary>
    public class AuthOptions {
        public const string ISSUER = "WebProtocolDH";                                   // издатель токена
        public const string AUDIENCE = "WebClient";                                     // потребитель токена
        public const string KEY = "0d5b3235a8b403c3dab9c3f4f65c07fcalskd234n1k41230";   // ключ для шифрации
        public static readonly TimeSpan LIFETIME = TimeSpan.FromMinutes(10);            // время жизни токена
    }
}
