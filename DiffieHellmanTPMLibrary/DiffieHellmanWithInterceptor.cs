using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiffieHellmanTPMLibrary {
    /// <summary>
    /// Протокол Диффи-Хеллмана с реализацией перехватчика
    /// </summary>
    public class DiffieHellmanWithInterceptor : DiffieHellmanWithSyncPoints {
        private TreeParityMachine MachineInterceptor;
        private List<double> SyncPointsInterceptor1;
        private List<double> SyncPointsInterceptor2;

        /// <summary>
        /// Событие передачи точек синхронизации перехватчика
        /// </summary>
        public event EventHandler<Tuple<double[], double[]>> InterceptorSyncPointsEvent;

        public event EventHandler SuccessInterceptor;

        public DiffieHellmanWithInterceptor() : base() {
            SyncPointsInterceptor1 = new List<double>();
            SyncPointsInterceptor2 = new List<double>();

            SyncPointsEvent += (s, e) => {
                // Вызов события на получение значений синхронизации перехватчика
                Tuple<double[], double[]> tupleSyncPoints = 
                    new Tuple<double[], double[]>(SyncPointsInterceptor1.ToArray(), SyncPointsInterceptor2.ToArray());

                InterceptorSyncPointsEvent?.Invoke(this, tupleSyncPoints);
            };
        }

        /// <summary>
        /// Запуск протокола Диффи-Хеллмана с древовидной машиной четности
        /// </summary>
        /// <param name="numInputNeurons">Количество входных нейронов в ДМЧ <see cref="TreeParityMachine"/></param>
        /// <param name="numHiddenNeoruns">Количество скрытых нейронов в ДМЧ <see cref="TreeParityMachine"/></param>
        /// <param name="weightRange">Диапозон весов в ДМЧ <see cref="TreeParityMachine"/></param>
        /// <param name="rule">Правило обучения нейронов в ДМЧ <see cref="TreeParityMachine"/></param>
        public override async Task RunProtocolAsync(uint numInputNeurons, uint numHiddenNeoruns, int weightRange, LearningRuleNeurons rule) {
            MachineInterceptor = new TreeParityMachine(numInputNeurons, numHiddenNeoruns, weightRange, rule);
            await base.RunProtocolAsync(numInputNeurons, numHiddenNeoruns, weightRange, rule).ConfigureAwait(false);
        }

        /// <summary>
        /// Получение случайных входных значений (-1, 1) от сервера
        /// </summary>
        /// <param name="valuesInput">Матрица случайных входных значений (-1, 1)</param>
        public override async Task ReceiveValuesInputAsync(int[][] valuesInput) {
            await Task.Run(() => MachineInterceptor.GetTau(valuesInput)).ConfigureAwait(false);
            await base.ReceiveValuesInputAsync(valuesInput).ConfigureAwait(false);
        }

        /// <summary>
        /// Получение тау (результата ДМЧ) авбонента от сервера
        /// </summary>
        /// <param name="inputTau">тау (результата ДМЧ) авбонента</param>
        public override async Task ReceiveTauAsync(int inputTau) {
            if (Machine.Tau == inputTau && MachineInterceptor.Tau == inputTau) {
                await Task.Run(() => MachineInterceptor.UpdateWeighs(inputTau)).ConfigureAwait(false);
            }
            await base.ReceiveTauAsync(inputTau).ConfigureAwait(false);
        }

        /// <summary>
        /// Получение проверки синхронизации ДМЧ от сервера
        /// </summary>
        /// <param name="hashOtherWeights">Хэш-значение другого абонента</param>
        public override async Task ReceiveHashWeightsAsync(string hashOtherWeights) {
            // проверка синхронизации перехватчика
            string hashWeightsInretceptor = MachineInterceptor.GetHashWeights();
            if (hashWeightsInretceptor.Equals(HashWeights) ||
                hashWeightsInretceptor.Equals(hashOtherWeights))
            {
                SuccessInterceptor?.Invoke(this, EventArgs.Empty);
                return;
            }
            await base.ReceiveHashWeightsAsync(hashOtherWeights);
        }

        /// <summary>
        /// Получение значения весов ДМЧ абонента
        /// </summary>
        /// <param name="otherWeights">Веса второго абонента</param>
        public override void ReceiveValueSyncWeightsAsync(int[][] otherWeights) {
            base.ReceiveValueSyncWeightsAsync(otherWeights);

            double averageDifference1 = ComputeAverageDifferenceWeights(MachineInterceptor.GetWeights(), Machine.GetWeights());
            double averageDifference2 = ComputeAverageDifferenceWeights(MachineInterceptor.GetWeights(), otherWeights);
            // вычисление процента синхронизации весов
            double syncPercentageInterceptor1 = (1.0 - averageDifference1) * 100.0;
            double syncPercentageInterceptor2 = (1.0 - averageDifference2) * 100.0;

            SyncPointsInterceptor1.Add(syncPercentageInterceptor1);
            SyncPointsInterceptor2.Add(syncPercentageInterceptor2);
        }
    }
}
