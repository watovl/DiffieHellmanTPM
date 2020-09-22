using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiffieHellmanTPMLibrary;

namespace DiffieHellmanTPM_winForm {
    public partial class ProtocolDHForm : Form {
        private GraphForm _graphForm;
        private readonly string UserName;
        private readonly string Password;
        private readonly string Recipient;
        DiffieHellman Protocol;
        ProtocolOption _protocolOption;
        private DateTime BeginTime;

        public ProtocolDHForm(string userName, string password, string recipient, ProtocolOption protocolOption) {
            InitializeComponent();

            UserName = userName;
            Password = password;
            Recipient = recipient;
            _protocolOption = protocolOption;

            usersProtocolLabel.Text = $"Процесс выполнения протокола между пользователями: {UserName} и {Recipient}";
        }

        /// <summary>
        /// Возвращает новый объект Диффи-Хеллмана нужного подкласса
        /// </summary>
        /// <param name="protocolOption">Опция выбора типа Диффи-Хеллмана</param>
        private DiffieHellman GetNewDiffieHellman(ProtocolOption protocolOption) {
            switch (protocolOption) {
                case ProtocolOption.Default:
                    return new DiffieHellman();
                case ProtocolOption.Graph:
                    return new DiffieHellmanWithSyncPoints();
                case ProtocolOption.Eva:
                    return new DiffieHellmanWithInterceptor();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Запускает протокол Диффи-Хеллмана
        /// </summary>
        public async Task RunProtocolAsync(uint numInputNeurons, uint numHiddenNeurons, int weightRange, LearningRuleNeurons rule) {
            // установка значений TextLabel
            paramsTPMLabel.Text = $"({numHiddenNeurons}, {numInputNeurons}, {weightRange})";
            runtimeLabel.Text = TimeSpan.Zero.ToString();

            Protocol = GetNewDiffieHellman(_protocolOption);
            SetHandleProtocol();

            try {
                await Protocol.ConnectAsync(UserName, Password, Recipient);
                await Protocol.RunProtocolAsync(numInputNeurons, numHiddenNeurons, weightRange, rule);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message,
                    "Ошибка выполнения протокола!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            return;
        }

        /// <summary>
        /// Установка обработчиков событий для протокола
        /// </summary>
        private void SetHandleProtocol() {
            Protocol.DifferentParamsEvent += (s, e) => {
                statusLabel.Text = "ошибка синхронизации: у абонентов разные параметры ДМЧ";
                errorProvider.SetError(paramsTPMLabel, "У абонентов разные параметры ДМЧ");
            };

            Protocol.BeginningSync += (s, e) => {
                BeginTime = DateTime.Now;
                protocolTimer.Start();
                statusLabel.Text = "вычисление секретного ключа";
            };

            Protocol.ClosedConnection += Protocol_ClosedConnection;

            Protocol.DisconnectRecipient += (s, e) => {
                Protocol.ClosedConnection -= Protocol_ClosedConnection;
                protocolTimer.Stop();
                statusLabel.Text = "ошибка: второй абонент отключился";
                //errorProvider.SetError(statusLabel, "Второй абонент отключился");
            };

            Protocol.FinishedProtocol += (s, e) => {
                Protocol.ClosedConnection -= Protocol_ClosedConnection;
                protocolTimer.Stop();
                statusLabel.Text = "секретный ключ сгенерирован";
            };

            Protocol.GeneratedSecretKey += (s, secretKey) => {
                secretKeyTextBox.Enabled = true;
                secretKeyTextBox.Text = secretKey;
            };

            if (Protocol is DiffieHellmanWithSyncPoints protocolWithSyncPoints) {
                protocolWithSyncPoints.SyncPointsEvent += OnDrawGraph;
            }

            if (Protocol is DiffieHellmanWithInterceptor protocolWithInterceptor) {
                protocolWithInterceptor.InterceptorSyncPointsEvent += OnDrawGraphInterceptor;

                protocolWithInterceptor.SuccessInterceptor += (s, e) => {
                    protocolTimer.Stop();
                    statusLabel.Text = "неудача: перехватчик синхронизировался с одним из абонентов";
                    errorProvider.SetError(statusLabel, "Перехватчик синхронизировался с одним из абонентов");
                };
            }
        }

        private void Protocol_ClosedConnection(object sender, EventArgs e) {
            protocolTimer.Stop();
            statusLabel.Text = "ошибка: соединение с сервером закрыто";
            //errorProvider.SetError(statusLabel, "Соединение с сервером закрыто");
        }

        /// <summary>
        /// Вызов окна с графиком синхронизации
        /// </summary>
        /// <param name="syncPoints">Список точек синхронизации весов.</param>
        private void OnDrawGraph(object sender, double[] syncPoints) {
            if (syncPoints.Length == 0) {
                errorProvider.SetError(statusLabel, "Второй абонент не передавал веса");
                return;
            }
            if (_graphForm == null) {
                _graphForm = new GraphForm();
                _graphForm.Show();
            }
            _graphForm.DrawSyncGraph(syncPoints);
        }

        private void OnDrawGraphInterceptor(object sender, Tuple<double[], double[]> tuple) {
            if (tuple.Item1.Length != 0 && tuple.Item2.Length != 0) {
                _graphForm = new GraphForm();
                _graphForm.Show();
                _graphForm.DrawSyncInterceptorGraph(tuple.Item1, tuple.Item2);
            }
        }

        /// <summary>
        /// Изменение времени выполнения протокола
        /// </summary>
        private void protocolTimer_Tick(object sender, EventArgs e) {
            runtimeLabel.Text = DateTime.Now.Subtract(BeginTime).ToString();
        }

        /// <summary>
        /// Событие изменения размера <see cref="usersProtocolLabel"/>
        /// </summary>
        private void usersProtocolLabel_SizeChanged(object sender, EventArgs e) {
            // изменение размера окна
            this.Width = usersProtocolLabel.Width + 36;
        }

        private async void ProtocolDHForm_FormClosing(object sender, FormClosingEventArgs e) {
            await Protocol.DisposeAsync();
        }
    }
}
