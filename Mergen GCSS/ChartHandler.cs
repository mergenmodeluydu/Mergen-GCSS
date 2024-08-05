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

    public void UpdateCharts(float[] floatValues, float receivedFloat)
    {
        form.Invoke((MethodInvoker)delegate
        {
            for (int i = 0; i <= 7; i++)
            {
                Chart chart = form.Controls.Find($"chart{i + 1}", true).FirstOrDefault() as Chart;
                if (chart != null)
                {
                    chart.Series[0].Points.AddY(floatValues[i]);
                }
            }

            Chart chart9 = form.Controls.Find("chart9", true).FirstOrDefault() as Chart;
            if (chart9 != null)
            {
                chart9.Series[0].Points.AddY(receivedFloat);
            }
        });
    }
}
