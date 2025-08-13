using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guna.Charts.WinForms;
using System;
using System.Data;
using System.Windows.Forms;
using Guna.UI2.WinForms.Suite;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace Стоматология.Classes
{
    internal class CreatorChart
    {
        public bool checkEmpty(DataTable dataTable)
        {
            return dataTable.Rows.Count > 0;
        }
        public void ChartBar(GunaChart chart, DataTable data, string nameChart)
        {
            if (checkEmpty(data))
            {
                chart.Datasets.Clear();

                // Chart configuration
                //chart.Legend.Position = Guna.Charts.WinForms.LegendPosition.Right;
                chart.Legend.Display = false;
                chart.YAxes.GridLines.Display = true;
                chart.XAxes.Display = true;
                chart.YAxes.Display = true;
                chart.Title.Text = nameChart;

                var dataset = new GunaBarDataset();
                dataset.Label = "Общая стоимость"; // Устанавливаем название для легенды

                for (int i = 0; i < data.Rows.Count; i++)
                {
                    dataset.DataPoints.Add(
                        Convert.ToString(data.Rows[i][0]),
                        Convert.ToDouble(data.Rows[i][1]));
                }

                chart.Datasets.Add(dataset);
            }
            else
            {MessageBox.Show("Данных не достаточно.", "Ошибка!");}
        }
        public void ChartHorizontalBar(GunaChart chart, DataTable data, string nameChart)
        {
            if (checkEmpty(data))
            {
                chart.Datasets.Clear(); //Chart configuration
                chart.Legend.Display = false;
                chart.XAxes.Display = true;
                chart.YAxes.Display = true;
                chart.Title.Text = nameChart;
                var dataset = new GunaHorizontalBarDataset();
                dataset.Label = "Общая стоимость оказанной услуги"; // Устанавливаем название для легенды
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    dataset.DataPoints.Add(
                        Convert.ToString(data.Rows[i][0]),
                        Convert.ToDouble(data.Rows[i][1])
                    );
                }
                chart.Datasets.Add(dataset);
            }
            else
            {
                MessageBox.Show("Данных не достаточно.", "Ошибка!");
            }
        }
    }
}
