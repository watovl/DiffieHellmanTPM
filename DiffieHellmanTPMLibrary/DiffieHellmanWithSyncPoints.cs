using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace DiffieHellmanTPMLibrary {
    /// <summary>
    /// Протокол Диффи-Хеллмана с списком точек процесса синхронизации
    /// </summary>
    public class DiffieHellmanWithSyncPoints : DiffieHellman {
        /// <summary>
        /// Список значений синхронизации
        /// </summary>
        private List<double> SyncPoints;

        /// <summary>
        /// Событие передачи точек синхронизации
        /// </summary>
        public event EventHandler<double[]> SyncPointsEvent;


        public DiffieHellmanWithSyncPoints() : base() {
            SyncPoints = new List<double>();

            FinishedProtocol += (s, e) => {
                // Вызов события на получение значений синхронизации
                SyncPointsEvent?.Invoke(this, SyncPoints.ToArray());
            };
        }

        /// <summary>
        /// Установка методов для вызова сервером
        /// </summary>
        protected override void SetHubConnectionOn() {
            HubProtocolConnection.On<int[][]>("ReceiveValueSyncWeights", weights => ReceiveValueSyncWeightsAsync(weights));
            base.SetHubConnectionOn();
        }

        /// <summary>
        /// Получение тау (результата ДМЧ) авбонента от сервера
        /// </summary>
        /// <param name="inputTau">тау (результата ДМЧ) авбонента</param>
        public override async Task ReceiveTauAsync(int inputTau) {
            await base.ReceiveTauAsync(inputTau);
            try {
                // Запрос на получение весов ДМЧ абонента
                await HubProtocolConnection.InvokeAsync("SendWeights",
                    NameRecipient, Machine.GetWeights(), Machine.WeightRange).ConfigureAwait(false);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Вычисление средней разницы между весами (насколько они различаются в среднем)
        /// </summary>
        /// <returns> Возвращает число от 0 до 1, где 1 - максимальная разница весов, а 0 - веса полностью идентичны</returns>
        //protected double ComputeAverageDifferenceWeights(int[][] firstWeights, int[][] secondWeights) {
        //    double sum = 0.0;
        //    for (int i = 0; i < firstWeights.Length; ++i) {
        //        for (int j = 0; j < firstWeights[i].Length; ++j) {
        //            sum += Math.Pow((firstWeights[i][j] - secondWeights[i][j]), 2);
        //        }
        //    }
        //    sum /= (firstWeights.Length * firstWeights[0].Length);
        //    return Math.Sqrt(sum) / (2.0 * Machine.WeightRange);
        //}

        protected double ComputeAverageDifferenceWeights(int[][] firstWeights, int[][] secondWeights) {
            double sum = 0.0;
            for (int i = 0; i < firstWeights.Length; ++i) {
                for (int j = 0; j < firstWeights[i].Length; ++j) {
                    sum += Math.Abs(firstWeights[i][j] - secondWeights[i][j]);
                }
            }
            return sum / (2.0 * Machine.WeightRange) / (firstWeights.Length * firstWeights[0].Length);
        }

        //protected double ComputeSyncPercentage(int[][] firstWeights, int[][] secondWeights) {
        //    int diff = 0;
        //    for (int i = 0; i < firstWeights.Length; ++i) {
        //        for (int j = 0; j < firstWeights[i].Length; ++j) {
        //            if (firstWeights[i][j] == secondWeights[i][j]) {
        //                ++diff;
        //            }
        //        }
        //    }
        //    return diff * 100.0 / (firstWeights.Length * firstWeights[0].Length);
        //}

        /// <summary>
        /// Получение значения весов ДМЧ абонента
        /// </summary>
        /// <param name="otherWeights">Веса второго абонента</param>
        public virtual void ReceiveValueSyncWeightsAsync(int[][] otherWeights) {
            // средняя разница весов
            double averageDifference = ComputeAverageDifferenceWeights(Machine.GetWeights(), otherWeights);
            // вычисление процента синхронизации весов
            double syncPercentage = (1.0 - averageDifference) * 100.0; //ComputeSyncPercentage(Machine.GetWeights(), otherWeights); //
            SyncPoints.Add(syncPercentage);
            Debug.WriteLine($"Sync: {syncPercentage}%");
        }
    }
}
