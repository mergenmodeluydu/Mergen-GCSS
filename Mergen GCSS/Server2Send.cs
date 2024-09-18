using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mergen_GCSS
{
    internal class Server2Send
    {
        private TcpListener server;
        private TcpClient client;
        private bool clientConnected = false;

        public Server2Send(string ip, int port)
        {
            server = new TcpListener(IPAddress.Parse(ip), port);
        }

        // Asenkron sunucu başlatma metodu
        public async Task StartAsync()
        {
            server.Start();
            Console.WriteLine("Server başlatıldı... Bağlantı bekleniyor(server2send)...");

            while (true)
            {
                try
                {
                    if (client == null || !clientConnected) // Eğer client bağlı değilse yeniden bağlan
                    {
                        client = await server.AcceptTcpClientAsync();
                        clientConnected = true;
                        Console.WriteLine("Client bağlandı(server2send).");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Bağlantı hatası: {e.Message}. Tekrar bağlanmayı deniyorum...");
                    await Task.Delay(5000); // Bağlantı hatası olduğunda 5 saniye bekleyip tekrar dene
                }
            }
        }

        // Asenkron Client'a string veri gönderme fonksiyonu
        public async Task SendStringToClientAsync(string data)
        {
            if (client != null && clientConnected)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);

                    await stream.WriteAsync(lengthBytes, 0, lengthBytes.Length);
                    await stream.WriteAsync(dataBytes, 0, dataBytes.Length);

                    Console.WriteLine($"Gönderilen veri: {data}");
                }
                catch (Exception e) // Veri gönderirken hata alırsanız bağlantıyı kapatın
                {
                    Console.WriteLine($"Veri gönderme hatası: {e.Message}. Bağlantı koptu.");
                    client.Close(); // Bağlantı koptuğunda client'ı kapat
                    clientConnected = false; // Client bağlı değil olarak işaretle
                }
            }
            else
            {
                Console.WriteLine("Client bağlı değil, veri gönderilemedi.");
            }
        }

        // Sunucuyu durdurma metodu
        public void Stop()
        {
            if (client != null)
            {
                client.Close();
                clientConnected = false;
            }
            server.Stop();
            Console.WriteLine("Server kapatıldı.");
        }
    }
}
