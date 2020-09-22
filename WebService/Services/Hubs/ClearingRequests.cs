using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Services.Hubs {
    /// <summary>
    /// Класс для очистки запросов на синхронизацию с истёкшем временим жизни.
    /// </summary>
    /// <remarks>Выполняется в фоновом режими.</remarks>
    public class ClearingRequests : BackgroundService {
        private readonly ILogger<ClearingRequests> Logger;
        private readonly ListRequests SyncRequests;
        private const int TimeDelay = 120000; // Задержка на 5 минут (в миллисекундах)

        public ClearingRequests(ILogger<ClearingRequests> logger, ListRequests syncRequestsList) {
            Logger = logger;
            SyncRequests = syncRequestsList;
        }

        /// <summary>
        /// Перегруженная функция, которая чистит список запросов от 
        /// запросов с истёкшим временем <see cref="SyncRequest.Expires"/>.
        /// </summary>
        /// <param name="stoppingToken">Токен для отмены операции.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                Logger.LogInformation("ClearingRequests running at: {Time}", DateTime.Now);
                // удаление элементов с истёкшем временим жизни
                 SyncRequests.RemoveAll(requset => requset.Expires < DateTime.Now);
                await Task.Delay(TimeDelay); // задержка
            }
        }
    }
}
