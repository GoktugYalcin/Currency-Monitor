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
    public partial class KurSecim : Form
    {
        public KurSecim()
        {
            InitializeComponent();

            List<Doviz> CurList = null;
            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString("http://www.doviz.com/api/v1/currencies/all/latest");
                CurList = JsonConvert.DeserializeObject<List<Doviz>>(json);
            }
            Doviz dov = new Doviz();

            if (CurList == null)
                return;

           foreach(Doviz d in CurList)
            {
                comboBox1.Items.Add(d.code);
                comboBox2.Items.Add(d.code);
                comboBox3.Items.Add(d.code);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
