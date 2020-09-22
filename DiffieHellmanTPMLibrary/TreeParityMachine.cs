using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiffieHellmanTPMLibrary {
    /// <summary>
    /// ������� �������� �������� ��� ����������� ������ ��������.
    /// </summary>
    public enum LearningRuleNeurons : byte {
        Hebbian, AntiHebbian, RandomWalk
    }

    /// <summary>
    /// ����������� ������ �������� (���). ������������ � ��������� �����-�������� ��� ��������� ���������� �����.
    /// </summary>
    public class TreeParityMachine {
        /// <summary>
        /// ���������� ������� ��������
        /// </summary>
        public readonly uint NumberInputNeurons;
        /// <summary>
        /// ���������� �������� �������� ����
        /// </summary>
        public readonly uint NumberHiddenNeurons;
        /// <summary>
        /// �������� �����
        /// </summary>
        public readonly int WeightRange;
        /// <summary>
        /// ������� ������� ������� �������� �������� <see cref="NumberHiddenNeurons"/> �� <see cref="NumberInputNeurons"/>
        /// </summary>
        private readonly int[][] WeightsNeurons;
        /// <summary>
        /// ������� �������� �����
        /// </summary>
        private readonly LearningRuleNeurons LearningRule;

        /* ������������� �������� */

        /// <summary>
        /// ��������, ����������� �� ������� �������
        /// </summary>
        private int[][] ValuesInputNeurons;
        /// <summary>
        /// �������� ����� ������� ��� ������� ��������
        /// </summary>
        private int[] SignumsNeurons;
        /// <summary>
        /// �������� ���������� ������ ��� (���)
        /// </summary>
        public int Tau { get; private set; }


        /// <param name="numberInputNeurons">���������� ������� ��������.</param>
        /// <param name="numberHiddenNeurons">���������� �������� �������� ����.</param>
        /// <param name="weightRange">�������� ����� ���</param>
        /// /// <param name="learningRule">������� �������� <see cref="LearningRuleNeurons"/> ��������.
        /// �� ��������� ����� ������� <see cref="LearningRuleNeurons.Hebbian"/>.</param>
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
        /* �������� ������� (public) */
        /* --------------------------------------------------------------------------- */

        /// <summary>
        /// ���������� <see cref="Tau"/> (�������������� ��������), ����������� ����� ������� �������� <paramref name="valuesInputNeurons"/>.
        /// </summary>
        /// <param name="valuesInputNeurons">������� �������� (-1, 1) ��� �������� (��������� ��������� ������ 
        /// ������������ <see cref="NumberHiddenNeurons"/> �� <see cref="NumberInputNeurons"/>).</param>
        /// <returns>���������� <see cref="Tau"/>.</returns>
        public int GetTau(int[][] valuesInputNeurons) {
            ValuesInputNeurons = valuesInputNeurons;
            // ���������� �������� �������� ���� ��������
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
            // ���������� ��������� �������
            return Tau = ArrayProduct(SignumsNeurons);
        }

        /// <summary>
        /// ���������� ����� <see cref="WeightsNeurons"/> ��� �� 
        /// ������ <see cref="Tau"/> �� ������� <see cref="LearningRuleNeurons"/>.
        /// </summary>
        /// <param name="inputTau"><see cref="Tau"/> ������� ��������.</param>
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
        /// ���������� ���-�������� �����. ������������ �������� SHA256
        /// </summary>
        /// <returns>���������� ���-�������� �����</returns>
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
        /// ���������� ��������� ����, ��������������� �� ����� ���
        /// </summary>
        /// <returns>���������� ��������� ����, ��������������� �� ����� ���</returns>
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
        /// ���������� ���� ���
        /// </summary>
        /// <remarks>������ � ������ DEBUG.</remarks>
        public int[][] GetWeights() {
            return WeightsNeurons;
        }
#endif

        /* --------------------------------------------------------------------------- */
        /* ��������������� ������� (private) */
        /* --------------------------------------------------------------------------- */

        /// <summary>
        /// ���������� ��������� ������ �������� <paramref name="length"/> � ���������� 
        /// �������� �� -<paramref name="valueRange"/> �� <paramref name="valueRange"/>
        /// </summary>
        /// <param name="length">����� �������</param>
        /// <param name="valueRange">�������� ��������</param>
        /// <returns>���������� ��������� ������</returns>
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
        /// ��������� �������� ���������� ������� � ������ � ��������� 
        /// �� -<paramref name="weightRang"/> �� <paramref name="weightRang"/>
        /// </summary>
        /// <param name="numberInputNeurons">���������� ������� ��������.</param>
        /// <param name="numberHiddenNeurons">���������� �������� �������� ����.</param>
        /// <param name="weightRange">�������� ����� ���</param>
        /// <returns>���������� ��������� ��������� ������ �����</returns>
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
        /// ���������� ���-�������� ������. ����������� ���������� ���������� SHA256
        /// </summary>
        /// <param name="data">������ ��� �����������</param>
        /// <returns></returns>
        private string GetHash(string data) {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            return BitConverter.ToString(SHA256.Create().ComputeHash(dataBytes));
        }

        /// <summary>
        /// �����-�������. ���������� 1 ���� �������� ������ 0, ����� -1.
        /// </summary>
        /// <param name="value">��������, ������� ����� ��������� ����� �����-�������</param>
        /// <returns>���������� 1 ���� �������� ������ 0, ����� -1.</returns>
        private int Signum(int value) {
            return value > 0 ? 1 : -1;
        }

        /// <summary>
        /// ��������� ������������ �������� ������� <paramref name="signums"/>.
        /// </summary>
        /// <param name="signums">������ ��� ������������ ��� �������� (������ ������� ��������).</param>
        /// <returns>���������� ��������� ������������ �������� �������.</returns>
        private int ArrayProduct(int[] signums) {
            int resultProduct = 1;
            foreach (int signum in signums) {
                resultProduct *= signum;
            }
            return resultProduct;
        }

        /// <summary>
        /// ���������� ����� <paramref name="value1"/> � <paramref name="value2"/>.
        /// </summary>
        /// <param name="value1">������ ����� ��� ���������.</param>
        /// <param name="value2">������ ����� ��� ���������.</param>
        /// <returns>���������� <see cref="int"/> ��������� ��������� �������� �����.</returns>
        private int EqualsInt(int value1, int value2) {
            return Convert.ToInt32(value1.Equals(value2));
        }

        /// <summary>
        /// ������������ ��� <paramref name="weight"/> ���������� <see cref="WeightRange"/>.
        /// </summary>
        /// <param name="weight">��� ��� ��� ����������� ��� ���������� <see cref="WeightRange"/></param>
        /// <returns>���������� ������������ ��� <paramref name="weight"/> ���������� <see cref="WeightRange"/>.</returns>
        private int ClipWeight(int weight) {
            return weight > WeightRange || weight < -WeightRange ? 
                Signum(weight) * WeightRange : 
                weight;
        }

        /// <summary>
        /// ���������� ����� ��� �������� �������� �������� <see cref="LearningRuleNeurons.Hebbian"/>
        /// </summary>
        /// <param name="inputTau">��������� ������ ���, � ������� ���� ������������������.</param>
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
        /// ���������� ����� ��� �������� �������� ����-�������� <see cref="LearningRuleNeurons.AntiHebbian"/>
        /// </summary>
        /// <param name="inputTau">��������� ������ ���, � ������� ���� ������������������.</param>
        private void AntiHebbianLearningRule(int inputTau) {
            for (int i = 0; i < NumberHiddenNeurons; ++i) {
                for (int j = 0; j < NumberInputNeurons; ++j) {
                    WeightsNeurons[i][j] -= ValuesInputNeurons[i][j] * SignumsNeurons[i] * EqualsInt(SignumsNeurons[i], Tau);
                    WeightsNeurons[i][j] = ClipWeight(WeightsNeurons[i][j]);
                }
            }
        }

        /// <summary>
        /// ���������� ����� ��� �������� �������� ���������� ��������� <see cref="LearningRuleNeurons.RandomWalk"/>
        /// </summary>
        /// <param name="inputTau">��������� ������ ���, � ������� ���� ������������������.</param>
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