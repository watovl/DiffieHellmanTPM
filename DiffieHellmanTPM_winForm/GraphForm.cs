using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DiffieHellmanTPM_winForm {
    /// <summary>
    /// Форма для построения и вывода графика
    /// </summary>
    public partial class GraphForm : Form {
        public GraphForm() {
            InitializeComponent();

            SyncChart.ChartAreas[0].AxisX.Minimum = 0;
            SyncChart.ChartAreas[0].AxisX.MajorGrid.Interval = 50;
            SyncChart.ChartAreas[0].AxisX.MajorTickMark.Interval = 50;

            SyncChart.ChartAreas[0].AxisY.Minimum = 0;
            SyncChart.ChartAreas[0].AxisY.Maximum = 100;
            SyncChart.ChartAreas[0].AxisY.MajorGrid.Interval = 5;
            SyncChart.ChartAreas[0].AxisY.MajorTickMark.Interval = 10;
        }

        /// <summary>
        /// Построение графика синхронизации весов
        /// </summary>
        /// <param name="syncPoints">Массив точек синхронизации весов</param>
        public void DrawSyncGraph(double[] syncPoints) {
            SyncChart.ChartAreas[0].AxisX.Maximum = syncPoints.Length;
            int minY = SyncChart.ChartAreas[0].AxisY.Minimum == 0
                ? (int)syncPoints.Min() 
                : (int)Math.Min((int)syncPoints.Min(), SyncChart.ChartAreas[0].AxisY.Minimum);
            SyncChart.ChartAreas[0].AxisY.Minimum = minY - (minY % 5);

            foreach (int value in Enumerable.Range(0, syncPoints.Length)) {
                SyncChart.Series["SyncWeights"].Points.AddXY(value, syncPoints[value]);
            }
        }

        /// <summary>
        /// Построение графика синхронизации весов перехватчика
        /// </summary>
        /// <param name="syncPoints1">Массив точек синхронизации с первым абонентом</param>
        /// <param name="syncPoints2">Массив точек синхронизации со вторым абонентом</param>
        public void DrawSyncInterceptorGraph(double[] syncPoints1, double[] syncPoints2) {
            EnableLegends();

            int minY = Math.Min((int)syncPoints1.Min(), (int)syncPoints2.Min());
            SyncChart.ChartAreas[0].AxisY.Minimum = minY - (minY % 5);

            foreach (int value in Enumerable.Range(0, syncPoints1.Length)) {
                SyncChart.Series["SyncWeightsInterceptor1"].Points.AddXY(value, syncPoints1[value]);
                SyncChart.Series["SyncWeightsInterceptor2"].Points.AddXY(value, syncPoints2[value]);
            }
        }

        private void EnableLegends() {
            SyncChart.Legends["LegendSyncWeights"].Enabled = true;
            SyncChart.Legends["LegendInterceptor1"].Enabled = true;
            SyncChart.Legends["LegendInterceptor2"].Enabled = true;
        }
    }
}
