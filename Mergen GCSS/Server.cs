using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.UI.WebControls.WebParts;


namespace Mergen_GCSS
{
    public class Server
    {
        private TcpListener server;
        private mainForm formm;
        private DataGridViewHandler datagridVievHandler;
        private ChartHandler chartHandler;
        private _3dSim simulation;
        public TcpClient client;

        public Server(string ip, int port,DataGridViewHandler dataGridViewHandler,ChartHandler chartHandler,_3dSim simulation,mainForm formm)
        {
            server = new TcpListener(IPAddress.Parse(ip), port);
            this.datagridVievHandler= dataGridViewHandler;
            this.chartHandler= chartHandler; 
            this.simulation= simulation;
            this.formm= formm;

        }


        private void AddData(int param1, int param2,string param3,string param4, float param5, float param6, float param7, float param8, float param9, float param10, float param11, float param12, float param13, float param14, float param15, float param16, float param17, float param18,string param19,float param20,int param21)
        {
            datagridVievHandler.AddData(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15, param16, param17, param18, param19, param20, param21);
            chartHandler.UpdateCharts(param4, param5, param6, param7, param8, param9, param10, param11, param12, param20);
            simulation.UpdateRotation(param16, param17, param18);
            formm.Aras(param3);
            formm.Map(param12,param13);
        }
        // Server'ı başlatan metod
        public void Start()
        {
            server.Start();
            Console.WriteLine("Server başlatıldı... Bağlantı bekleniyor...");

            while (true)
            {
                try
                {
                    client = server.AcceptTcpClient();
                    Console.WriteLine("Client bağlandı.");
                    HandleClient(client);
                    client.Close();
                    Console.WriteLine("Client bağlantısı kapatıldı.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Hata: {e.Message}");
                }
            }
        }

        // Client bağlantısından gelen verileri işleyecek metod
        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            try
            {
                while (true) // Sürekli veri okuma döngüsü
                {
                    int param1 = ReadInt(stream);
                    int param2 = ReadInt(stream);
                    string param3 = ReadString(stream);
                    string param4 = ReadString(stream);
                    float param5 = ReadFloat(stream);
                    float param6 = ReadFloat(stream);
                    float param7 = ReadFloat(stream);
                    float param8 = ReadFloat(stream);
                    float param9 = ReadFloat(stream);
                    float param10 = ReadFloat(stream);
                    float param11 = ReadFloat(stream);
                    float param12 = ReadFloat(stream);
                    float param13 = ReadFloat(stream);
                    float param14 = ReadFloat(stream);
                    float param15 = ReadFloat(stream);
                    float param16 = ReadFloat(stream);
                    float param17 = ReadFloat(stream);
                    float param18 = ReadFloat(stream);
                    string param19 = ReadString(stream);
                    float param20 = ReadFloat(stream);
                    int param21 = ReadInt(stream);

                    AddData(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15, param16, param17, param18, param19, param20, param21);
                    Console.WriteLine($"Alınan veriler: {param1}, {param2}, {param3}, {param4}, {param5}, {param6}, {param7}, {param8}, {param9}, {param10}, {param11}, {param12}, {param13}, {param14}, {param15}, {param16}, {param17}, {param18}, {param19}, {param20}, {param21}");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Veri alma hatası: {e.Message}");
            }
        }

        // Int veri alma fonksiyonu
        private int ReadInt(NetworkStream stream)
        {
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt32(buffer, 0);
        }

        // Float veri alma fonksiyonu
        private float ReadFloat(NetworkStream stream)
        {
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToSingle(buffer, 0);
        }

        // String veri alma fonksiyonu
        private string ReadString(NetworkStream stream)
        {
            byte[] lengthBuffer = new byte[4];
            stream.Read(lengthBuffer, 0, lengthBuffer.Length);
            int stringLength = BitConverter.ToInt32(lengthBuffer, 0);

            byte[] stringBuffer = new byte[stringLength];
            stream.Read(stringBuffer, 0, stringLength);
            return Encoding.UTF8.GetString(stringBuffer);
        }

        // Client'a string veri gönderme fonksiyonu
        public void SendStringToClient(TcpClient client, string data)
        {
            Console.WriteLine(data);
            NetworkStream stream = client.GetStream();
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);

            stream.Write(lengthBytes, 0, lengthBytes.Length);
            stream.Write(dataBytes, 0, dataBytes.Length);

            Console.WriteLine($"Gönderilen veri: {data}");
        }

        // Server'ı durdurmak için kullanılacak metod
        public void Stop()
        {
            server.Stop();
            Console.WriteLine("Server kapatıldı.");
        }

        // Programın başlangıç noktası
    }
}
