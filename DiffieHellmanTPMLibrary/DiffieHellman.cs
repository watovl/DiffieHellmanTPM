using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;

namespace DiffieHellmanTPMLibrary {
    /// <summary>
    /// Алгоритм обмена секретного ключа Диффи-Хеллмана реализованый через древовидную 
    /// машину чётности <see cref="TreeParityMachine"/>.
    /// </summary>
    public class DiffieHellman : IAsyncDisposable, IDisposable {
        protected TreeParityMachine Machine;
        private readonly HttpClient HttpResponse;
        protected readonly HubConnection HubProtocolConnection;
        private string ConnectionToken;
        CancellationTokenSource ClosedTokenSource;
        protected string NameRecipient;
        
        protected string HashWeights;
        // ----------------------------------------------------------------------------------------
        /* СОБЫТИЯ */

        /// <summary>
        /// Событие срабатывающее при разных параметрах ДМЧ у абонентов.
        /// </summary>
        public event EventHandler DifferentParamsEvent;
        /// <summary>
        /// Событие начала синхронизации абонентов после проверки равенства параметров
        /// </summary>
        public event EventHandler BeginningSync;
        /// <summary>
        /// Событие при завершении удачной работы протокола.
        /// </summary>
        public event EventHandler FinishedProtocol;
        /// <summary>
        /// Событие срабатывающее когда секретный ключ был сгенерирован.
        /// </summary>
        public event EventHandler<string> GeneratedSecretKey;
        /// <summary>
        /// Событие закрытия соединения
        /// </summary>
        public event EventHandler ClosedConnection;
        /// <summary>
        /// Получатель отключился
        /// </summary>
        public event EventHandler DisconnectRecipient;

        // ----------------------------------------------------------------------------------------


