using GMap.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net.Sockets;


namespace Mergen_GCSS
{
    public partial class mainForm : Form
    {
        private TcpServer server;
        private DataGridViewHandler dataGridViewHandler;
        private ChartHandler chartHandler;
        public  mainForm()
        {
            
            


            InitializeComponent();

            dataGridViewHandler = new DataGridViewHandler(dataGridView1);
            chartHandler = new ChartHandler(this);
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


            timer1.Tick += UpdateTimer_Tick;
           
            
        }
        
        public void UpdateTimer_Tick(object sender, EventArgs e)
        {
           
            
           
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void button1_Click(object sender, EventArgs e)
        {

            server = new TcpServer("192.168.137.228", 65432, dataGridViewHandler, chartHandler);
            server.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            server.Stop();
        }
    }
}
