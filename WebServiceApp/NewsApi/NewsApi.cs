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
            dynamic jsonTest = JObject.Parse("{ 'Name': 'Jon Smith', 'Address': { 'City': 'New York', 'State': 'NY' }, 'Age': 42 }");
            string jsonNews = await client.GetStringAsync(new Uri(
                "https://newsapi.org/v2/top-headlines?country=us&apiKey=5cb35fea7dfd4098abd2498570bdcfb9"));
            dynamic stuff = JObject.Parse(jsonNews);

            //textBox1.AppendText(stuff);
            //string name = jsonTest.Name;
            //string address = jsonTest.Address.City;
            //textBox1.AppendText(name);
            //textBox1.AppendText(address);

            //NewsHeader testHeader = new NewsHeader();
            //testHeader.NewsTitle = stuff.articles[0].title;
            //testHeader.NewsDate = stuff.articles[0].publishedAt;
            //testHeader.NewsSource = stuff.articles[0].source.name;
            //testHeader.NewsLink = stuff.articles[0].url;
            //string article = string.Format("Title: {0} Publish Date: {1} Source: {2} Link: ",testHeader.NewsTitle, testHeader.NewsDate, testHeader.NewsSource, testHeader.NewsLink);
            //string article = stuff.articles[0].title;
            //textBox1.AppendText(article);

            // Was trying to find the number of articles using .Length at first, but that doesn't work
            // Used .Count instead: https://stackoverflow.com/questions/19025174/get-length-of-array-json-net
            for (int i = 0; i < (stuff.articles).Count; i++)
            {
                string article = stuff.articles[i].title;
                textBox1.AppendText(article);
            }
        }
    }
}
