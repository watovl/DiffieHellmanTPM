using System;
using System.Security.Cryptography;

namespace WebService.Services.Hashing {
    /// <summary>
    /// Класс для генерации случайной последовательности
    /// Используется для генерации хэша.
    /// </summary>
    public class Salt {
        /// <summary>
        /// Генерирует случайную последовательность длиной 16 байт (128 бит).
        /// </summary>
        /// <returns>Возвращает случайную последовательность длиной 16 байт (128 бит).</returns>
        public static byte[] Create() {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create()) {
                generator.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
