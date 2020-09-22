namespace DiffieHellmanTPM_winForm {
    partial class FormDiffieHellmanTPM {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numHiddenNeuronsTextBox = new System.Windows.Forms.TextBox();
            this.numInputNeuronsTextBox = new System.Windows.Forms.TextBox();
            this.weightRangeTextBox = new System.Windows.Forms.TextBox();
            this.runProtocolButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.recipientTextBox = new System.Windows.Forms.TextBox();
            this.ParamsTPMGroupBox = new System.Windows.Forms.GroupBox();
            this.HebbianRadioButton = new System.Windows.Forms.RadioButton();
            this.AntiHebbifnRadioButton = new System.Windows.Forms.RadioButton();
            this.RandomWalkRadioButton = new System.Windows.Forms.RadioButton();
            this.LearningRuleGroupBox = new System.Windows.Forms.GroupBox();
            this.userToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.ParamsTPMGroupBox.SuspendLayout();
            this.LearningRuleGroupBox.SuspendLayout();
            this.userToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество скрытых нейронов:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Количество входных нейронов:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(3, 73);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Максимальный вес:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.numHiddenNeuronsTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.numInputNeuronsTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.weightRangeTextBox, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(297, 100);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // numHiddenNeuronsTextBox
            // 
            this.numHiddenNeuronsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numHiddenNeuronsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.numHiddenNeuronsTextBox.Location = new System.Drawing.Point(225, 5);
            this.numHiddenNeuronsTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.numHiddenNeuronsTextBox.Name = "numHiddenNeuronsTextBox";
            this.numHiddenNeuronsTextBox.Size = new System.Drawing.Size(55, 23);
            this.numHiddenNeuronsTextBox.TabIndex = 3;
            this.numHiddenNeuronsTextBox.Text = "3";
            this.numHiddenNeuronsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numHiddenNeuronsTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ParamsTPMTextBox_KeyPress);
            this.numHiddenNeuronsTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.numHiddenNeuronsTextBox_Validating);
            this.numHiddenNeuronsTextBox.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // numInputNeuronsTextBox
            // 
            this.numInputNeuronsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numInputNeuronsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.numInputNeuronsTextBox.Location = new System.Drawing.Point(225, 38);
            this.numInputNeuronsTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.numInputNeuronsTextBox.Name = "numInputNeuronsTextBox";
            this.numInputNeuronsTextBox.Size = new System.Drawing.Size(55, 23);
            this.numInputNeuronsTextBox.TabIndex = 4;
            this.numInputNeuronsTextBox.Text = "100";
            this.numInputNeuronsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numInputNeuronsTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ParamsTPMTextBox_KeyPress);
            this.numInputNeuronsTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.numInputNeuronsTextBox_Validating);
            this.numInputNeuronsTextBox.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // weightRangeTextBox
            // 
            this.weightRangeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.weightRangeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.weightRangeTextBox.Location = new System.Drawing.Point(225, 71);
            this.weightRangeTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.weightRangeTextBox.Name = "weightRangeTextBox";
            this.weightRangeTextBox.Size = new System.Drawing.Size(55, 23);
            this.weightRangeTextBox.TabIndex = 5;
            this.weightRangeTextBox.Text = "3";
            this.weightRangeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.weightRangeTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ParamsTPMTextBox_KeyPress);
            this.weightRangeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.weightRangeTextBox_Validating);
            this.weightRangeTextBox.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // runProtocolButton
            // 
            this.runProtocolButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.runProtocolButton.Location = new System.Drawing.Point(120, 326);
            this.runProtocolButton.Name = "runProtocolButton";
            this.runProtocolButton.Size = new System.Drawing.Size(88, 30);
            this.runProtocolButton.TabIndex = 2;
            this.runProtocolButton.Text = "Начать";
            this.runProtocolButton.UseVisualStyleBackColor = true;
            this.runProtocolButton.Click += new System.EventHandler(this.RunProtocolButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label6.Location = new System.Drawing.Point(12, 285);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Имя получателя:";
            // 
            // recipientTextBox
            // 
            this.recipientTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.recipientTextBox.Location = new System.Drawing.Point(137, 282);
            this.recipientTextBox.Name = "recipientTextBox";
            this.recipientTextBox.Size = new System.Drawing.Size(184, 23);
            this.recipientTextBox.TabIndex = 1;
            this.recipientTextBox.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // ParamsTPMGroupBox
            // 
            this.ParamsTPMGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.ParamsTPMGroupBox.Location = new System.Drawing.Point(12, 31);
            this.ParamsTPMGroupBox.Name = "ParamsTPMGroupBox";
            this.ParamsTPMGroupBox.Size = new System.Drawing.Size(309, 131);
            this.ParamsTPMGroupBox.TabIndex = 13;
            this.ParamsTPMGroupBox.TabStop = false;
            this.ParamsTPMGroupBox.Text = "Параметры ДМЧ";
            // 
            // HebbianRadioButton
            // 
            this.HebbianRadioButton.AutoSize = true;
            this.HebbianRadioButton.Checked = true;
            this.HebbianRadioButton.Location = new System.Drawing.Point(6, 19);
            this.HebbianRadioButton.Name = "HebbianRadioButton";
            this.HebbianRadioButton.Size = new System.Drawing.Size(68, 17);
            this.HebbianRadioButton.TabIndex = 7;
            this.HebbianRadioButton.TabStop = true;
            this.HebbianRadioButton.Text = "Хеббиан";
            this.HebbianRadioButton.UseVisualStyleBackColor = true;
            // 
            // AntiHebbifnRadioButton
            // 
            this.AntiHebbifnRadioButton.AutoSize = true;
            this.AntiHebbifnRadioButton.Location = new System.Drawing.Point(6, 44);
            this.AntiHebbifnRadioButton.Name = "AntiHebbifnRadioButton";
            this.AntiHebbifnRadioButton.Size = new System.Drawing.Size(95, 17);
            this.AntiHebbifnRadioButton.TabIndex = 15;
            this.AntiHebbifnRadioButton.Text = "Анти-Хеббиан";
            this.AntiHebbifnRadioButton.UseVisualStyleBackColor = true;
            // 
            // RandomWalkRadioButton
            // 
            this.RandomWalkRadioButton.AutoSize = true;
            this.RandomWalkRadioButton.Location = new System.Drawing.Point(6, 69);
            this.RandomWalkRadioButton.Name = "RandomWalkRadioButton";
            this.RandomWalkRadioButton.Size = new System.Drawing.Size(136, 17);
            this.RandomWalkRadioButton.TabIndex = 16;
            this.RandomWalkRadioButton.Text = "Случайное блуждение";
            this.RandomWalkRadioButton.UseVisualStyleBackColor = true;
            // 
            // LearningRuleGroupBox
            // 
            this.LearningRuleGroupBox.Controls.Add(this.AntiHebbifnRadioButton);
            this.LearningRuleGroupBox.Controls.Add(this.RandomWalkRadioButton);
            this.LearningRuleGroupBox.Controls.Add(this.HebbianRadioButton);
            this.LearningRuleGroupBox.Location = new System.Drawing.Point(12, 170);
            this.LearningRuleGroupBox.Name = "LearningRuleGroupBox";
            this.LearningRuleGroupBox.Size = new System.Drawing.Size(309, 92);
            this.LearningRuleGroupBox.TabIndex = 17;
            this.LearningRuleGroupBox.TabStop = false;
            this.LearningRuleGroupBox.Text = "Правило обучение ДМЧ";
            // 
            // userToolStrip
            // 
            this.userToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1});
            this.userToolStrip.Location = new System.Drawing.Point(0, 0);
            this.userToolStrip.Name = "userToolStrip";
            this.userToolStrip.Size = new System.Drawing.Size(339, 25);
            this.userToolStrip.TabIndex = 18;
            this.userToolStrip.Text = "Настройки";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(97, 22);
            this.toolStripSplitButton1.Text = "Пользователь";
            // 
            // changeToolStripMenuItem
            // 
            this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
            this.changeToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.changeToolStripMenuItem.Text = "Изменить";
            this.changeToolStripMenuItem.Click += new System.EventHandler(this.ChangeToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.deleteToolStripMenuItem.Text = "Удалить";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.exitToolStripMenuItem.Text = "Выйти";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // FormDiffieHellmanTPM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(339, 368);
            this.Controls.Add(this.userToolStrip);
            this.Controls.Add(this.LearningRuleGroupBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.recipientTextBox);
            this.Controls.Add(this.ParamsTPMGroupBox);
            this.Controls.Add(this.runProtocolButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.Name = "FormDiffieHellmanTPM";
            this.Text = "Протокол Диффи-Хеллмана через ДМЧ";
            this.Load += new System.EventHandler(this.FormDiffieHellmanTPM_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ParamsTPMGroupBox.ResumeLayout(false);
            this.LearningRuleGroupBox.ResumeLayout(false);
            this.LearningRuleGroupBox.PerformLayout();
            this.userToolStrip.ResumeLayout(false);
            this.userToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox numHiddenNeuronsTextBox;
        private System.Windows.Forms.TextBox numInputNeuronsTextBox;
        private System.Windows.Forms.TextBox weightRangeTextBox;
        private System.Windows.Forms.Button runProtocolButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox recipientTextBox;
        private System.Windows.Forms.GroupBox ParamsTPMGroupBox;
        private System.Windows.Forms.RadioButton HebbianRadioButton;
        private System.Windows.Forms.RadioButton AntiHebbifnRadioButton;
        private System.Windows.Forms.RadioButton RandomWalkRadioButton;
        private System.Windows.Forms.GroupBox LearningRuleGroupBox;
        private System.Windows.Forms.ToolStrip userToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem changeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}

