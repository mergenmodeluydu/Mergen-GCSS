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

namespace Mergen_GCSS
{
    public partial class mainForm : Form
    {
        private Camera camera;
        private TcpServer server;
        private DataGridViewHandler dataGridViewHandler;
        private ChartHandler chartHandler;
        public mainForm()
        {
            InitializeComponent();
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
            server = new TcpServer("192.168.218.26", 12345, dataGridViewHandler, chartHandler);
            server.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            server.Stop();
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
    }
}
