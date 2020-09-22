using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebServer.Services.Hashing {
    /// <summary>
    /// Класс для работы с хэшом.
    /// </summary>
    public class Hash {
        /// <summary>
        /// Генерирует хэш значение.
        /// </summary>
        /// <param name="value">Значение, которое надо хэшировать.</param>
        /// <param name="salt">Случайная последовательность в байтах.</param>
        /// <returns>Возвращает хэш входного значение <paramref name="value"/> в строковом виде.</returns>
        public static string Create(string value, byte[] salt) {
            byte[] valueBytes = KeyDerivation.Pbkdf2(
                                password: value,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA256,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        /// <summary>
        /// Проверяет на равенство хэш входного значения <paramref name="value"/> и предоставленный хэш <paramref name="hash"/>.
        /// </summary>
        /// <param name="value">Значение, которое надо хэшировать.</param>
        /// <param name="salt">Случайная последовательность в байтах.</param>
        /// <param name="hash">Хэш значение, с которым надо сравнивать сгенированный 
        /// хэш значения <paramref name="value"/>.</param>
        /// <returns>Возвращает булевый результат проверки на равенство хэша входного 
        /// значения <paramref name="value"/> и предоставленного хэша <paramref name="hash"/>.</returns>
        public static bool Validate(string value, byte[] salt, string hash)
            => Create(value, salt) == hash;
    }
}
