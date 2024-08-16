using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;


public class TcpServer
{
    private TcpListener listener;
    private Thread listenerThread;
    private DataGridView dataGridView;
    private Form form;
    private DataGridViewHandler dataGridViewHandler;
    private ChartHandler chartHandler;

    public TcpServer(string ipAddress, int port, DataGridViewHandler dataGridViewHandler, ChartHandler chartHandler)
    {
        listener = new TcpListener(IPAddress.Parse(ipAddress), port);
        this.dataGridViewHandler = dataGridViewHandler;
        this.chartHandler = chartHandler;
    }
    private void AddDataToGridAndCharts(int[] intValues, string receivedString, string receivedString2, float[] floatValues, string additionalString, float receivedFloat, int receivedInt)
    {
        dataGridViewHandler.AddData(intValues, receivedString, receivedString2, floatValues, additionalString, receivedFloat, receivedInt);
        chartHandler.UpdateCharts(floatValues, receivedFloat);
    }

    public void Start()
    {
        listener.Start();
        listenerThread = new Thread(ListenForClients);
        listenerThread.Start();
    }

    private void ListenForClients()
    {
        while (true)
        {
            Socket clientSocket = listener.AcceptSocket();
            Thread clientThread = new Thread(() => HandleClient(clientSocket));
            clientThread.Start();
        }
    }

    private void HandleClient(Socket clientSocket)
    {
        using (clientSocket)
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[8];
                    int bytesRead = clientSocket.Receive(buffer, 0, 8, SocketFlags.None);
                    if (bytesRead == 0) break;
                    int[] intValues = new int[2];
                    Buffer.BlockCopy(buffer, 0, intValues, 0, 8);

                    byte[] lengthBuffer = new byte[4];
                    clientSocket.Receive(lengthBuffer, 0, 4, SocketFlags.None);
                    int stringLength = BitConverter.ToInt32(lengthBuffer, 0);
                    byte[] stringBuffer = new byte[stringLength];
                    clientSocket.Receive(stringBuffer, 0, stringLength, SocketFlags.None);
                    string receivedString = Encoding.UTF8.GetString(stringBuffer);

                    byte[] lengthBuffer2 = new byte[4];
                    clientSocket.Receive(lengthBuffer2, 0, 4, SocketFlags.None);
                    int stringLength2 = BitConverter.ToInt32(lengthBuffer2, 0);
                    byte[] stringBuffer2 = new byte[stringLength2];
                    clientSocket.Receive(stringBuffer2, 0, stringLength2, SocketFlags.None);
                    string receivedString2 = Encoding.UTF8.GetString(stringBuffer2);

                    buffer = new byte[14 * 4];
                    clientSocket.Receive(buffer, 0, 14 * 4, SocketFlags.None);
                    float[] floatValues = new float[14];
                    Buffer.BlockCopy(buffer, 0, floatValues, 0, 14 * 4);

                    clientSocket.Receive(lengthBuffer, 0, 4, SocketFlags.None);
                    stringLength = BitConverter.ToInt32(lengthBuffer, 0);
                    stringBuffer = new byte[stringLength];
                    clientSocket.Receive(stringBuffer, 0, stringLength, SocketFlags.None);
                    string additionalString = Encoding.UTF8.GetString(stringBuffer);

                    buffer = new byte[4];
                    clientSocket.Receive(buffer, 0, 4, SocketFlags.None);
                    float receivedFloat = BitConverter.ToSingle(buffer, 0);

                    buffer = new byte[4];
                    clientSocket.Receive(buffer, 0, 4, SocketFlags.None);
                    int receivedInt = BitConverter.ToInt32(buffer, 0);
                    AddDataToGridAndCharts(intValues, receivedString, receivedString2, floatValues, additionalString, receivedFloat, receivedInt);

                    // AddDataToGrid(intValues, receivedString, floatValues, additionalString, receivedFloat, receivedInt);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("İstemciyle iletişimde hata: " + e.Message);
            }
        }
    }

    /*private void AddDataToGrid(int[] intValues, string receivedString, float[] floatValues, string additionalString, float receivedFloat, int receivedInt)
    {
        dataGridView.Invoke((MethodInvoker)delegate
        {
            dataGridView.Rows.Add(intValues[0], intValues[1], intValues[2], receivedString,
           floatValues[0], floatValues[1], floatValues[2], floatValues[3], floatValues[4],
           floatValues[5], floatValues[6], floatValues[7], floatValues[8], floatValues[9],
           floatValues[10], floatValues[11], floatValues[12], floatValues[13],
           additionalString, receivedFloat, receivedInt);
            for (int i = 0; i <= 7; i++)
            {
                Chart chart = form.Controls.Find($"chart{i + 1}", true).FirstOrDefault() as Chart;
                if (chart != null)
                {
                    chart.Series[0].Points.AddY(floatValues[i]);
                }
            }

            // Chart9'a receivedFloat değerini ekleyin
            Chart chart9 = form.Controls.Find("chart9", true).FirstOrDefault() as Chart;
            if (chart9 != null)
            {
                chart9.Series[0].Points.AddY(receivedFloat);
            }
        });
    }*/

    public void Stop()
    {
        listener.Stop();
        if (listenerThread != null && listenerThread.IsAlive)
        {
            listenerThread.Abort();
        }
    }
}
