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
        public mainForm()
        {
            InitializeComponent();
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


            chart1.Series[0].ChartType = SeriesChartType.Line;
            // chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "HH:mm:ss";

            // Veri saklama için bir sınırlayıcı tanımla
            int maxDataPoints = 8;

            // X eksenindeki zamanı güncelleme
            Timer timer1 = new Timer();
            timer1.Interval = 1000; // Her saniyede bir güncelle
            timer1.Tick += (sender, e) =>
            {
                Random rnd = new Random();
                int random = rnd.Next(1, 6);

                DateTime now = DateTime.Now;
                string zaman = now.ToString("HH:mm:ss");

                // Eğer mevcut veri sayısı maksimuma ulaştıysa ilk veriyi kaldır
                if (chart1.Series[0].Points.Count >= maxDataPoints)
                {
                    chart1.Series[0].Points.RemoveAt(0);
                }

                chart1.Series[0].Points.AddXY(zaman, random);

                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisY.Interval = 1;
            };
            timer1.Start();
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
    }
}
