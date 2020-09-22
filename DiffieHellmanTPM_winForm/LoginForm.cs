using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace DiffieHellmanTPM_winForm {
    public partial class LoginForm : Form {
        private readonly HttpClient _httpClient;
        /// <summary>
        /// Флаг успешности авторизации пользователя
        /// </summary>
        public bool IsAuthenticated { get; private set; } = false;
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public LoginForm() {
            InitializeComponent();
            // создание http соединения
            _httpClient = new HttpClient {
                BaseAddress = new Uri("https://localhost:44355/api/users/")
            };
        }

        /// <summary>
        /// Вход в систему при нажатии кнопки
        /// </summary>
        private async void EntryButton_Click(object sender, EventArgs e) {
            EntryButton.CheckState = CheckState.Checked;
            // проверка параметров
            if (!(ValidateUserName() & ValidatePassword())) {
                EntryButton.CheckState = CheckState.Unchecked;
                return;
            }
            UserName = userNameTextBox.Text;
            Password = passwordTextBox.Text;

            string result = string.Empty;
            try {
                string content = JsonConvert.SerializeObject(new { name = UserName, password = Password });
                HttpResponseMessage response = await _httpClient.PostAsync("existence",
                    new StringContent(content, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex) {
                EntryButton.CheckState = CheckState.Unchecked;
                MessageBox.Show(ex.Message, "Ошибка запроса", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // неверный пользователь
            if (result != "true") {
                EntryButton.CheckState = CheckState.Unchecked;
                MessageBox.Show("Данный пользователь не зарегистрирован в системе",
                   "Неверный пользователь",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                return;
            }
            // успешная авторизация
            IsAuthenticated = true;
            Close();
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        private async void RegisterButton_Click(object sender, EventArgs e) {
            RegisterButton.CheckState = CheckState.Checked;
            // проверка параметров
            if (!(ValidateUserName() & ValidatePassword())) {
                RegisterButton.CheckState = CheckState.Unchecked;
                return;
            }

            UserName = userNameTextBox.Text;
            Password = passwordTextBox.Text;

            try {
                string content = JsonConvert.SerializeObject(new { name = UserName, password = Password });
                HttpResponseMessage response = await _httpClient.PostAsync("",
                    new StringContent(content, Encoding.UTF8, "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict) {
                    RegisterButton.CheckState = CheckState.Unchecked;
                    errorProvider.SetError(userNameTextBox, "Пользователь с таким именем уже зарегистрирован");
                    return;
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex) {
                RegisterButton.CheckState = CheckState.Unchecked;
                MessageBox.Show(ex.Message, "Ошибка запроса", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // успешная авторизация
            IsAuthenticated = true;
            Close();
        }

        /// <summary>
        /// Обработка нажатие на клавишу
        /// </summary>
        private void Button_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                EntryButton_Click(sender, EventArgs.Empty);
            }
        }

        private void userNameTextBox_Validating(object sender, CancelEventArgs e) {
            e.Cancel = !ValidateUserName();
        }

        private void passwordTextBox_Validating(object sender, CancelEventArgs e) {
            e.Cancel = !ValidatePassword();
        }

        /// <summary>
        /// Проверка блока <see cref="TextBox"/> имени пользователя
        /// </summary>
        private bool ValidateUserName() {
            if (string.IsNullOrWhiteSpace(userNameTextBox.Text)) {
                errorProvider.SetError(userNameTextBox, "Необходимо ввести значение");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверка блока <see cref="TextBox"/> пароля пользователя
        /// </summary>
        private bool ValidatePassword() {
            if (passwordTextBox.Text.Length < 5) {
                errorProvider.SetError(passwordTextBox, "Пароль должен состоять из более 4 символов");
                return false;
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
