using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
// https://stackoverflow.com/questions/6620165/how-can-i-parse-json-with-c
using Newtonsoft.Json.Linq;

namespace NewsApi
{
    public partial class NewsApi : Form
    {
        private HttpClient client = new HttpClient();

        
        public NewsApi()
        {
            InitializeComponent();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            string jsonNews = await client.GetStringAsync(new Uri(
                "https://newsapi.org/v2/top-headlines?country=us&apiKey=5cb35fea7dfd4098abd2498570bdcfb9"));
            dynamic stuff = JObject.Parse(jsonNews);            
        }
    }
}
