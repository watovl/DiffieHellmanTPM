using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiffieHellmanTPMLibrary {
    /// <summary>
    /// Правила обучения нейронов для древовидной машины четности.
    /// </summary>
    public enum LearningRuleNeurons : byte {
        Hebbian, AntiHebbian, RandomWalk
    }

    /// <summary>
    /// Древовидная машина чётности (ДМЧ). Используется в протоколе Диффи-Хеллмана для генерации секретного ключа.
    /// </summary>
    public class TreeParityMachine {
        /// <summary>
        /// Количество входных нейронов
        /// </summary>
        public readonly uint NumberInputNeurons;
        /// <summary>
        /// Количество нейронов скрытого слоя
        /// </summary>
        public readonly uint NumberHiddenNeurons;
        /// <summary>
        /// Диапозон весов
        /// </summary>
        public readonly int WeightRange;
        /// <summary>
        /// Весовая матрица входных нейронов размером <see cref="NumberHiddenNeurons"/> на <see cref="NumberInputNeurons"/>
        /// </summary>
        private readonly int[][] WeightsNeurons;
        /// <summary>
        /// Правило обучение весов
        /// </summary>
        private readonly LearningRuleNeurons LearningRule;

        /* Промежуточные значения */

        /// <summary>
        /// Значения, поступающие на входные нейроны
        /// </summary>
        private int[][] ValuesInputNeurons;
        /// <summary>
        /// Значения сигмы функции для скрытых нейронов
        /// </summary>
        private int[] SignumsNeurons;
        /// <summary>
        /// Значение выполнения работы ДМЧ (Тау)
        /// </summary>
        public int Tau { get; private set; }


        /// <param name="numberInputNeurons">Количество входных нейронов.</param>
        /// <param name="numberHiddenNeurons">Количество нейронов скрытого слоя.</param>
        /// <param name="weightRange">Диапозон весов ДМЧ</param>
        /// /// <param name="learningRule">Правило обучения <see cref="LearningRuleNeurons"/> нейронов.
        /// По умолчанию стоит правило <see cref="LearningRuleNeurons.Hebbian"/>.</param>
        public TreeParityMachine(uint numberInputNeurons, uint numberHiddenNeurons, int weightRange, 
            LearningRuleNeurons learningRule = LearningRuleNeurons.Hebbian) 
        {
            NumberInputNeurons = numberInputNeurons;
            NumberHiddenNeurons = numberHiddenNeurons;
            WeightRange = weightRange;
            LearningRule = learningRule;
            
            WeightsNeurons = GenerationWeights(numberInputNeurons, numberHiddenNeurons, weightRange);
        }

        /* --------------------------------------------------------------------------- */
        /* ОСНОВНЫЕ ФУНКЦИИ (public) */
        /* --------------------------------------------------------------------------- */

        /// <summary>
        /// Возвращает <see cref="Tau"/> (индентификатор чётности), вычисленное через входные значения <paramref name="valuesInputNeurons"/>.
        /// </summary>
        /// <param name="valuesInputNeurons">Входные значения (-1, 1) для нейронов (случайный двумерный массив 
        /// размерностью <see cref="NumberHiddenNeurons"/> на <see cref="NumberInputNeurons"/>).</param>
        /// <returns>Возвращает <see cref="Tau"/>.</returns>
        public int GetTau(int[][] valuesInputNeurons) {
            ValuesInputNeurons = valuesInputNeurons;
            // вычисление значений скрытого слоя нейронов
            SignumsNeurons = new int[NumberHiddenNeurons];
            //for (int i = 0; i < NumberHiddenNeurons; ++i) {
            //    int sumWeights = 0;
            //    for (int j = 0; j < NumberInputNeurons; ++j) {
            //        sumWeights += WeightsNeurons[i][j] * ValuesInputNeurons[i][j];
            //    }
            //    SignumsNeurons[i] = Signum(sumWeights);
            //}
            Parallel.ForEach(Partitioner.Create(0, NumberHiddenNeurons), range => {
                for (long i = range.Item1; i < range.Item2; i++) {
                    int sumWeights = 0;
                    for (int j = 0; j < NumberInputNeurons; ++j) {
                        sumWeights += WeightsNeurons[i][j] * ValuesInputNeurons[i][j];
                    }
                    SignumsNeurons[i] = Signum(sumWeights);
                }
            });
            // вычисление выходного нейрона
            return Tau = ArrayProduct(SignumsNeurons);
        }

        /// <summary>
        /// Обновление весов <see cref="WeightsNeurons"/> ДМЧ на 
        /// основе <see cref="Tau"/> по правилу <see cref="LearningRuleNeurons"/>.
        /// </summary>
        /// <param name="inputTau"><see cref="Tau"/> второго абонента.</param>
        public void UpdateWeighs(int inputTau) {
            switch (LearningRule) {
                case LearningRuleNeurons.Hebbian:
                    HebbianLearningRule(inputTau);
                    return;
                case LearningRuleNeurons.AntiHebbian:
                    AntiHebbianLearningRule(inputTau);
                    return;
                case LearningRuleNeurons.RandomWalk:
                    RandomWalkLearningRule(inputTau);
                    return;
                default:
                    return;
            }
        }

        /// <summary>
        /// Возвращает хэш-значение весов. Используется алгоритм SHA256
        /// </summary>
        /// <returns>Возвращает хэш-значение весов</returns>
        public string GetHashWeights() {
            StringBuilder stringWeights = new StringBuilder();
            for (int i = 0; i < NumberHiddenNeurons; ++i) {
                for (int j = 0; j < NumberInputNeurons; ++j) {
                    stringWeights.Append(WeightsNeurons[i][j] + " ");
                }
            }
            return GetHash(stringWeights.ToString());
        }

        /// <summary>
        /// Возвращает секретный ключ, сгенерированный из весов ДМЧ
        /// </summary>
        /// <returns>Возвращает секретный ключ, сгенерированный из весов ДМЧ</returns>
        public string GetSecretKey() {
            StringBuilder stringWeights = new StringBuilder();
            for (int i = 0; i < NumberHiddenNeurons; ++i) {
                for (int j = 0; j < NumberInputNeurons; ++j) {
                    stringWeights.Append(WeightsNeurons[i][j] + " ");
                }
            }
            return GetHash(stringWeights.ToString());
        }

#if DEBUG
        /// <summary>
        /// Возвращает веса ДМЧ
        /// </summary>
        /// <remarks>Только в режиме DEBUG.</remarks>
        public int[][] GetWeights() {
            return WeightsNeurons;
        }
#endif

        /* --------------------------------------------------------------------------- */
        /* ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ (private) */
        /* --------------------------------------------------------------------------- */

        /// <summary>
        /// Возвращает случайный массив размером <paramref name="length"/> и диапозоном 
        /// значений от -<paramref name="valueRange"/> до <paramref name="valueRange"/>
        /// </summary>
        /// <param name="length">Длина массива</param>
        /// <param name="valueRange">Диапозон значений</param>
        /// <returns>Возвращает случайный массив</returns>
        private int[] GetRandomArray(int length, int valueRange) {
            int[] array = new int[length];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) {
                for (int i = 0; i < length; ++i) {
                    byte[] randomBytes = new byte[4];
                    rng.GetBytes(randomBytes);
                    array[i] = BitConverter.ToInt32(randomBytes, 0) % valueRange;
                }
            }
            return array;
        }

        /// <summary>
        /// Генерация весового двумерного массива с весами в диапозоне 
        /// от -<paramref name="weightRang"/> до <paramref name="weightRang"/>
        /// </summary>
        /// <param name="numberInputNeurons">Количество входных нейронов.</param>
        /// <param name="numberHiddenNeurons">Количество нейронов скрытого слоя.</param>
        /// <param name="weightRange">Диапозон весов ДМЧ</param>
        /// <returns>Возвращает случайный двумерный массив весов</returns>
        private int[][] GenerationWeights(uint numberInputNeurons, uint numberHiddenNeurons, int weightRang) {
            int[][] weights = new int[numberHiddenNeurons][];
            //for (int i = 0; i < numberHiddenNeurons; ++i) {
            //    weights[i] = GetRandomArray((int)numberInputNeurons, weightRang);
            //}
            Parallel.ForEach(Partitioner.Create(0, numberHiddenNeurons), range => {
                for (long i = range.Item1; i < range.Item2; i++) {
                    weights[i] = GetRandomArray((int)numberInputNeurons, weightRang);
                }
            });
            return weights;
        }

        /// <summary>
        /// Возвращает хэш-значение строки. Кодирование происходит алгоритмом SHA256
        /// </summary>
        /// <param name="data">Строка для хэширования</param>
        /// <returns></returns>
        private string GetHash(string data) {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            return BitConverter.ToString(SHA256.Create().ComputeHash(dataBytes));
        }

        /// <summary>
        /// Сигма-функция. Возвращает 1 если значение больше 0, иначе -1.
        /// </summary>
        /// <param name="value">Значение, которое будет пропущено через сигма-функцию</param>
        /// <returns>Возвращает 1 если значение больше 0, иначе -1.</returns>
        private int Signum(int value) {
            return value > 0 ? 1 : -1;
        }

        /// <summary>
        /// Вычисляет произведение значений массива <paramref name="signums"/>.
        /// </summary>
        /// <param name="signums">Массив для перемножения его значений (выходы скрытых нейронов).</param>
        /// <returns>Возвращает результат произведения значений массива.</returns>
        private int ArrayProduct(int[] signums) {
            int resultProduct = 1;
            foreach (int signum in signums) {
                resultProduct *= signum;
            }
            return resultProduct;
        }

        /// <summary>
        /// Сравнивает числа <paramref name="value1"/> и <paramref name="value2"/>.
        /// </summary>
        /// <param name="value1">Первое число для сравнения.</param>
        /// <param name="value2">Второе число для сравнения.</param>
        /// <returns>Возвращает <see cref="int"/> результат сравнения входязих чисел.</returns>
        private int EqualsInt(int value1, int value2) {
            return Convert.ToInt32(value1.Equals(value2));
        }

        /// <summary>
        /// Ограничивает вес <paramref name="weight"/> диапозоном <see cref="WeightRange"/>.
        /// </summary>
        /// <param name="weight">Вес ДМЧ для ограничения его диапозоном <see cref="WeightRange"/></param>
        /// <returns>Возвращает ограниченный вес <paramref name="weight"/> диапозоном <see cref="WeightRange"/>.</returns>
        private int ClipWeight(int weight) {
            return weight > WeightRange || weight < -WeightRange ? 
                Signum(weight) * WeightRange : 
                weight;
        }

        /// <summary>
        /// Обновление весов ДМЧ правилом обучения Хеббиана <see cref="LearningRuleNeurons.Hebbian"/>
        /// </summary>
        /// <param name="inputTau">Результат работы ДМЧ, с которым надо синхронизироваться.</param>
        private void HebbianLearningRule(int inputTau) {
            //for (int i = 0; i < NumberHiddenNeurons; ++i) {
            //    for (int j = 0; j < NumberInputNeurons; ++j) {
            //        WeightsNeurons[i][j] += ValuesInputNeurons[i][j] * SignumsNeurons[i] * EqualsInt(SignumsNeurons[i], Tau);
            //        WeightsNeurons[i][j] = ClipWeight(WeightsNeurons[i][j]);
            //    }
            //}
            Parallel.ForEach(Partitioner.Create(0, NumberHiddenNeurons * NumberInputNeurons), range => {
                for (long index = range.Item1; index < range.Item2; ++index) {
                    int i = (int)(index / NumberInputNeurons);
                    int j = (int)(index % NumberInputNeurons);
                    WeightsNeurons[i][j] += ValuesInputNeurons[i][j] * SignumsNeurons[i] * EqualsInt(SignumsNeurons[i], Tau);
                    WeightsNeurons[i][j] = ClipWeight(WeightsNeurons[i][j]);
                }
            });
        }

        /// <summary>
        /// Обновление весов ДМЧ правилом обучения Анти-Хэббиана <see cref="LearningRuleNeurons.AntiHebbian"/>
        /// </summary>
        /// <param name="inputTau">Результат работы ДМЧ, с которым надо синхронизироваться.</param>
        private void AntiHebbianLearningRule(int inputTau) {
            for (int i = 0; i < NumberHiddenNeurons; ++i) {
                for (int j = 0; j < NumberInputNeurons; ++j) {
                    WeightsNeurons[i][j] -= ValuesInputNeurons[i][j] * SignumsNeurons[i] * EqualsInt(SignumsNeurons[i], Tau);
                    WeightsNeurons[i][j] = ClipWeight(WeightsNeurons[i][j]);
                }
            }
        }

        /// <summary>
        /// Обновление весов ДМЧ правилом обучения Случайного блуждения <see cref="LearningRuleNeurons.RandomWalk"/>
        /// </summary>
        /// <param name="inputTau">Результат работы ДМЧ, с которым надо синхронизироваться.</param>
        private void RandomWalkLearningRule(int inputTau) {
            for (int i = 0; i < NumberHiddenNeurons; ++i) {
                for (int j = 0; j < NumberInputNeurons; ++j) {
                    WeightsNeurons[i][j] += ValuesInputNeurons[i][j] * EqualsInt(SignumsNeurons[i], Tau);
                    WeightsNeurons[i][j] = ClipWeight(WeightsNeurons[i][j]);
                }
            }
        }
    }
}