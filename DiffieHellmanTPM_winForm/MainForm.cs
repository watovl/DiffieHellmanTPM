using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using DiffieHellmanTPMLibrary;


namespace DiffieHellmanTPM_winForm {
    public partial class FormDiffieHellmanTPM : Form {
        private LoginForm _loginForm;
        private ProtocolDHForm _protocolDHForm;
        private EditUserForm _editUserFrom;
        private readonly HttpClient _httpClient;
        private readonly ProtocolOption _protocolOption;
        private string UserName;
        private string Password;

        public FormDiffieHellmanTPM(ProtocolOption protocolOption) {
            InitializeComponent();
            _protocolOption = protocolOption;
            // создание http соединения
            _httpClient = new HttpClient {
                BaseAddress = new Uri("https://localhost:44355/api/users/name/")
            };
        }

        private void FormDiffieHellmanTPM_Load(object sender, EventArgs e) {
            Hide();
            _loginForm = new LoginForm();
            _loginForm.FormClosed += OnLoginFormClosed;
            _loginForm.ShowDialog();
        }

        /// <summary>
        /// Обработка события закрытия формы входа
        /// </summary>
        private void OnLoginFormClosed(object sender, FormClosedEventArgs e) {
            // авторизация прошла успешно
            if (_loginForm.IsAuthenticated) {
                UserName = _loginForm.UserName;
                Password = _loginForm.Password;
                toolStripSplitButton1.Text = UserName;
                Show();
            }
            else {
                Close();
            }
        }

        /// <summary>
        /// Возвращает правило обучение выбранное в <see cref="RadioButton"/>
        /// </summary>
        private LearningRuleNeurons? GetLearningRule() {
            LearningRuleNeurons? rule = null;
            Dictionary<string, LearningRuleNeurons> DictionaryRules = new Dictionary<string, LearningRuleNeurons> {
                { "Хеббиан" , LearningRuleNeurons.Hebbian },
                { "Анти-Хеббиан",  LearningRuleNeurons.AntiHebbian },
                { "Случайное блуждение", LearningRuleNeurons.RandomWalk }
            };
            foreach (Control control in LearningRuleGroupBox.Controls) {
                if (control is RadioButton) {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked) {
                        rule = DictionaryRules[radio.Text];
                    }
                }
            }
            return rule;
        }

        /// <summary>
        /// Выполнение протокола Диффи-Хеллмана ДМЧ
        /// </summary>
        private async void RunProtocolButton_Click(object sender, EventArgs ev) {
            // проверка параметров
            if (!(ValidateParamsTPMTextBox(numInputNeuronsTextBox, 200) &
                ValidateParamsTPMTextBox(numHiddenNeuronsTextBox, 200) &
                ValidateParamsTPMTextBox(weightRangeTextBox, 20) &&
                await ValidateRecipient())) 
            {
                return;
            }

            // получение правила обучения
            LearningRuleNeurons? rule = GetLearningRule();
            if (rule == null) {
                errorProvider.SetError(LearningRuleGroupBox, "Не выбрано правило обучения");
                return;
            }
            
            // выполнение генерации секретного ключа через протокол Диффи-Хеллмана
            try {
                _protocolDHForm = new ProtocolDHForm(UserName, Password, recipientTextBox.Text, _protocolOption);
                _protocolDHForm.Show();
                await _protocolDHForm.RunProtocolAsync(uint.Parse(numInputNeuronsTextBox.Text),
                    uint.Parse(numHiddenNeuronsTextBox.Text),
                    int.Parse(weightRangeTextBox.Text),
                    rule.Value);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Ошибка выполнения протокола!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;
        }

        /// <summary>
        /// Изменение данного пользователя
        /// </summary>
        private void ChangeToolStripMenuItem_Click(object sender, EventArgs e) {
            _editUserFrom = new EditUserForm(UserName);
            _editUserFrom.FormClosed += OnEditUserFormClosed;
            _editUserFrom.ShowDialog();
        }

        /// <summary>
        /// Обработка закрытия окна с изменением пользователя
        /// </summary>
        private void OnEditUserFormClosed(object sender, FormClosedEventArgs e) {
            // пользователь был изменён
            if (_editUserFrom.IsEdit) {
                UserName = _editUserFrom.NewUserName;
                Password = _editUserFrom.NewPassword;
                toolStripSplitButton1.Text = UserName;
            }
        }

        /// <summary>
        /// Удаление данного пользователя
        /// </summary>
        private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e) {
            var dialogResult = MessageBox.Show("Вы точно ходите удалить данного пользователя?", 
                "Удаление пользователя", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.No) {
                return;
            }

            try {
                HttpResponseMessage response = await _httpClient.DeleteAsync(UserName);
                response.EnsureSuccessStatusCode();
                // В случаи успешного удаления выходим в форму входа
                ExitToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Ошибка запроса", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Выйти в меню входа
        /// </summary>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
            Hide();
            _loginForm = new LoginForm();
            _loginForm.FormClosed += OnLoginFormClosed;
            _loginForm.ShowDialog();
        }

        /// <summary>
        /// Обработка вводимых значений в <see cref="TextBox"/> параметров ДМЧ на принадлежность к числам.
        /// </summary>
        private void ParamsTPMTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void numHiddenNeuronsTextBox_Validating(object sender, CancelEventArgs e) {
            //errorProvider.SetError(numHiddenNeuronsTextBox, "");
            e.Cancel = !ValidateParamsTPMTextBox(numHiddenNeuronsTextBox, 200);
        }

        private void numInputNeuronsTextBox_Validating(object sender, CancelEventArgs e) {
            //errorProvider.SetError(numInputNeuronsTextBox, "");
            e.Cancel = !ValidateParamsTPMTextBox(numInputNeuronsTextBox, 200);
        }

        private void weightRangeTextBox_Validating(object sender, CancelEventArgs e) {
            //errorProvider.SetError(weightRangeTextBox, "");
            e.Cancel = !ValidateParamsTPMTextBox(weightRangeTextBox, 20);
        }

        /// <summary>
        /// Проверка вводимых значений в <see cref="TextBox"/> параметров ДМЧ
        /// </summary>
        private bool ValidateParamsTPMTextBox(TextBox textBox, int limitParam) {
            if (string.IsNullOrWhiteSpace(textBox.Text)) {
                errorProvider.SetError(textBox, "Необходимо ввести значение");
                return false;
            }
            if (int.Parse(textBox.Text) > limitParam) {
                errorProvider.SetError(textBox, "Слишком большое значение");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверка существования получателя
        /// </summary>
        private async Task<bool> ValidateRecipient() {
            if (string.IsNullOrWhiteSpace(recipientTextBox.Text)) {
                errorProvider.SetError(recipientTextBox, "Необходимо ввести значение");
                return false;
            }
            try {
                HttpResponseMessage response = await _httpClient.GetAsync(recipientTextBox.Text);
                if (response.StatusCode == HttpStatusCode.NotFound) {
                    errorProvider.SetError(recipientTextBox, "Данный пользователь не существует");
                    return false;
                }
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Ошибка запроса", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }

        /// <summary>
        /// Очистка ошибки при успешной проверки параметров ДМЧ
        /// </summary>
        private void TextBox_Validated(object sender, EventArgs e) {
            if (sender is TextBox textBox) {
                errorProvider.SetError(textBox, "");
            }
        }
    }
}
