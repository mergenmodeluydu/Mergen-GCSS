using Accord.Video.FFMPEG;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Mergen_GCSS
{
    public partial class mainForm : Form
    {

        private Camera camera;
        private TcpServer server;
        private GondericiServer gonderici;

        private DataGridViewHandler dataGridViewHandler;
        private ChartHandler chartHandler;
        public mainForm()
        {
            InitializeComponent();
            gonderici = new GondericiServer("192.168.137.169", 65432);
            button2.Enabled = false;
            dataGridViewHandler = new DataGridViewHandler(dataGridView1);
            chartHandler = new ChartHandler(this);
            camera = new Camera(pictureBox7);
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMapControl1.SetPositionByKeywords("Istanbul, Turkey");

            // Koordinatlarla manuel olarak pozisyon ayarlama
           double latitude = 38.707901; // Enlem (Latitude)
            double longitude = 35.519833; // Boylam (Longitude)

            // Haritayı bu koordinatlara göre merkezleme
            gMapControl1.Position = new PointLatLng(latitude, longitude);
            gMapControl1.MinZoom = 5;
            gMapControl1.MaxZoom = 100;
            gMapControl1.Zoom = 10;   


            
        }

        

        private void label6_Click(object sender, EventArgs e)
        {
            DialogResult firstResponse = MessageBox.Show("Uygulamadan çıkmak istediğinize emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (firstResponse == DialogResult.Yes)
            {
                // İkinci onay kutusu
                DialogResult secondResponse = MessageBox.Show("Emin misiniz? Cidden çıkmak istiyor musunuz? Videoyu vs kontrol ettiniz mi?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (secondResponse == DialogResult.Yes)
                {
                    // Uygulamadan çıkış
                    Application.Exit();
                }
            }
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.ForeColor = Color.DarkRed;
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Red;

        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Chartreuse;
            pictureBox2.BackColor = Color.Chartreuse;
            pictureBox3.BackColor = Color.Chartreuse;
            pictureBox4.BackColor = Color.Chartreuse;
            pictureBox5.BackColor = Color.Chartreuse;

            label7.ForeColor = Color.Chartreuse;
            label8.ForeColor = Color.Chartreuse;
            label9.ForeColor = Color.Chartreuse;
            label10.ForeColor = Color.Chartreuse;
            label11.ForeColor = Color.Chartreuse;

        }

        private  void button3_Click(object sender, EventArgs e)
        {
            camera.StartRecording();
        }

        private  void button4_Click(object sender, EventArgs e)
        {
            camera.StopRecording();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server = new TcpServer("192.168.218.114", 12345, dataGridViewHandler, chartHandler);
            server.Start();
            button1.Enabled = false; // Başlat butonunu devre dışı bırak
            button2.Enabled = true;   // Durdur butonunu etkinleştir
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // server.Stop();
            if (server != null)
            {
                server.Stop(); // server olarak tanımladığınız TcpServer'ı durdurun
                server = null;

            }
            button1.Enabled = true;  // Başlat butonunu etkinleştir
            button2.Enabled = false;
        }
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Alt+F4 veya kullanıcı tarafından kapatma işlemi engellenir
                e.Cancel = true;
                MessageBox.Show("Alt+F4 ile uygulamayı kapatamazsınız!", "Kapatma Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            gonderici.SendCommand("80");

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string command = textBox1.Text;
            if (isValidCommand(command))
            {
                gonderici.SendCommand(command);
            }
            else
            {
                MessageBox.Show("Hatalı komut. Lütfen geçerli bir komut girin.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            gonderici.SendCommand("180");
        }
        private bool isValidCommand(string command)
        {
            // Komut doğrulama mantığını buraya ekleyebilirsiniz (Python kodundaki gibi)
            if (command.Length != 4) return false;
            if (!char.IsDigit(command[0]) || !char.IsLetter(command[1]) ||
                !char.IsDigit(command[2]) || !char.IsLetter(command[3])) return false;
            if (!"RGBrgb".Contains(command[1]) || !"RGBrgb".Contains(command[3])) return false;
            if (int.Parse(command[0].ToString()) < 1 || int.Parse(command[0].ToString()) > 9 ||
                int.Parse(command[2].ToString()) < 1 || int.Parse(command[2].ToString()) > 9) return false;

            return true;
        }
    }

}
