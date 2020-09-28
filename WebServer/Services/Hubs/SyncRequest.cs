using System;
using System.Collections.Generic;

namespace WebServer.Services.Hubs {
    /// <summary>
    /// Параметры древовидной машины чётности.
    /// </summary>
    public class ParamsTreeParityMachine {
        public uint NumberInputNeurons;    // количество входных нейронов
        public uint NumberHiddenNeurons;   // количество нейронов скрытого слоя
        public int WeightRange;            // диапозон весов

        // Перегрузка метода проверки на равенство
        public bool Equals(ParamsTreeParityMachine machine) {
            if (machine == null)
                return false;

            return NumberInputNeurons == machine.NumberInputNeurons &&
                NumberHiddenNeurons == machine.NumberHiddenNeurons &&
                WeightRange == machine.WeightRange;
        }
    }

    /// <summary>
    /// Определяет способ синхронизации двух клиентов.
    /// </summary>
    public class SyncWay {
        public ParamsTreeParityMachine ParamsTPM { get; set; }  // проверка равенста параметров ДМЧ
        public bool isGetVector { get; set; }                   // получить случайно сгенерированный вектор
        public int? Tau { get; set; }                           // передать Тау (результат работы ДМЧ)
        public string HashedWeighs { get; set; }                // проверка равенства хэш весов ДМЧ
#if DEBUG
        public int[][] Weights { get; set; }                    // весса для синхронизации
#endif
    }

    /// <summary>
    /// Запрос на синхронизации с другим клиентом.
    /// </summary>
    public class SyncRequest {
        public string Sender { get; set; }      // отправитель
        public string Recipient { get; set; }   // получатель
        public SyncWay Way { get; set; }        // способ синхронизации
        public readonly DateTime Expires;       // время жизни запроса

        private static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(30); // время жизни запроса

        public SyncRequest() {
            Expires = DateTime.Now.Add(LifeTime);
        }
    }

    //public class Dict {
    //    private List<Dictionary<string, SyncRequest>> _listSyncRequests = new List<Dictionary<string, SyncRequest>>();
    //    private static readonly object locker = new object();

    //    public SyncRequest GetOrAdd(SyncRequest request, Predicate<Dictionary<string, SyncRequest>> match) {
    //        Dictionary<string, SyncRequest> foundRequest = null;
    //        foundRequest = _listSyncRequests.Find(match);

    //        lock (locker) {
    //            foundRequest = _listSyncRequests.Find(match);
    //            if (foundRequest == null) {
    //                _listSyncRequests.Add(request);
    //            }
    //            else {
    //                _listSyncRequests.Remove(foundRequest);
    //            }
    //        }
    //        return foundRequest;
    //    }
    //}

    public class ListRequests {
        private List<SyncRequest> _listSyncRequests = new List<SyncRequest>();

        private static readonly object locker = new object();

        /// <summary>
        /// Создаёт запрос <see cref="SyncRequest"/> на синхронизацию. Если запрос от второго абонента был, возвращает его
        /// </summary>
        /// <param name="request"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public SyncRequest GetOrAdd(SyncRequest request, Predicate<SyncRequest> match) {
            SyncRequest foundRequest = null;

            lock (locker) {
                foundRequest = _listSyncRequests.Find(match);
                if (foundRequest == null) {
                    _listSyncRequests.Add(request);
                }
                else {
                    _listSyncRequests.Remove(foundRequest);
                }
            }
            return foundRequest;
        }

        public void RemoveAll(Predicate<SyncRequest> match) {
            _listSyncRequests.RemoveAll(match);
        }
    }
}
