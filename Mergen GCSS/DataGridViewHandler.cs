using System.Windows.Forms;

public class DataGridViewHandler
{
    private DataGridView dataGridView;

    public DataGridViewHandler(DataGridView dataGridView)
    {
        this.dataGridView = dataGridView;
    }

    public void AddData(int[] intValues, string receivedString, float[] floatValues, string additionalString, float receivedFloat, int receivedInt)
    {
        dataGridView.Invoke((MethodInvoker)delegate
        {
            dataGridView.Rows.Add(intValues[0], intValues[1], intValues[2], receivedString,
                floatValues[0], floatValues[1], floatValues[2], floatValues[3], floatValues[4],
                floatValues[5], floatValues[6], floatValues[7], floatValues[8], floatValues[9],
                floatValues[10], floatValues[11], floatValues[12], floatValues[13],
                additionalString, receivedFloat, receivedInt);
        });
    }
}
