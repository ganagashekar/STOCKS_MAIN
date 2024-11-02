using Breeze;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BreezeAPIBUY
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string APIKEY = string.Empty;
                string APISecret = string.Empty;
                string token = string.Empty;
                var url = "http://localhost:99/breezeOperation";
                var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\jobskeys.txt");
                string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                string HUbUrl = url;


                string[] line;
                string arg = "0";


                switch (0)
                {
                    case 0:
                        line = lines[0].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        HUbUrl = "http://localhost:8080/BreezeOperation";
                        break;
                }

                BreezeConnect breeze = new BreezeConnect(APIKEY);
                breeze.generateSession(APISecret, token);

                var datsa = System.Text.Json.JsonSerializer.Serialize(breeze.getQuotes("NIFTY", "NFO".ToString(), "2024-10-31", "Options".ToString(), "Others".ToString(), ""));
                //var details=breeze.getFunds();
               // string Expdate = dateTimePicker1.Text;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
