namespace DiffieHellmanTPM_winForm {
    partial class GraphForm {
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.SyncChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.SyncChart)).BeginInit();
            this.SuspendLayout();
            // 
            // SyncChart
            // 
            chartArea1.AxisX.Title = "Эпохи";
            chartArea1.AxisY.Title = "Проценты %";
            chartArea1.Name = "ChartArea1";
            this.SyncChart.ChartAreas.Add(chartArea1);
            this.SyncChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Enabled = false;
            legend1.MaximumAutoSize = 10F;
            legend1.Name = "LegendSyncWeights";
            legend2.Alignment = System.Drawing.StringAlignment.Center;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend2.Enabled = false;
            legend2.MaximumAutoSize = 10F;
            legend2.Name = "LegendInterceptor1";
            legend2.Position.Auto = false;
            legend2.Position.Height = 8.3F;
            legend2.Position.Width = 35F;
            legend2.Position.X = 25F;
            legend2.Position.Y = 10F;
            legend2.TextWrapThreshold = 20;
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend3.Enabled = false;
            legend3.MaximumAutoSize = 10F;
            legend3.Name = "LegendInterceptor2";
            legend3.Position.Auto = false;
            legend3.Position.Height = 8.3F;
            legend3.Position.Width = 35F;
            legend3.Position.X = 60F;
            legend3.Position.Y = 10F;
            this.SyncChart.Legends.Add(legend1);
            this.SyncChart.Legends.Add(legend2);
            this.SyncChart.Legends.Add(legend3);
            this.SyncChart.Location = new System.Drawing.Point(0, 0);
            this.SyncChart.Name = "SyncChart";
            this.SyncChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent;
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "LegendSyncWeights";
            series1.LegendText = "Синхронизация\nабонентов";
            series1.Name = "SyncWeights";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "LegendInterceptor1";
            series2.LegendText = "Синхронизация перехватичка\\nс первым абонентом";
            series2.Name = "SyncWeightsInterceptor1";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series3.BorderWidth = 3;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "LegendInterceptor2";
            series3.LegendText = "Синхронизация перехватичка\\nсо вторым абонентом";
            series3.Name = "SyncWeightsInterceptor2";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.SyncChart.Series.Add(series1);
            this.SyncChart.Series.Add(series2);
            this.SyncChart.Series.Add(series3);
            this.SyncChart.Size = new System.Drawing.Size(509, 290);
            this.SyncChart.TabIndex = 1;
            this.SyncChart.Text = "График синхронизации весов";
            title1.Name = "Title1";
            title1.Text = "Синхронизация весов двух ДМЧ";
            this.SyncChart.Titles.Add(title1);
            // 
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 290);
            this.Controls.Add(this.SyncChart);
            this.Name = "GraphForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GraphForm";
            ((System.ComponentModel.ISupportInitialize)(this.SyncChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart SyncChart;
    }
}