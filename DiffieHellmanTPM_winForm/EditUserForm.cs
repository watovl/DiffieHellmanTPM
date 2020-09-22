using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiffieHellmanTPM_winForm {
    public partial class EditUserForm : Form {
        private readonly HttpClient _httpClient;
        private readonly string UserName;
        public string NewUserName { get; private set; }
        public string NewPassword { get; private set; }
        /// <summary>
        /// Флаг успешности изменения пользователя
        /// </summary>
        public bool IsEdit { get; private set; } = false;


        public EditUserForm(string userName) {
            InitializeComponent();
            // создание http соединения
            _httpClient = new HttpClient {
                BaseAddress = new Uri("https://localhost:44355/api/users/name/")
            };

            UserName = userName;
        }

        /// <summary>
        /// Изменение пользователя
        /// </summary>
        private async void EditButton_Click(object sender, EventArgs e) {
            EditButton.CheckState = CheckState.Checked;
            // проверка параметров
            if (!(ValidateNewUserName() & ValidateNewPassword())) {
                EditButton.CheckState = CheckState.Unchecked;
                return;
            }

            NewUserName = newUserNameTextBox.Text;
            NewPassword = newPasswordTextBox.Text;

            try {
                string content = JsonConvert.SerializeObject(new { name = NewUserName, password = NewPassword });

                HttpResponseMessage response = await _httpClient.PutAsync($"?userName={UserName}",
                    new StringContent(content, Encoding.UTF8, "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict) {
                    EditButton.CheckState = CheckState.Unchecked;
                    errorProvider.SetError(newUserNameTextBox, "Пользователь с таким именем уже зарегистрирован");
                    return;
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex) {
                EditButton.CheckState = CheckState.Unchecked;
                MessageBox.Show(ex.Message, "Ошибка запроса", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // успешное изменение пользователя
            IsEdit = true;
            Close();
        }

        /// <summary>
        /// Обработка нажатие на клавишу
        /// </summary>
        private void EditButton_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                EditButton_Click(sender, EventArgs.Empty);
            }
        }

        private void newUserNameTextBox_Validating(object sender, CancelEventArgs e) {
            e.Cancel = !ValidateNewUserName();
        }

        private void newPasswordTextBox_Validating(object sender, CancelEventArgs e) {
            e.Cancel = !ValidateNewPassword();
        }

        /// <summary>
        /// Проверка блока <see cref="TextBox"/> имени пользователя
        /// </summary>
        private bool ValidateNewUserName() {
            if (string.IsNullOrWhiteSpace(newUserNameTextBox.Text)) {
                errorProvider.SetError(newUserNameTextBox, "Необходимо ввести значение");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверка блока <see cref="TextBox"/> пароля пользователя
        /// </summary>
        private bool ValidateNewPassword() {
            if (newPasswordTextBox.Text.Length < 5) {
                errorProvider.SetError(newPasswordTextBox, "Пароль должен состоять из более 4 символов");
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
