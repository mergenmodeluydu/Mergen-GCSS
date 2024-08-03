using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Mergen_GCSS
{   
    public class ReceivedData
{
    public int[] IntValues { get; set; }
    public string FirstString { get; set; }
    public float[] FloatValues { get; set; }
    public string SecondString { get; set; }
    public float LastFloat { get; set; }
    public int LastInt { get; set; }
}

    public class wifi
    {
        
       public  static Socket basla()
        {
            // Sunucu bilgileri
            IPAddress ipAddress = IPAddress.Parse("192.168.187.114");
             int port = 65432;

            // Sunucu soketi oluşturma
            TcpListener listener = new TcpListener(ipAddress, port);
            listener.Start();
            //Console.WriteLine("Sunucu başlatıldı ve bağlantılar dinleniyor...");

            while (true)
            {
                Socket clientSocket = listener.AcceptSocket();
                //Console.WriteLine("Bağlantı kabul edildi.");

                // Yeni bir thread oluştur ve bağlantıyı işleyici metoda geçir
                Thread clientThread = new Thread(() => HandleClient(clientSocket));
                clientThread.Start();
                return clientSocket;
            }
        }



        public static ReceivedData HandleClient(Socket clientSocket)
        {
            var receivedData = new ReceivedData();

            using (clientSocket)
            {
                try
                {
                    while (true)
                    {   
                        // 3 tane int sayı al
                        byte[] buffer = new byte[12];
                        int bytesRead = clientSocket.Receive(buffer, 0, 12, SocketFlags.None);
                        if (bytesRead == 0) break; // Bağlantı kapandıysa döngüden çık
                        receivedData.IntValues = new int[3];
                        receivedData.IntValues[0] = 3;
                        receivedData.IntValues[1] = 0;
                        receivedData.IntValues[2] = 5;
                        Buffer.BlockCopy(buffer, 0, receivedData.IntValues, 0, 12);

                        // Bir string al
                        byte[] lengthBuffer = new byte[4];
                        clientSocket.Receive(lengthBuffer, 0, 4, SocketFlags.None);
                        int stringLength = BitConverter.ToInt32(lengthBuffer, 0);
                        byte[] stringBuffer = new byte[stringLength];
                        clientSocket.Receive(stringBuffer, 0, stringLength, SocketFlags.None);
                        receivedData.FirstString = Encoding.UTF8.GetString(stringBuffer);

                        // 14 tane float sayı al
                        buffer = new byte[14 * 4];
                        clientSocket.Receive(buffer, 0, 14 * 4, SocketFlags.None);
                        receivedData.FloatValues = new float[14];
                        Buffer.BlockCopy(buffer, 0, receivedData.FloatValues, 0, 14 * 4);

                        // Ek bir string al
                        clientSocket.Receive(lengthBuffer, 0, 4, SocketFlags.None);
                        stringLength = BitConverter.ToInt32(lengthBuffer, 0);
                        stringBuffer = new byte[stringLength];
                        clientSocket.Receive(stringBuffer, 0, stringLength, SocketFlags.None);
                        receivedData.SecondString = Encoding.UTF8.GetString(stringBuffer);

                        // Bir float sayı al
                        buffer = new byte[4];
                        clientSocket.Receive(buffer, 0, 4, SocketFlags.None);
                        receivedData.LastFloat = BitConverter.ToSingle(buffer, 0);

                        // Bir int sayı al
                        buffer = new byte[4];
                        clientSocket.Receive(buffer, 0, 4, SocketFlags.None);
                        receivedData.LastInt = BitConverter.ToInt32(buffer, 0);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("İstemciyle iletişimde hata: " + e.Message);
                }
            }

            return receivedData;
        }





    }
}
