using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Mergen_GCSS
{
   
    public class Server
    {
        private mainForm formm;
        
        private DataGridViewHandler dataGridViewHandler;
        private ChartHandler chartHandler;
        private _3dSim simulation;
        private int[] _ports;
        private byte[] _buffer = new byte[1024]; // buffer değişkenini sınıf seviyesinde tanımladık
        private TcpClient _client5002;
        
        public float Port5001FloatValue { get; set; } = 3.1415f; // Port 5001 için float değerini sınıf değişkeni olarak tanımladık


        public int LastInt1 ;//{ get; private set; } // Port 5000 üzerinden gelen son int
        public int LastInt2 ;//{ get; private set; } // Port 5000 üzerinden gelen son int
        public string LastString1 ;//{ get; private set; } // Port 5000 üzerinden gelen son string
        public string LastString2 ;//{ get; private set; } // Port 5000 üzerinden gelen son string
        public float LastFloat1 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat2 ;// { get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat3 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat4 ;// { get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat5 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat6 ;// { get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat7 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat8 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat9 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat10 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat11 ;// { get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat12 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat13 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public float LastFloat14 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public string LastString3 ;//{ get; private set; } // Port 5000 üzerinden gelen son string
        public float LastFloat15 ;//{ get; private set; } // Port 5000 üzerinden gelen son float
        public int LastInt3 ;//{ get; private set; } // Port 5000 üzerinden gelen son int

        private void AddDataToGridAndCharts(int LastInt1,int LastInt2 , string LastString1, string LastString2, float LastFloat1, float LastFloat2, float LastFloat3, float LastFloat4, float LastFloat5, float LastFloat6, float LastFloat7, float LastFloat8, float LastFloat9, float LastFloat10, float LastFloat11, float LastFloat12, float LastFloat13, float LastFloat14, string LastString3, float LastFloat15, int LastInt3)
        {
           
            dataGridViewHandler.AddData( LastInt1,  LastInt2,  LastString1,  LastString2,  LastFloat1,  LastFloat2,  LastFloat3,  LastFloat4, LastFloat5,  LastFloat6,  LastFloat7, LastFloat8,  LastFloat9,  LastFloat10,  LastFloat11,  LastFloat12,  LastFloat13,  LastFloat14,  LastString3,  LastFloat15, LastInt3);
            chartHandler.UpdateCharts( LastString2,LastFloat1, LastFloat2, LastFloat3, LastFloat4, LastFloat5, LastFloat6, LastFloat7, LastFloat8, LastFloat15);
            simulation.UpdateRotation(LastFloat12, LastFloat13, LastFloat14);
            formm.Aras(LastString1);

        }

        public Server(int[] ports, DataGridViewHandler dataGridViewHandler, ChartHandler chartHandler, mainForm formm, _3dSim simulation)
        {
            _ports = ports;
            this.dataGridViewHandler = dataGridViewHandler;
            this.chartHandler = chartHandler; 
            this.formm = formm;
            this.simulation = simulation;


        }

        public void Start()
        {
            foreach (int port in _ports)
            {
                Task.Run(() => StartServer(port)); // Her port için ayrı bir görev başlat
            }

            Console.WriteLine("Server başlatıldı. Bağlantılar bekleniyor...");
        }


        private void StartServer(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Port {port} üzerinde dinleniyor...");

            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine($"Bağlantı kabul edildi: {port}");

                    if (port == 5002)
                    {
                        _client5002 = client; // Port 5002 için bağlantıyı saklayalım
                    }

                    Task.Run(() => HandleClient(client, port));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Port {port} üzerinde hata: {ex.Message}");
                }
            }
        }

        private void HandleClient(TcpClient client, int port)
        {
            NetworkStream stream = client.GetStream();

            while (true)
            {
                try
                {
                    if (port == 5000)
                    {
                        HandlePort5000(stream);
                    }
                    else if (port == 5001)
                    {
                        HandlePort5001(stream);
                    }
                    else if (port == 5002)
                    {
                        HandlePort5002(stream);
                    }
                    else
                    {
                        HandleOtherPorts(stream, port);
                    }

                    Thread.Sleep(1000); // 1 saniye bekle
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Port {port} üzerinde hata: {ex.Message}");
                    break; // Hata durumunda döngüden çık ve bağlantıyı kapat
                }
            }

            client.Close();
            Console.WriteLine($"Port {port} üzerindeki bağlantı kapatıldı.");
        }

        private void HandlePort5000(NetworkStream stream)
        {
            byte[] intBuffer = new byte[4];
            stream.Read(intBuffer, 0, 4);
            LastInt1 = BitConverter.ToInt32(intBuffer, 0);
            

            byte[] intBuffer2 = new byte[4];
            stream.Read(intBuffer2, 0, 4);
            LastInt2 = BitConverter.ToInt32(intBuffer2, 0);
            

            byte[] lengthBuffer = new byte[4];
            stream.Read(lengthBuffer, 0, 4);
            int stringLength = BitConverter.ToInt32(lengthBuffer, 0);
            byte[] stringBuffer = new byte[stringLength];
            stream.Read(stringBuffer, 0, stringLength);
            LastString1 = Encoding.UTF8.GetString(stringBuffer);
            

            byte[] lengthBuffer2 = new byte[4];
            stream.Read(lengthBuffer2, 0, 4);
            int stringLength2 = BitConverter.ToInt32(lengthBuffer2, 0);
            byte[] stringBuffer2 = new byte[stringLength2];
            stream.Read(stringBuffer2, 0, stringLength2);
            LastString2 = Encoding.UTF8.GetString(stringBuffer2);
            

            byte[] floatBuffer = new byte[4];
            stream.Read(floatBuffer, 0, 4);
            LastFloat1 = BitConverter.ToSingle(floatBuffer, 0);
           

            byte[] floatBuffer2 = new byte[4];
            stream.Read(floatBuffer2, 0, 4);
            LastFloat2 = BitConverter.ToSingle(floatBuffer2, 0);
           

            byte[] floatBuffer3 = new byte[4];
            stream.Read(floatBuffer3, 0, 4);
            LastFloat3 = BitConverter.ToSingle(floatBuffer3, 0);
          

            byte[] floatBuffer4 = new byte[4];
            stream.Read(floatBuffer4, 0, 4);
            LastFloat4 = BitConverter.ToSingle(floatBuffer4, 0);
           

            byte[] floatBuffer5 = new byte[4];
            stream.Read(floatBuffer5, 0, 4);
            LastFloat5 = BitConverter.ToSingle(floatBuffer5, 0);
           

            byte[] floatBuffer6 = new byte[4];
            stream.Read(floatBuffer6, 0, 4);
            LastFloat6 = BitConverter.ToSingle(floatBuffer6, 0);
            

            byte[] floatBuffer7 = new byte[4];
            stream.Read(floatBuffer7, 0, 4);
            LastFloat7 = BitConverter.ToSingle(floatBuffer7, 0);
            

            byte[] floatBuffer8 = new byte[4];
            stream.Read(floatBuffer8, 0, 4);
            LastFloat8 = BitConverter.ToSingle(floatBuffer8, 0);
           

            byte[] floatBuffer9 = new byte[4];
            stream.Read(floatBuffer9, 0, 4);
            LastFloat9 = BitConverter.ToSingle(floatBuffer9, 0);
           

            byte[] floatBuffer10 = new byte[4];
            stream.Read(floatBuffer10, 0, 4);
            LastFloat10 = BitConverter.ToSingle(floatBuffer10, 0);
           

            byte[] floatBuffer11 = new byte[4];
            stream.Read(floatBuffer11, 0, 4);
            LastFloat11 = BitConverter.ToSingle(floatBuffer11, 0);
           

            byte[] floatBuffer12 = new byte[4];
            stream.Read(floatBuffer12, 0, 4);
            LastFloat12 = BitConverter.ToSingle(floatBuffer12, 0);
            

            byte[] floatBuffer13 = new byte[4];
            stream.Read(floatBuffer13, 0, 4);
            LastFloat13 = BitConverter.ToSingle(floatBuffer13, 0);
            

            byte[] floatBuffer14 = new byte[4];
            stream.Read(floatBuffer14, 0, 4);
            LastFloat14 = BitConverter.ToSingle(floatBuffer14, 0);
           

            byte[] lengthBuffer3 = new byte[4];
            stream.Read(lengthBuffer3, 0, 4);
            int stringLength3 = BitConverter.ToInt32(lengthBuffer3, 0);
            byte[] stringBuffer3 = new byte[stringLength3];
            stream.Read(stringBuffer3, 0, stringLength3);
            LastString3 = Encoding.UTF8.GetString(stringBuffer3);
           

            byte[] floatBuffer15 = new byte[4];
            stream.Read(floatBuffer15, 0, 4);
            LastFloat15 = BitConverter.ToSingle(floatBuffer15, 0);
            

            byte[] intBuffer3 = new byte[4];
            stream.Read(intBuffer3, 0, 4);
            LastInt3 = BitConverter.ToInt32(intBuffer3, 0);

            AddDataToGridAndCharts(LastInt1, LastInt2, LastString1, LastString2, LastFloat1, LastFloat2, LastFloat3, LastFloat4, LastFloat5, LastFloat6, LastFloat7, LastFloat8, LastFloat9, LastFloat10, LastFloat11, LastFloat12, LastFloat13, LastFloat14, LastString3, LastFloat15, LastInt3);

        }

        private void HandlePort5001(NetworkStream stream)
        {
            byte[] floatBytes = BitConverter.GetBytes(Port5001FloatValue); // Sınıf değişkenini kullanıyoruz
            stream.Write(floatBytes, 0, floatBytes.Length);
           
        }

        private void HandlePort5002(NetworkStream stream)
        {
            // Port 5002'de sürekli mesaj göndermeyi kaldırdık
        }

        private void HandleOtherPorts(NetworkStream stream, int port)
        {
            int bytesRead = stream.Read(_buffer, 0, _buffer.Length);
            if (bytesRead == 0) return; // Client bağlantısı kapandığında döngüden çık

            string message = Encoding.UTF8.GetString(_buffer, 0, bytesRead);
           

            // Geri bildirim mesajı gönder
            string responseMessage = $"Mesaj alındı: {DateTime.Now}";
            byte[] response = Encoding.UTF8.GetBytes(responseMessage);
            stream.Write(response, 0, response.Length);
        }

        public void SendMessageToPort5002(string message)
        {
            if (_client5002 != null && _client5002.Connected)
            {
                NetworkStream stream = _client5002.GetStream();
                byte[] stringBytes = Encoding.UTF8.GetBytes(message);
                stream.Write(stringBytes, 0, stringBytes.Length);
                Console.WriteLine($"Port 5002 üzerinden client'a gönderilen mesaj: {message}");
            }
            else
            {
                MessageBox.Show("Port 5002 bağlantısı mevcut değil.");
            }
        }
    
}
}
