using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Döviz_Programı
{
    public partial class Form1 : Form
    {
        Point defaultPoint;
        //StreamReader sr = new StreamReader("currencies.txt");
        string[] kurlar = {"","",""};
        string ProgramAdi = "Döviz Programı";
        Timer t = new Timer();
        private void Form1_Load(object sender, EventArgs e)
        {
            defaultPoint = new Point(1400, 826);
            this.Location = defaultPoint;
            t.Tick += T_Tick;
            t.Start();

            FileStream fs = new FileStream("currencies.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            String dosya = sr.ReadToEnd();
            var dosya2 = dosya.ToCharArray();

            fs.Close();
            sr.Close();

            for(int i = 0 ; i < 3 ; i++)
            {
                kurlar[0] += dosya2[i];
                kurlar[1] += dosya[i+4];
                kurlar[2] += dosya[i+8];
            }
            label1.Text = kurlar[0] + " :";
            label2.Text = kurlar[1] + " :";
            label3.Text = kurlar[2] + " :";
        }
        public Form1()
        {
            InitializeComponent();
            t.Interval = 1000;
            this.Location = new Point(1400,826);
        }
        void kurlarSatis()
        {
            List<Doviz> CurList = null;
            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString("http://www.doviz.com/api/v1/currencies/all/latest");
                CurList = JsonConvert.DeserializeObject<List<Doviz>>(json);
            }

            if (CurList == null)
                return;
            foreach(Doviz d in CurList)
            {
                if (d.code == kurlar[0])
                {

                    dolar.Text = String.Format("{0:0.000}", d.buying);
                    if (d.change_rate > 0)
                        dolar.ForeColor = Color.ForestGreen;
                    else
                        dolar.ForeColor = Color.IndianRed;
                }
                if (d.code == kurlar[1])
                {
                    euro.Text = String.Format("{0:0.000}", d.buying);
                    if (d.change_rate > 0)
                        euro.ForeColor = Color.ForestGreen;
                    else
                        euro.ForeColor = Color.IndianRed;
                }
                if (d.code == kurlar[2])
                {
                    pound.Text = String.Format("{0:0.000}", d.buying);
                    if (d.change_rate > 0)
                        pound.ForeColor = Color.ForestGreen;
                    else
                        pound.ForeColor = Color.IndianRed;
                }
            }

        }
        void kurlarAlis()
        {
            List<Doviz> CurList = null;
            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString("http://www.doviz.com/api/v1/currencies/all/latest");
                CurList = JsonConvert.DeserializeObject<List<Doviz>>(json);
            }

            if (CurList == null)
                return;
            foreach (Doviz d in CurList)
            {
                if (d.code == kurlar[0])
                {
                    label4.Text = string.Format("{0:0.000}", d.selling);
                }

                if (d.code == kurlar[1])
                {
                    label5.Text = string.Format("{0:0.000}",d.selling);
                }
                if (d.code == kurlar[2])
                {
                    label6.Text = string.Format("{0:0.000}", d.selling);
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                this.TopMost = true;
            else
                this.TopMost = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            KurListesi kl = new KurListesi();
            kl.ShowDialog();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            KurSecim ks = new KurSecim();
            if (ks.ShowDialog() == DialogResult.OK)
            {
                label1.Text = string.Format("{0} : ",ks.comboBox1.Text);
                label2.Text = string.Format("{0} : ",ks.comboBox2.Text);
                label3.Text = string.Format("{0} : ",ks.comboBox3.Text);

                kurlar[0] = ks.comboBox1.Text;
                kurlar[1] = ks.comboBox2.Text;
                kurlar[2] = ks.comboBox3.Text;

                List<Doviz> CurList = null;
                using (WebClient client = new WebClient())
                {
                    var json = client.DownloadString("http://www.doviz.com/api/v1/currencies/all/latest");
                    CurList = JsonConvert.DeserializeObject<List<Doviz>>(json);
                }

                if (CurList == null)
                    return;

                foreach (Doviz d in CurList)
                {
                    if (d.code == kurlar[0])
                    {
                        dolar.Text =  String.Format("{0:0.000}", d.buying);
                        label4.Text = String.Format("{0:0.000}", d.selling);
                    }
                    if(d.code == kurlar[1])
                    {
                        euro.Text = String.Format("{0:0.000}", d.buying);
                        label5.Text = String.Format("{0:0.000}", d.selling);
                    }
                    if(d.code == kurlar[2])
                    {
                        pound.Text = String.Format("{0:0.000}", d.buying);
                        label6.Text = String.Format("{0:0.000}", d.selling);
                    }

                }
                
            }
        }
        private void T_Tick(object sender, EventArgs e)
        {
            kurlarSatis();
            kurlarAlis();
            if (NetworkInterface.GetIsNetworkAvailable() == true)
            {
                label7.Text = "Online";
                label7.ForeColor = Color.LimeGreen;
            }
            else
            {
                label7.Text = "Offline";
                label7.ForeColor = Color.LightCoral;
            }
        }
        private string addZero(int p)
        {
            if (p.ToString().Length == 1)
                return "0" + p;
            return p.ToString();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText("currencies.txt", String.Empty);
            FileStream fs = new FileStream("currencies.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(kurlar[0] + " " + kurlar[1] + " " + kurlar[2]);
            sw.Close();
            fs.Close();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (checkBox2.Checked==true)
            {
                key.SetValue(ProgramAdi, "\"" + Application.ExecutablePath + "\"");
            }
            else
            {
                key.DeleteValue(ProgramAdi);
            }
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            this.Location = defaultPoint;
        }
    }
}
