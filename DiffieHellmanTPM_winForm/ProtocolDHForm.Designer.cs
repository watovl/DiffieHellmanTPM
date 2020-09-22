namespace DiffieHellmanTPM_winForm {
    partial class ProtocolDHForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.usersProtocolLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.secretKeyTextBox = new System.Windows.Forms.TextBox();
            this.protocolTimer = new System.Windows.Forms.Timer(this.components);
            this.headingParamsTPMLabel = new System.Windows.Forms.Label();
            this.headingRuntimeLabel = new System.Windows.Forms.Label();
            this.paramsTPMLabel = new System.Windows.Forms.Label();
            this.runtimeLabel = new System.Windows.Forms.Label();
            this.headingStatusLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // usersProtocolLabel
            // 
            this.usersProtocolLabel.AutoSize = true;
            this.usersProtocolLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.usersProtocolLabel.Location = new System.Drawing.Point(12, 9);
            this.usersProtocolLabel.Name = "usersProtocolLabel";
            this.usersProtocolLabel.Size = new System.Drawing.Size(136, 13);
            this.usersProtocolLabel.TabIndex = 0;
            this.usersProtocolLabel.Text = "Пользователи протокола";
            this.usersProtocolLabel.SizeChanged += new System.EventHandler(this.usersProtocolLabel_SizeChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Секретный ключ:";
            // 
            // secretKeyTextBox
            // 
            this.secretKeyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.secretKeyTextBox.Enabled = false;
            this.secretKeyTextBox.Location = new System.Drawing.Point(115, 139);
            this.secretKeyTextBox.Name = "secretKeyTextBox";
            this.secretKeyTextBox.Size = new System.Drawing.Size(178, 20);
            this.secretKeyTextBox.TabIndex = 2;
            // 
            // protocolTimer
            // 
            this.protocolTimer.Tick += new System.EventHandler(this.protocolTimer_Tick);
            // 
            // headingParamsTPMLabel
            // 
            this.headingParamsTPMLabel.AutoSize = true;
            this.headingParamsTPMLabel.Location = new System.Drawing.Point(12, 42);
            this.headingParamsTPMLabel.Name = "headingParamsTPMLabel";
            this.headingParamsTPMLabel.Size = new System.Drawing.Size(98, 13);
            this.headingParamsTPMLabel.TabIndex = 3;
            this.headingParamsTPMLabel.Text = "Параметры ДМЧ:";
            // 
            // headingRuntimeLabel
            // 
            this.headingRuntimeLabel.AutoSize = true;
            this.headingRuntimeLabel.Location = new System.Drawing.Point(12, 108);
            this.headingRuntimeLabel.Name = "headingRuntimeLabel";
            this.headingRuntimeLabel.Size = new System.Drawing.Size(108, 13);
            this.headingRuntimeLabel.TabIndex = 4;
            this.headingRuntimeLabel.Text = "Время выполнения:";
            // 
            // paramsTPMLabel
            // 
            this.paramsTPMLabel.AutoSize = true;
            this.paramsTPMLabel.Location = new System.Drawing.Point(118, 43);
            this.paramsTPMLabel.Name = "paramsTPMLabel";
            this.paramsTPMLabel.Size = new System.Drawing.Size(43, 13);
            this.paramsTPMLabel.TabIndex = 5;
            this.paramsTPMLabel.Text = "(0, 0, 0)";
            // 
            // runtimeLabel
            // 
            this.runtimeLabel.AutoSize = true;
            this.runtimeLabel.Location = new System.Drawing.Point(126, 108);
            this.runtimeLabel.Name = "runtimeLabel";
            this.runtimeLabel.Size = new System.Drawing.Size(13, 13);
            this.runtimeLabel.TabIndex = 6;
            this.runtimeLabel.Text = "0";
            // 
            // headingStatusLabel
            // 
            this.headingStatusLabel.AutoSize = true;
            this.headingStatusLabel.Location = new System.Drawing.Point(12, 75);
            this.headingStatusLabel.Name = "headingStatusLabel";
            this.headingStatusLabel.Size = new System.Drawing.Size(44, 13);
            this.headingStatusLabel.TabIndex = 7;
            this.headingStatusLabel.Text = "Статус:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(55, 75);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(150, 13);
            this.statusLabel.TabIndex = 8;
            this.statusLabel.Text = "ожидание второго абонента";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ProtocolDHForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 173);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.headingStatusLabel);
            this.Controls.Add(this.runtimeLabel);
            this.Controls.Add(this.paramsTPMLabel);
            this.Controls.Add(this.headingRuntimeLabel);
            this.Controls.Add(this.headingParamsTPMLabel);
            this.Controls.Add(this.secretKeyTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.usersProtocolLabel);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(250, 212);
            this.Name = "ProtocolDHForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выполнение протокола";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProtocolDHForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label usersProtocolLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox secretKeyTextBox;
        private System.Windows.Forms.Timer protocolTimer;
        private System.Windows.Forms.Label headingParamsTPMLabel;
        private System.Windows.Forms.Label headingRuntimeLabel;
        private System.Windows.Forms.Label paramsTPMLabel;
        private System.Windows.Forms.Label runtimeLabel;
        private System.Windows.Forms.Label headingStatusLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}