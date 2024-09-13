using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;


public class ChartHandler
{
    private Form form;

    public ChartHandler(Form form)
    {
        this.form = form;
    }

    public void UpdateCharts(string LastString2, float LastFloat1, float LastFloat2, float LastFloat3, float LastFloat4, float LastFloat5, float LastFloat6, float LastFloat7, float LastFloat8,float LastFloat15)
    {
        var parts = LastString2.Split(',');
        if (parts.Length < 2) return;
        string timePart = parts[1];

        // Zaman kısmındaki '/' işaretlerini ':' ile değiştir
        string formattedTime = timePart.Replace('/', ':');
        form.Invoke((MethodInvoker)delegate
        {
            
            
            Chart chart1 = form.Controls.Find($"chart1", true).FirstOrDefault() as Chart;
            if (chart1 != null)
            {
                chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                chart1.Series[0].Points.AddXY(formattedTime, LastFloat1);
            }
            Chart chart2 = form.Controls.Find($"chart2", true).FirstOrDefault() as Chart;
            if (chart2 != null)
            {
                chart2.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                chart2.Series[0].Points.AddXY(formattedTime, LastFloat2);
            }
            Chart chart3 = form.Controls.Find($"chart3", true).FirstOrDefault() as Chart;
            if (chart3 != null)
            {
                chart3.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                chart3.Series[0].Points.AddXY(formattedTime, LastFloat3);
            }
            Chart chart4 = form.Controls.Find($"chart4", true).FirstOrDefault() as Chart;
            if (chart4 != null)
            {
                chart4.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                chart4.Series[0].Points.AddXY(formattedTime, LastFloat4);
            }
            Chart chart5 = form.Controls.Find($"chart5", true).FirstOrDefault() as Chart;
            if (chart5 != null)
            {
                chart5.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                chart5.Series[0].Points.AddXY(formattedTime, LastFloat5);
            }
            Chart chart6 = form.Controls.Find($"chart6", true).FirstOrDefault() as Chart;
            if (chart6 != null)
            {
                chart6.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                chart6.Series[0].Points.AddXY(formattedTime, LastFloat6);
            }
            Chart chart7 = form.Controls.Find($"chart7", true).FirstOrDefault() as Chart;
            if (chart7 != null)
            {
                chart7.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                chart7.Series[0].Points.AddXY(formattedTime, LastFloat7);
            }
            Chart chart8 = form.Controls.Find($"chart8", true).FirstOrDefault() as Chart;
            if (chart8 != null)
            {
                chart8.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                chart8.Series[0].Points.AddXY(formattedTime, LastFloat8);
            }

            Chart chart9 = form.Controls.Find("chart9", true).FirstOrDefault() as Chart;
            if (chart9 != null)
            {
                chart9.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                chart9.Series[0].Points.AddXY(formattedTime,LastFloat15);
            }
        });
    }
}
