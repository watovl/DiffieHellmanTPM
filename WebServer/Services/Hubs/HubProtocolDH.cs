using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WebServer.Services.Hubs {
    /// <summary>
    /// Синхронизация двух клиентов и генерация секретного ключа между ними через сокеты.
    /// </summary>
    /// <remarks>Необходимо быть авторизованным.</remarks>
    [Authorize]
    public class HubProtocolDH : Hub {
        private readonly ILogger _logger;
        private ListRequests SyncRequestsList;

        public HubProtocolDH(ILogger<HubProtocolDH> logger, ListRequests syncList) {
            _logger = logger;
            SyncRequestsList = syncList;
        }

        ///<summary>
        /// Генерация двумерного массива (массив массивов) размерностью <paramref name="numberInputNeurons"/> на
        /// <paramref name="numberHiddenNeurons"/> случайными значениями 1 или -1.
        /// </summary>
        /// <param name="numberInputNeurons">Размер первого измерения (кол-во входных нейронов в ДМЧ).</param>
        /// <param name="numberHiddenNeurons">Размер второго измерения (кол-во скрытых нейронов в ДМЧ).</param>
        /// <returns>Возвращает двумерный массив со случайными значениями 1 или -1.</returns>
        private int[][] GetRandomVector(uint numberInputNeurons, uint numberHiddenNeurons) {
            int[][] randomVector = new int[numberHiddenNeurons][];
            Random random = new Random();
            for (int i = 0; i < numberHiddenNeurons; ++i) {
                randomVector[i] = new int[numberInputNeurons];
                for (int j = 0; j < numberInputNeurons; ++j)
                    randomVector[i][j] = random.Next() % 2 == 0 ? 1 : -1;
            }
            return randomVector;
        }

        /// <summary>
        /// Запрос на начало выполнение протокола Диффи-Хиллмана между двумя клиентами. 
        /// </summary>
        /// <param name="recipient">Получатель (второй абонент) результата для синхронизации абонентов.</param>
        /// <param name="numberInputNeurons">Размер первого измерения (кол-во входных нейронов в ДМЧ).</param>
        /// <param name="numberHiddenNeurons">Размер второго измерения (кол-во скрытых нейронов в ДМЧ).</param>
        /// <param name="weightRange">Диапозон весов ДМЧ.</param>
        /// <returns>
        /// Если запрос от второго абонента был - то вызывает функцию <see cref="ReceiveEqualsParams"/> 
        /// у обоих клиентов с результатом проверки
        /// Иначе - записывает запрос в список запросов с нужный методом синхронизации.
        /// </returns>
        public async Task CompareParams(string recipient, uint numberInputNeurons, uint numberHiddenNeurons, int weightRange) 
        {
            SyncRequest request = new SyncRequest {
                Sender = Context.UserIdentifier,
                Recipient = recipient,
                Way = new SyncWay {
                    ParamsTPM = new ParamsTreeParityMachine {
                        NumberInputNeurons = numberInputNeurons,
                        NumberHiddenNeurons = numberHiddenNeurons,
                        WeightRange = weightRange
                    }
                }
            };
            // поиск запроса от userName на синхронизацию
            SyncRequest recipientRequest = SyncRequestsList.GetOrAdd(request, syncReq =>
                syncReq.Sender == recipient && syncReq.Recipient == Context.UserIdentifier && syncReq.Way != null);

            if (recipientRequest != null) {
                // проверка на равенство параметров ДМЧ
                bool isEquals = recipientRequest.Way.ParamsTPM.Equals(request.Way.ParamsTPM);
                // отправка результата сравнения
                await Clients.User(Context.UserIdentifier).SendAsync("ReceiveEqualsParams", isEquals);
                await Clients.User(recipient).SendAsync("ReceiveEqualsParams", isEquals);
            }
        }

        /// <summary>
        /// Запрос на получение случайных входных значений для ДМЧ.
        /// </summary>
        /// <param name="recipient">Получатель (второй абонент).</param>
        /// <param name="numberInputNeurons">Размер первого измерения (кол-во входных нейронов в ДМЧ).</param>
        /// <param name="numberHiddenNeurons">Размер второго измерения (кол-во скрытых нейронов в ДМЧ).</param>
        /// <returns>
        /// Если запрос от второго абонента на получение случайного массива был - то 
        /// вызывается функция <see cref="ReceiveValuesInput"/> у обоих клиентов и передаёт 
        /// в неё двумерный массив со случайными значениями -1 и 1
        /// Иначе - запрос записывается в список запросов с нужный методом синхронизации.
        /// </returns>
        public async Task SendValuesInput(string recipient, uint numberInputNeurons, uint numberHiddenNeurons) {
            SyncRequest request = new SyncRequest {
                Sender = Context.UserIdentifier,
                Recipient = recipient,
                Way = new SyncWay {
                    isGetVector = true
                }
            };
            // поиск запроса от userName на получение случайного вектора
            SyncRequest recipientRequest = SyncRequestsList.GetOrAdd(request, syncReq =>
                syncReq.Sender == recipient && syncReq.Recipient == Context.UserIdentifier && syncReq.Way.isGetVector);

            if (recipientRequest != null) {
                // генерация вектора
                int[][] randomVector = await Task.Run(() => GetRandomVector(numberInputNeurons, numberHiddenNeurons));

                await Clients.User(Context.UserIdentifier).SendAsync("ReceiveValuesInput", randomVector);
                await Clients.User(recipient).SendAsync("ReceiveValuesInput", randomVector);
            }
        }

        /// <summary>
        /// Запрос на отправку <paramref name="tau"/> значений Тау (результат выполнения ДМЧ) второму абоненту.
        /// </summary>
        /// <param name="recipient">Получатель (второй абонент).</param>
        /// <param name="tau">Результат выполнения ДМЧ, который нужно передать второму абоненту.</param>
        /// <returns>
        /// Если запрос на передачу Тау от второго абонента был - то вызывается функция <see cref="ReceiveTau"/>
        /// у обоих клиентов, передавая Тау другого абонента
        /// Иначе - запрос записывается в список запросов с нужный методом синхронизации.
        /// </returns>
        public async Task SendTau(string recipient, int tau) {
            SyncRequest request = new SyncRequest {
                Sender = Context.UserIdentifier,
                Recipient = recipient,
                Way = new SyncWay {
                    Tau = tau
                }
            };
            // поиск запроса от userName на передачу тау
            SyncRequest recipientRequest = SyncRequestsList.GetOrAdd(request, syncReq =>
                syncReq.Sender == recipient && syncReq.Recipient == Context.UserIdentifier && syncReq.Way.Tau != null);

            if (recipientRequest != null) {
                // отправка Тау обоим клиентам
                await Clients.User(Context.UserIdentifier).SendAsync("ReceiveTau", recipientRequest.Way.Tau.Value);
                await Clients.User(recipient).SendAsync("ReceiveTau", tau);
            }
        }

        /// <summary>
        /// Проверка синхронизации весов ДМЧ обоих клиентов по хэшу этих весов <paramref name="hashWeights"/>.
        /// </summary>
        /// <param name="recipient">Получатель (второй абонент).</param>
        /// <param name="hashWeights">Значение хэша весов ДМЧ.</param>
        /// <returns>
        /// Если запрос на синхронизацию весов ДМЧ от второго абонента был - вызывается функция <see cref="ReceiveSyncWeights"/>
        /// у обоих клиентов с булевым результатом синхронизации
        /// Иначе - запрос записывается в список запросов с нужный методом синхронизации.
        /// </returns>
        public async Task SendHashWeights(string recipient, string hashWeights) {
            SyncRequest request = new SyncRequest {
                Sender = Context.UserIdentifier,
                Recipient = recipient,
                Way = new SyncWay {
                    HashedWeighs = hashWeights
                }
            };
            // поиск запроса от userName на синхронизацию весов
            SyncRequest recipientRequest = SyncRequestsList.GetOrAdd(request, syncReq =>
                syncReq.Sender == recipient &&
                syncReq.Recipient == Context.UserIdentifier &&
                !string.IsNullOrEmpty(syncReq.Way.HashedWeighs));

            if (recipientRequest != null) {
                // отправка результата синхронизации весов обоим клиентам
                await Clients.User(Context.UserIdentifier).SendAsync("ReceiveHashWeights", recipientRequest.Way.HashedWeighs);
                await Clients.User(recipient).SendAsync("ReceiveHashWeights", hashWeights);
            }
        }

#if DEBUG
        /// <summary>
        /// Вычисление синхронизации весов ДМЧ в процентах.
        /// </summary>
        /// <param name="recipient">Получатель (второй абонент).</param>
        /// <param name="weights">Веса ДМЧ.</param>
        /// <param name="weightRange">Диапозон весов.</param>
        /// <returns>
        /// Если запрос на синхронизацию весов ДМЧ от второго абонента был - возвращает процент синхронизации весов ДМЧ.
        /// Иначе - запрос записывается в список запросов с нужный методом синхронизации.
        /// </returns>
        public async Task SendWeights(string recipient, int[][] weights, int weightRange) {
            SyncRequest request = new SyncRequest {
                Sender = Context.UserIdentifier,
                Recipient = recipient,
                Way = new SyncWay {
                    Weights = weights
                }
            };
            SyncRequest recipientRequest = SyncRequestsList.GetOrAdd(request, syncReq =>
                syncReq.Sender == recipient &&
                syncReq.Recipient == Context.UserIdentifier &&
                syncReq.Way.Weights != null);

            if (recipientRequest != null) {
                await Clients.User(Context.UserIdentifier).SendAsync("ReceiveValueSyncWeights", recipientRequest.Way.Weights);
                await Clients.User(recipient).SendAsync("ReceiveValueSyncWeights", weights);
            }
        }
#endif

        /// <summary>
        /// Вызывается при подключении клиента.
        /// </summary>
        public override async Task OnConnectedAsync() {
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Вызывается при отключении клиента
        /// </summary>
        /// <param name="exception">Информация об ошибка, если она была.</param>
        public override async Task OnDisconnectedAsync(Exception exception) {
            // удаление всех запросов от клиента
            SyncRequestsList.RemoveAll(syncReq => syncReq.Sender == Context.UserIdentifier);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
