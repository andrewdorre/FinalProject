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
        List<NewsHeader> newsList = new List<NewsHeader>();

        public NewsApi()
        {
            InitializeComponent();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {            
            string jsonNews = await client.GetStringAsync(new Uri(
                "https://newsapi.org/v2/top-headlines?country=us&apiKey=5cb35fea7dfd4098abd2498570bdcfb9"));
            dynamic stuff = JObject.Parse(jsonNews);

            
            var labelURL = new List<dynamic>();

            List<Label> labelTitle = new List<Label>();
            List<Label> labelDate = new List<Label>();
            List<Label> labelSource = new List<Label>();
            //List<LinkClickedEventArgs> linkClicked = new List<LinkClickedEventArgs>();

            //int n = 1;
            // Used bottom comment, although their syntax wasn't correct
            // https://stackoverflow.com/questions/37480105/how-can-i-add-several-labels-to-a-list-in-a-for-loop
            // Thinking I could group the labels by type... source, date, title
            for (int n = 1; n < 11; n++)
            {
                // https://stackoverflow.com/questions/1536739/get-a-windows-forms-control-by-name-in-c-sharp
                string panel = "panel" + n.ToString();
                var ctn = this.Controls.Find(panel, false).FirstOrDefault();

                foreach (Label control in ctn.Controls)
                {
                    if (control.Name.StartsWith("linkLabel"))
                        labelTitle.Add(control);

                    if (control.Name.StartsWith("lblDate"))
                        labelDate.Add(control);

                    if (control.Name.StartsWith("lblSource"))
                        labelSource.Add(control);
                }
            }           
            /*foreach (Label control in panel1.Controls)
            {
                if (control.Name.StartsWith("linkLabel"))                
                    labelTitle.Add(control);
                
                if (control.Name.StartsWith("lblDate"))                
                    labelDate.Add(control);
                
                if (control.Name.StartsWith("lblSource"))                
                    labelSource.Add(control);
            }*/
            /*foreach (Label control in groupBox1.Controls)
            {
                string linkName = "linkLabel" + n;                

                if (control.Name.StartsWith("linkLabel"))
                {                    
                    labelTitle.Add(control);                    
                }
                else if (control.Name.StartsWith("lblDate"))
                {
                    labelDate.Add(control);
                }
                else if (control.Name.StartsWith("lblSource"))
                {
                    labelSource.Add(control);
                }
            }*/
            

            // Was trying to find the number of articles using .Length at first, but that doesn't work
            // Used .Count instead: https://stackoverflow.com/questions/19025174/get-length-of-array-json-net
            for (int i = 0; i < (stuff.articles).Count; i++)
            {
                NewsHeader testHeader = new NewsHeader();
                testHeader.NewsTitle = stuff.articles[i].title;
                testHeader.NewsDate = stuff.articles[i].publishedAt;
                testHeader.NewsSource = stuff.articles[i].source.name;
                testHeader.NewsLink = stuff.articles[i].url;
                newsList.Add(testHeader);
            }            

            for (int i = 1; i < 11; i++)
            {
                labelTitle[i].Text = newsList[i].NewsTitle;
                //labelURL[i] = newsList[i].NewsLink;
                labelDate[i].Text = newsList[i].NewsDate;
                labelSource[i].Text = newsList[i].NewsSource;

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(newsList[0].NewsLink);
        }
    }
}
