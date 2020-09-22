using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiffieHellmanTPMLibrary;

namespace DHTPM_wenForm {
    public partial class Form1 : Form {
        private DiffieHellman Protocol;

        public Form1() {
            InitializeComponent();
        }

        private async void RunProtocolButton_Click(object sender, EventArgs ev) {
            // проверка ввода параметров ДМЧ
            if (!(uint.TryParse(numInputNeuronsTextBox.Text, out uint numInputNeurons) &&
                uint.TryParse(numHiddenNeuronsTextBox.Text, out uint numHiddenNeurons) &&
                int.TryParse(weightRangeTextBox.Text, out int weightRange))) {
                MessageBox.Show("Параметры древовидной машины четности введены не корректно",
                    "Ошибка ввода!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            // проверка ввода параметров соединения
            if (string.IsNullOrWhiteSpace(userNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordTextBox.Text) ||
                string.IsNullOrWhiteSpace(recipientTextBox.Text)) {
                MessageBox.Show("Параметры соединения введены не корректно",
                    "Ошибка ввода!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            // выполнение генерации секретного ключа через протокол Диффи-Хеллмана
            try {
                Protocol = new DiffieHellman();
                // Действия при событиях
                Protocol.DifferentParamsEvent += (s, e) =>
                    MessageBox.Show(this, "Параметры древовидной машины четности (ДМЧ) различаются от параметров второго абонента",
                        "Ошибка синхронизации!",
                         MessageBoxButtons.OK,
                        MessageBoxIcon.Stop
                    );

                Protocol.GeneratedSecretKey += (s, secretKey) =>
                    MessageBox.Show($"Сгенерирован секретный ключ: {secretKey}",
                        "Секретный ключ",
                         MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                await Task.Run(() => Protocol.ConnectAsync(userNameTextBox.Text, passwordTextBox.Text, recipientTextBox.Text));
                await Task.Run(() => Protocol.RunProtocol(numInputNeurons, numHiddenNeurons, weightRange));
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message,
                    "Ошибка выполнения протокола!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            return;
        }
    }
}
