using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Döviz_Programı
{
    public partial class KurListesi : Form
    {
        public KurListesi()
        {
            InitializeComponent();
            List<Doviz> CurList = null;
            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString("http://www.doviz.com/api/v1/currencies/all/latest");
                CurList = JsonConvert.DeserializeObject<List<Doviz>>(json);
            }

            if (CurList == null)
                return;
            foreach (Doviz d in CurList)
                listBox1.Items.Add(d.name);
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            List<Doviz> CurList = null;
            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString("http://www.doviz.com/api/v1/currencies/all/latest");
                CurList = JsonConvert.DeserializeObject<List<Doviz>>(json);
            }
            Doviz dov=new Doviz();

            if (CurList == null)
                return;

            foreach (Doviz d in CurList)
                if(d.name==listBox1.SelectedItem.ToString())
                {
                    dov = d;
                }
            label2.Text = dov.name;
            label6.Text = dov.buying.ToString();
            label5.Text = dov.selling.ToString();

            if (dov.change_rate > 0)
                label5.ForeColor = Color.ForestGreen;
            else      
                label5.ForeColor = Color.IndianRed;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
