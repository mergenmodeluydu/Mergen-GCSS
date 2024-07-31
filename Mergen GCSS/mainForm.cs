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
    }
}