        public DiffieHellman() {
            // создание http соединения
            HttpResponse = new HttpClient();
            // создание соединения через сокеты
            HubProtocolConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44355/protocoldh", options => {
                    options.AccessTokenProvider = () => Task.FromResult(ConnectionToken);
                })
                .ConfigureLogging(logging => {
                    //logging.AddDebug();
                    //logging.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();
            HubProtocolConnection.ServerTimeout = TimeSpan.FromSeconds(60);
            HubProtocolConnection.KeepAliveInterval = TimeSpan.FromSeconds(30);

            FinishedProtocol += (s, e) => {
                // Вызов события на получение секретного ключа
                GeneratedSecretKey?.Invoke(this, Machine.GetSecretKey());
            };
        }

        /// <summary>
        /// Получение токена для аутентификации
        /// </summary>
        /// <param name="userName">Имя пользователя зарегистрированного на сервере</param>
        /// <param name="password">Пароль пользователя данного пользователя</param>
        /// <returns>Возвращает токен</returns>
        private async Task<string> GetTokenAsync(string userName, string password) {
            string content = JsonConvert.SerializeObject(new { name = userName, password });
            HttpResponseMessage response = await HttpResponse.PostAsync("auth",
                new StringContent(content, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Установка методов для вызова сервером
        /// </summary>
        protected virtual void SetHubConnectionOn() {
            HubProtocolConnection.On<bool>("ReceiveEqualsParams", async isEquals => await ReceiveEqualsParamsAsync(isEquals));
            HubProtocolConnection.On<int[][]>("ReceiveValuesInput", async randomVector => await ReceiveValuesInputAsync(randomVector));
            HubProtocolConnection.On<int>("ReceiveTau", async tau => await ReceiveTauAsync(tau));
            HubProtocolConnection.On<string>("ReceiveHashWeights", async hashWeights => await ReceiveHashWeightsAsync(hashWeights));
        }

        /// <summary>
        /// Соединение с сервером
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="nameRecipient">Имя получателя</param>
        public async Task ConnectAsync(string userName, string password, string nameRecipient) {
            HttpResponse.BaseAddress = new Uri("https://localhost:44355/api/");
            Task<string> gettingToken = GetTokenAsync(userName, password);

            ClosedTokenSource = new CancellationTokenSource();
            ClosedTokenSource.Token.Register(() => {
                if (HubProtocolConnection != null && HubProtocolConnection.State == HubConnectionState.Connected) {
                    DisconnectRecipient?.Invoke(this, EventArgs.Empty);
                }
            });

            HubProtocolConnection.Closed += (error) => {
                Console.WriteLine("Connection closed...");
                ClosedConnection?.Invoke(this, EventArgs.Empty);
                return Task.CompletedTask;
            };

            SetHubConnectionOn();

            try {
                // получение токена
                ConnectionToken = await gettingToken;
                Debug.WriteLine("Получен токен: " + ConnectionToken);

                // Старт соединения
                await HubProtocolConnection.StartAsync().ConfigureAwait(false);
                Debug.WriteLine("Connection hub start");
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                throw ex;
            }
            // установка получателя
            NameRecipient = nameRecipient;
        }

        /// <summary>
        /// Запуск протокола Диффи-Хеллмана с древовидной машиной четности
        /// </summary>
        /// <param name="numInputNeurons">Количество входных нейронов в ДМЧ <see cref="TreeParityMachine"/></param>
        /// <param name="numHiddenNeoruns">Количество скрытых нейронов в ДМЧ <see cref="TreeParityMachine"/></param>
        /// <param name="weightRange">Диапозон весов в ДМЧ <see cref="TreeParityMachine"/></param>
        /// <param name="rule">Правило обучения нейронов в ДМЧ <see cref="TreeParityMachine"/></param>
        public virtual async Task RunProtocolAsync(uint numInputNeurons, uint numHiddenNeoruns, int weightRange, LearningRuleNeurons rule) {
            Machine = new TreeParityMachine(numInputNeurons, numHiddenNeoruns, weightRange, rule);
            Debug.WriteLine("Run Protocol");
            // Проверка параметров ДМЧ
            try {
                ClosedTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
                await HubProtocolConnection.InvokeAsync("CompareParams", 
                    NameRecipient, numInputNeurons, numHiddenNeoruns, weightRange).ConfigureAwait(false);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------
        /* Функции для вызова сервером через Hub соединение */

        /// <summary>
        /// Получение результата сравнения параметров ДМЧ
        /// </summary>
        /// <param name="isEquals">Булевый результат сравнения параметров ДМЧ</param>
        public async Task ReceiveEqualsParamsAsync(bool isEquals) {
            Debug.WriteLine("Receive result compare params TPM");
            if (isEquals) {
                // событие начала синхронизации
                BeginningSync?.Invoke(this, EventArgs.Empty);

                ClosedTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
                // Запрос на получение входного значения
                try {
                    await HubProtocolConnection.InvokeAsync("SendValuesInput",
                        NameRecipient, Machine.NumberInputNeurons, Machine.NumberHiddenNeurons).ConfigureAwait(false);
                }
                catch (Exception ex) {
                    Debug.WriteLine(ex);
                    throw ex;
                }
            }
            else {
                // вызов события - разные параметры
                DifferentParamsEvent?.Invoke(this, EventArgs.Empty);
                Debug.WriteLine("Разные параметры ДМЧ");
            }
        }

        /// <summary>
        /// Получение случайных входных значений (-1, 1) от сервера
        /// </summary>
        /// <param name="valuesInput">Матрица случайных входных значений (-1, 1)</param>
        public virtual async Task ReceiveValuesInputAsync(int[][] valuesInput) {
            Debug.WriteLine("Get values input");
            int tau = await Task.Run(() => Machine.GetTau(valuesInput));
            
            ClosedTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
            // Отправка тау на сервер
            try {
                await HubProtocolConnection.InvokeAsync("SendTau", NameRecipient, tau).ConfigureAwait(false);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Получение тау (результата ДМЧ) авбонента от сервера
        /// </summary>
        /// <param name="inputTau">тау (результата ДМЧ) авбонента</param>
        public virtual async Task ReceiveTauAsync(int inputTau) {
            Debug.WriteLine("Get Tau");
            if (Machine.Tau.Equals(inputTau)) {
                await Task.Run(() => Machine.UpdateWeighs(inputTau));
            }
            HashWeights = Machine.GetHashWeights();

            ClosedTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
            // Отправка хэш весов на сервер
            try {
                await HubProtocolConnection.InvokeAsync("SendHashWeights", 
                    NameRecipient, HashWeights).ConfigureAwait(false);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Получение проверки синхронизации ДМЧ от сервера
        /// </summary>
        /// <param name="hashOtherWeights">Хэш-значение другого абонента</param>
        public virtual async Task ReceiveHashWeightsAsync(string hashOtherWeights) {
            if (!HashWeights.Equals(hashOtherWeights)) {
                Debug.WriteLine("Send random vector");
                ClosedTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
                try {
                    // Запрос на получение входного значения
                    await HubProtocolConnection.InvokeAsync("SendValuesInput", 
                        NameRecipient, Machine.NumberInputNeurons, Machine.NumberHiddenNeurons).ConfigureAwait(false);
                }
                catch (Exception ex) {
                    Debug.WriteLine(ex);
                    throw ex;
                }
            }
            else {
                Debug.WriteLine("Success sync TPM");
                // Вызов события завершения работы протокола
                FinishedProtocol?.Invoke(this, EventArgs.Empty);
                await HubProtocolConnection.DisposeAsync();
            }
        }

        // ----------------------------------------------------------------------------------------
        /* Реализация дескриптора */
#region IDisposable
        private bool disposed = false; // Для определения избыточных вызовов

        ~DiffieHellman() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    // Освобождаем управляемые ресурсы
                    HttpResponse?.Dispose();
                    HubProtocolConnection?.DisposeAsync().GetAwaiter().GetResult();
                }
                disposed = true;
            }
        }

        public async ValueTask DisposeAsync() {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore() {
            if (HubProtocolConnection != null) {
                await HubProtocolConnection.DisposeAsync().ConfigureAwait(false);
                HttpResponse?.Dispose();
            }
        }
#endregion
    }
}
