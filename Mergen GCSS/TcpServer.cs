using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

public class TcpServer
{
    private TcpListener listener;
    private Thread listenerThread;
    private DataGridView dataGridView;

    public TcpServer(string ipAddress, int port, DataGridView dgv)
    {
        listener = new TcpListener(IPAddress.Parse(ipAddress), port);
        dataGridView = dgv;
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
                    byte[] buffer = new byte[12];
                    int bytesRead = clientSocket.Receive(buffer, 0, 12, SocketFlags.None);
                    if (bytesRead == 0) break;
                    int[] intValues = new int[3];
                    Buffer.BlockCopy(buffer, 0, intValues, 0, 12);

                    byte[] lengthBuffer = new byte[4];
                    clientSocket.Receive(lengthBuffer, 0, 4, SocketFlags.None);
                    int stringLength = BitConverter.ToInt32(lengthBuffer, 0);
                    byte[] stringBuffer = new byte[stringLength];
                    clientSocket.Receive(stringBuffer, 0, stringLength, SocketFlags.None);
                    string receivedString = Encoding.UTF8.GetString(stringBuffer);

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

                    AddDataToGrid(intValues, receivedString, floatValues, additionalString, receivedFloat, receivedInt);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("İstemciyle iletişimde hata: " + e.Message);
            }
        }
    }

    private void AddDataToGrid(int[] intValues, string receivedString, float[] floatValues, string additionalString, float receivedFloat, int receivedInt)
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

    public void Stop()
    {
        listener.Stop();
        if (listenerThread != null && listenerThread.IsAlive)
        {
            listenerThread.Abort();
        }
    }
}
